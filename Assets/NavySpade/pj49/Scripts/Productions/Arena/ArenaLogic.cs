using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Guirao.UltimateTextDamage;
using Main.UI;
using NavySpade.Core.Runtime.Levels;
using NavySpade.pj46.UI.PoppingText;
using NavySpade.pj49.Scripts.BaseFunctionsFromMVC;
using NavySpade.pj49.Scripts.Items;
using NavySpade.pj49.Scripts.Items.Inventory;
using NavySpade.pj49.Scripts.Productions.Arena.Boss;
using NavySpade.pj49.Scripts.Productions.Factory;
using NavySpade.pj49.Scripts.Productions.Gym;
using NavySpade.pj49.Scripts.Saving;
using NavySpade.pj49.Scripts.UnitsQueues.Positions;
using UniRx;
using Unity.Mathematics;
using UnityEngine;


namespace NavySpade.pj49.Scripts.Productions.Arena{
    public class ArenaLogic : BaseMonoController, ISaveable{
        [SerializeField]
        private ResourcesInventory _inventory;
        [SerializeField]
        private ArenaConfig _arenaSettings;
        [SerializeField]
        private GymLogic _gym;
        [SerializeField]
        private UnitQueue _unitsQueue;
        [SerializeField]
        private CounterView _counterView;
        [SerializeField]
        private Progressbar _bossProgressbar;
        [SerializeField]
        private Transform _throwTarget;
        [SerializeField]
        private Transform _popupTarget;
        [SerializeField]
        private CirclePoints _inputZone;

        [SerializeField]
        private BossController _bossController;
        [SerializeField]
        private KeyController _keyController;
        [SerializeField]
        private GameEndViewController _gameEndController;

        [SerializeField]
        private GameObjectTimeFade[] _objectsForEnableAfterWin;
        [SerializeField]
        private GameObjectTimeFade[] _objectsForDisableAfterWin;

        [SerializeField]
        private ParticleSystem _puff;


        #region PrivateData

        private ArenaModel _arenaModel;
        private UiProgressbar _uiProgressbar;
        private readonly int _attackAnim = Animator.StringToHash("Attack");
        private List<BattleUnitArena> _listHpUnits = new List<BattleUnitArena>();
        private int _oldUnitsHp;
        private PopapingTextManager _popupManager;
        private float _popupDelta;
        private float _deltaDamage;
        private int _oldBossHp;
        private UltimateTextDamageManager _ultimateTextDamageManager;

        #endregion


        public event Action ArenaInitialise;
        public ArenaConfig Settings => _arenaSettings;
        public ArenaModel Model => _arenaModel;
        public ResourcesInventory Inventory => _inventory;
        public Transform ThrowTarget => _throwTarget;
        public bool IsWeakUnit{
            get{
                if (_listHpUnits.Count < _arenaModel.InputMaxCount) return true;

                for (int i = 0; i < _listHpUnits.Count; i++){
                    var unit = _listHpUnits[i];
                    var maxAttack = 0;
                    maxAttack = !_gym ? 1 : _gym.MuscleUnitPowerAttack;
                    if (unit.Attack < maxAttack){
                        KillUnit(unit.Unit);
                        return true;
                    }
                }

                return false;
            }
        }

        public void Init(){
            LevelSaver.Instance.Register(this);

            ArenaSavingData savingData = LevelSaver.LoadArenaSaving();
            _inventory.Init(savingData.Items);

            _arenaModel = new ArenaModel();
            _arenaModel.Init(_arenaSettings, savingData);

            if (!_uiProgressbar) _uiProgressbar = FindObjectOfType<UiProgressbar>();
            if (!_popupManager) _popupManager = FindObjectOfType<PopapingTextManager>();
            _ultimateTextDamageManager = FindObjectOfType<UltimateTextDamageManager>();
            
            
            _inputZone.GetPositions(_arenaModel.InputZoneSize.PointCount, _arenaModel.InputZoneSize.Radius);
            _arenaModel.BossHp.Subscribe(v => {
                // CuctAveregeDamage(v);
                ShowDamage(v);
                _bossProgressbar.UpdateProgressbar(v);
                _uiProgressbar.UpdateProgressbar(v);
            }).AddTo(_subscriptions);
            // _arenaModel.HealthPoints.Subscribe(v => _counterView.FormatHp = v).AddTo(_subscriptions);
            // _arenaModel.UnitsHp.Subscribe(OnChangeUnitsHp).AddTo(_subscriptions);

            _arenaModel.OnBossDie.Subscribe(OnBossDie).AddTo(_subscriptions);
            _arenaModel.OnBossDie.Subscribe(EndingLevel).AddTo(_subscriptions);

            InitProgressBar(_arenaSettings.VictoryConditionsByLevels.GetIntValue(LevelManager.CurrentLevelIndex), _arenaModel.BossHp.Value);
            _keyController.Init(_arenaModel, Settings.KeyPrefab);
            _bossController.Init(_arenaModel, Settings);
            _gameEndController.Init(_arenaModel);
            

            ArenaInitialise?.Invoke();
            _arenaModel.Units.ChangeListUnits += FillListUnits;

            _deltaDamage = 0;
            // StartCoroutine(StartPopup());
        }

        private void ShowDamage(int value){
            _deltaDamage = _oldBossHp - value;
            _ultimateTextDamageManager.Add($"-{_deltaDamage}", _popupTarget);
            _oldBossHp = value;
        }

        private IEnumerator StartPopup(){
            _popupDelta = 0f;
            _deltaDamage = 0;
            while (true){
                yield return new WaitForSeconds(Time.deltaTime);
                _popupDelta += Time.deltaTime;
                if (_popupDelta > 1){
                    _popupDelta = 0;
                    if(_deltaDamage > 1)
                        ShowPopup(_deltaDamage);
                    _deltaDamage = 0;
                }
            }
        }

        private void CuctAveregeDamage(int value){
            _deltaDamage += _oldBossHp - value;
            _oldBossHp = value;
        }

        private void ShowPopup(float value){
            var obj = _popupManager.Pool.Get();
            obj.OnTakeFromPool();

            obj.Initilaze($"-{value:F0}", _popupTarget.transform, animationOffset: new Vector3(0,0.001f,0));
            // obj.Initilaze($"{value:C1}", _counterView.transform);
        }

        private void OnBossDie(bool die){
            if (!die) return;

            foreach (var unit in _listHpUnits){
                KillUnit(unit.Unit);
            }
        }

        private void EndingLevel(bool die){
            if (!die) return;

            foreach (var gameObj in _objectsForEnableAfterWin){
                StartCoroutine(SetVisible(gameObj, true));
            }

            foreach (var gameObj in _objectsForDisableAfterWin){
                StartCoroutine(SetVisible(gameObj, false));
            }
            
            _inputZone.gameObject.SetActive(false);
        }

        private IEnumerator SetVisible(GameObjectTimeFade gameObj, bool state){
            yield return new WaitForSeconds(gameObj.DeltaTime);
            gameObj.GameObject.SetActive(state);
        }

        private void FillListUnits(List<CollectingUnit> list){
            _listHpUnits = new List<BattleUnitArena>();
            foreach (var unit in list){
                if (unit is MuscleCollectingUnit){
                    _listHpUnits.Add(new BattleUnitArena(){
                        Unit = unit,
                        // Hp = //_gymConfig.MuscleUnitHealthPoints.GetIntValue(LevelManager.CurrentLevelIndex),
                        Attack = _gym
                            .MuscleUnitPowerAttack //_gymConfig.MuscleUnitPowerAttack.GetIntValue(LevelManager.CurrentLevelIndex)
                    });
                } else{
                    _listHpUnits.Add(new BattleUnitArena(){
                        Unit = unit,
                        Hp = 1,
                        Attack = 1
                    });
                }
            }

            _arenaModel.UnitsHp.Value = list.Sum(unit => unit.UnitResource.Multiplier);
        }

        private void InitProgressBar(int maxValue, int value){
            _bossProgressbar.SetupProgressbar(maxValue, value);
            _uiProgressbar.SetupProgressbar(maxValue, value);
        }

        public void Save(){
            ArenaSavingData savingData = new ArenaSavingData();
            savingData.HealthPoints = _arenaModel.BossHp.Value;
            LevelSaver.SaveArena(savingData);
        }

        private void OnChangeUnitsHp(int value){
            if (value >= _oldUnitsHp){
                _oldUnitsHp = value;
                return;
            }

            if (_listHpUnits == null)
                FillListUnits(_arenaModel.Units.ListUnits);
            var damage = _oldUnitsHp - value;
            while (damage > 0){
                var currentUnit = _listHpUnits[_listHpUnits.Count - 1];

                var balanceOfDamage = damage - currentUnit.Hp;
                currentUnit.Hp -= damage;
                if (currentUnit.Hp <= 0){
                    KillUnit(currentUnit.Unit);
                    _listHpUnits.Remove(currentUnit);
                }

                damage = balanceOfDamage;
            }


            _oldUnitsHp = value;
        }

        private void BossDamagedOn(CollectingUnit attacker){
            foreach (var battleUnitArena in _listHpUnits){
                if (battleUnitArena.Unit == attacker){
                    _arenaModel.BossHp.Value -= battleUnitArena.Attack;
                }
            }
        }

        public void AddedUnit(CollectingUnit unit){
            unit.transform.LookAt(transform.position);

            StartCoroutine(AttackUnit(unit));

            _arenaModel.UnitsHp.Value += unit.UnitResource.Multiplier;
        }

        private IEnumerator AttackUnit(CollectingUnit unit){
            while (_arenaModel.BossHp.Value > 0){
                if (!unit) yield break;
                unit.gameObject.GetComponentInChildren<Animator>().SetTrigger(_attackAnim);
                CreateAndThrowBullet(owner: unit, Settings.RandomBullet(), unit.transform, ThrowTarget.position);
                yield return new WaitForSeconds(2);
            }
        }

        private void CreateAndThrowBullet(CollectingUnit owner, FlyingBullet original, Transform startPos,
            Vector3 endPos){
            var bullet = Instantiate(original, new Vector3(
                    startPos.position.x,
                    startPos.position.y + 1.5f,
                    startPos.position.z),
                quaternion.identity, startPos);
            bullet.Owner = owner;
            bullet.OnCollision += BulletOnOnCollision;
            bullet.ThrowTo(endPos);
        }

        private void BulletOnOnCollision(FlyingBullet bullet){
            bullet.OnCollision -= BulletOnOnCollision;
            StartCoroutine(CreatePuffEffect(_puff, bullet.transform.position));
            BossDamagedOn(bullet.Owner);
        }

        private IEnumerator CreatePuffEffect(ParticleSystem puff, Vector3 position){
            var effect = Instantiate(puff, position, quaternion.identity).GetComponent<ParticleSystem>();
            effect.Play();
            yield return new WaitForSeconds(2.0f); //Magic number 8-) Yes, is`t true, because I don`t give a fuck
            Destroy(effect.gameObject);
        }

        private void KillUnit(CollectingUnit unit){
            StartCoroutine(CreatePuffEffect(_puff, unit.transform.position));
            _arenaModel.Units.RemoveFromQueue(unit, false);
            _unitsQueue.RemoveFromQueue(unit, true);
            unit.DestroySelf();
        }
    }

    internal class BattleUnitArena{
        public CollectingUnit Unit;
        public int Hp;
        public int Attack;
    }

    [Serializable] public class GameObjectTimeFade{
        [SerializeField]
        public GameObject GameObject;
        [Range(0, 5)]
        [SerializeField]
        public float DeltaTime;
    }
}