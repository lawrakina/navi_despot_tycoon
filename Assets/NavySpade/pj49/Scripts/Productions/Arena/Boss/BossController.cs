using System;
using System.Collections;
using Mono.CSharp;
using NavySpade.pj49.Scripts.BaseFunctionsFromMVC;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;


namespace NavySpade.pj49.Scripts.Productions.Arena.Boss{
    public class BossController : BaseMonoController{
        [SerializeField]
        private BossView _view;
        private ArenaModel _model;
        private ArenaConfig _settings;
        private BossState _bossState;
        private float _leftTime = 0;
        private bool _isOn = true;

        public void Init(ArenaModel model, ArenaConfig settings){
            _model = model;
            _settings = settings;
            // _view = Instantiate(settings.GetBossOnCurrentLevel(), placeForBoss,false).GetComponent<BossView>();

            _model.BossHp.Subscribe(ChangeHp).AddTo(_subscriptions);
            _model.UnitsHp.Subscribe(ChangeCountUnit).AddTo(_subscriptions);

            _view.SetState(BossState.Idle);
        }

        private IEnumerator SetBossState(BossState state){
            yield return new WaitForSeconds(0.5f);
            _bossState = state;
            _view.SetState(state);
            _view.HitFlag(false);
        }

        private void ChangeHp(int value){
            if(!_isOn) return;
            
            if (value <= 0){
                _bossState = BossState.Dead;
                _view.SetState(_bossState);
                _model.OnBossDie.Value = true;
                _isOn = false;
            } 
            if(value > 0){
                _bossState = BossState.Hit;
                _view.SetState(_bossState);
                _view.HitParam = Random.Range(0, 2);
            }
        }

        private void ChangeCountUnit(int index){
            if(!_isOn) return;
            if(_bossState == BossState.Dead) return;

            if (index <= 0){
                if (_bossState != BossState.Idle){
                    _bossState = BossState.Idle;
                }
            }

            if (index > 0){
                if (_bossState != BossState.Hit)
                    _bossState = BossState.Hit;
            }

            //Boss only takes damage
            if (index > 0){
                if(_bossState != BossState.Hit)
                    _bossState = BossState.Hit;
            }
            _view.SetState(_bossState);
            // //Boss does not attack
            // return;
            // if (index > 0){
            //     if (_leftTime <= 0){
            //         _leftTime = _model.BossAttackSpeed;
            //     }
            //     _bossState = BossState.Angry;
            // }
        }

        private void Update(){
            if(!_isOn) return;
            
            //Boss does not attack
            return;
            // if (_bossState == BossState.Angry && _model.Units.ListUnits.Count > 0){
            //     _leftTime -= Time.deltaTime;
            //     if (_leftTime < 0){
            //         _leftTime = _model.BossAttackSpeed;
            //         _view.SetState(BossState.Angry);
            //         // StartCoroutine(WaitingRotation());
            //         _view.transform.LookAt(_model.Units.GetLastInQueue().transform);
            //         StartCoroutine(WaitingAttack());
            //     }
            // }
        }

        private IEnumerator WaitingRotation(){
            var time = _model.DelayBeforeAttack;
            while (time>0){
                time -= Time.deltaTime;
                _view.transform.rotation = Quaternion.Lerp(_view.transform.rotation, 
                    Quaternion.Euler(_model.Units.GetLastInQueue().transform.position - _view.transform.position), 
                    Time.deltaTime * 10);
                yield return new WaitForEndOfFrame();
            }
            _view.transform.LookAt(_model.Units.GetLastInQueue().transform);
        }

        private IEnumerator WaitingAttack(){
            yield return new WaitForSeconds(_model.DelayBeforeAttack);
            _model.UnitsHp.Value-= _model.BossAttackValue;
            if (_model.UnitsHp.Value <= 0)
                _bossState = BossState.Idle;
        }
    }

    public enum BossState{
        Idle,
        // Angry,
        Dead,
        Hit
    }
}