using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NavySpade.pj49.Scripts.Items;
using NavySpade.pj49.Scripts.Productions.ProductionStates;
using UnityEngine;


namespace NavySpade.pj49.Scripts.Productions.EnergyFactory{
    internal class UnitElectroPositions{
        public UnitElectroStationBehaviour Unit;
        public Transform Position;
    }

    public class ElectroStationStateMachine : FactoryStateMachine{
        [Tooltip("Первые более приоритетные")]
        [SerializeField]
        private Transform[] _pointsInCircle;

        [Header("Visual")]
        [SerializeField]
        private UnitQueue _unitsQueue;
        [SerializeField]
        private Animator _wheelAnimator;
        [SerializeField]
        private GameObject[] _effects;
        [SerializeField]
        private BatteryView _battery;

        [SerializeField]
        private GameObject _bloodSplash;


        #region PrivateData

        private readonly int _wheelWorkState = Animator.StringToHash("Work");
        private List<UnitElectroStationBehaviour> _unitsInProgress = new List<UnitElectroStationBehaviour>();
        private List<UnitElectroStationBehaviour> _unitsInProduction = new List<UnitElectroStationBehaviour>();
        private List<UnitElectroPositions> _listPositions;

        #endregion


        private int TotalUnitsOnCircle => _unitsInProduction.Count + _unitsInProgress.Count;

        public bool IsWork{ get; private set; }

        public float BoostValue => _factory.Stats.GetValueOf(_factory.Config.OutputCount);

        public float ResourceLifeTime => _factory.Stats.ProductionTime;

        private void Start(){
            _battery.gameObject.SetActive(false);
            _factory.ProductionStarted += StartProductionFlow;
            UpdateEffects();

            _listPositions = new List<UnitElectroPositions>();
            foreach (var transform1 in _pointsInCircle){
                _listPositions.Add(new UnitElectroPositions{Unit = null, Position = transform1});
            }
        }

        private void OnDestroy(){
            _factory.ProductionStarted -= StartProductionFlow;
        }

        private void StartProductionFlow(){
            if (TotalUnitsOnCircle >= _pointsInCircle.Length)
                return;

            CollectingUnit unit = _unitsQueue.GetFirstInQueue();
            UnitElectroStationBehaviour stateMachine = unit.GetComponent<UnitElectroStationBehaviour>();
            stateMachine.Init(this);
            stateMachine.StateChanged += UpdateSmt;
            stateMachine.SetState(FactoryStates.MoveToProductionPoint);
            _unitsQueue.RemoveFromQueue(unit);
            _unitsInProgress.Add(stateMachine);
        }

        private void UpdateSmt(UnitElectroStationBehaviour resource){
            switch (resource.State){
                case FactoryStates.MoveToProductionPoint:
                    break;
                case FactoryStates.Producing:
                    _unitsInProgress.Remove(resource);
                    _unitsInProduction.Add(resource);
                    _wheelAnimator.SetBool(_wheelWorkState, _unitsInProduction.Count > 0);
                    IsWork = true;
                    UpdateEffects();
                    break;
                case FactoryStates.Destroying:
                    RemoveUnitOnLists(resource);
                    Instantiate(_bloodSplash, resource.transform.position, Quaternion.identity);
                    _wheelAnimator.SetBool(_wheelWorkState, _unitsInProduction.Count > 0);
                    StartCoroutine(PlayDestroyingFlow());
                    StartCoroutine(ProduceInits());
                    CheckWorkState();
                    break;
                case FactoryStates.Finish:
                    break;
            }
        }

        private void RemoveUnitOnLists(UnitElectroStationBehaviour resource){
            foreach (var electroPosition in _listPositions.Where(electroPosition =>
                electroPosition.Unit == resource))
                electroPosition.Unit = null;
            _unitsInProduction.Remove(resource);
        }

        private IEnumerator ProduceInits(){
            yield return new WaitForSeconds(_battery.DurationClip);
            _factory.ProduceItem();
        }

        private IEnumerator PlayDestroyingFlow(){
            _battery.gameObject.SetActive(true);
            yield return new WaitForSeconds(_battery.DurationClip);
            _battery.gameObject.SetActive(false);
            yield return null;
        }

        public Transform GetPositionInCircle(UnitElectroStationBehaviour unit){
            Transform result = transform;
            foreach (var electroPosition in _listPositions.Where(electroPosition =>
                electroPosition.Unit == null)){
                electroPosition.Unit = unit;
                return electroPosition.Position;
            }

            return result;
        }

        private void CheckWorkState(){
            IsWork = _unitsInProduction.Count > 0 && _unitsQueue.UnitsInQueue > 0;
            UpdateEffects();
        }

        private void UpdateEffects(){
            foreach (var effect in _effects){
                effect.SetActive(IsWork);
            }
        }
    }
}