using System.Collections.Generic;
using System.Linq;
using NavySpade.pj49.Scripts.Items;
using NavySpade.pj49.Scripts.Navigation;
using NavySpade.pj49.Scripts.Productions.Factory;
using NavySpade.pj49.Scripts.Productions.ProductionStates;
using NavySpade.pj49.Scripts.UnitsQueues;
using UnityEngine;


namespace NavySpade.pj49.Scripts.Productions.Gym{
    public class GymLogic : FactoryStateMachine{
        [SerializeField]
        private UnitQueue _inputQueue;
        [SerializeField]
        private UnitQueue _outputQueue;
        [SerializeField]
        private GameObject _puffEffect;
        [SerializeField]
        private Transform _spawnPoint;

        [SerializeField]
        private Transform[] _barbellsPoints;
        [SerializeField]
        private AIPoint[] _studyPoints;

        private Factory.Factory _factory;

        public float ResourceLifeTime => _factory.Stats.ProductionTime;

        private List<UnitGymBehaviour> _unitsInProduction = new List<UnitGymBehaviour>();
        private List<UnitGymBehaviour> _unitsInProgress = new List<UnitGymBehaviour>();
        private List<BusyCellForListUnits> _listEquipments;
        public int TotalUnitInProgress => _unitsInProduction.Count + _unitsInProgress.Count;
        public int MuscleUnitPowerAttack => _factory.Stats.MuscleUnitPowerAttack;

        private void Start(){
            _factory = GetComponent<Factory.Factory>();

            CheckEnabledBarbell(_factory.Stats.ParallelProductionCount);

            _factory.ProductionStarted += StartProductionFlow;
            _factory.Stats.Upgraded += StatsOnUpgraded;
        }

        private void CheckEnabledBarbell(int statsParallelProductionCount){
            _listEquipments = new List<BusyCellForListUnits>();
            for (var i = 0; i < _barbellsPoints.Length; i++){
                _barbellsPoints[i].gameObject.SetActive(i < statsParallelProductionCount);
                _listEquipments.Add(new BusyCellForListUnits()
                    {studyPoint = _studyPoints[i], BarbellPoint = _barbellsPoints[i]});
            }
        }

        private void StartProductionFlow(){
            if (TotalUnitInProgress >= _factory.Stats.ParallelProductionCount) return;

            MoveUnitsToGym();
        }

        private void UpdateSmt(UnitGymBehaviour resource){
            switch (resource.State){
                case FactoryStates.MoveToProductionPoint:
                    break;
                case FactoryStates.Producing:
                    _unitsInProgress.Remove(resource);
                    _unitsInProduction.Add(resource);
                    break;
                case FactoryStates.Destroying:
                    _unitsInProduction.Remove(resource);
                    var barbell = _listEquipments.FirstOrDefault(x => resource.Compare(x.Unit));
                    if(barbell != null)
                        barbell.Unit = null;
                    _factory.InProductionProcess = TotalUnitInProgress >= _factory.Stats.ParallelProductionCount;
                    TryAddUnit();
                    break;
                case FactoryStates.Finish:
                    break;
            }
        }

        private void MoveUnitsToGym(){
            for (int i = 0; i < _factory.GetResourceCountToStart(); i++){
                CollectingUnit unit = _inputQueue.GetFirstInQueue();

                var gymBehaviour = unit.GetComponent<UnitGymBehaviour>();
                gymBehaviour.Init(this);
                gymBehaviour.StateChanged += UpdateSmt;
                gymBehaviour.SetState(FactoryStates.MoveToProductionPoint);
                _inputQueue.RemoveFromQueue(unit);
                _unitsInProgress.Add(gymBehaviour);

                _factory.InProductionProcess = TotalUnitInProgress >= _factory.Stats.ParallelProductionCount;
            }
        }

        // private IEnumerator Producing()
        // {
        //     _gymAnimator.SetBool(_workHash, true);
        //
        //     _factory.ElapsedTimeOfProduction = 0;
        //     for (int i = 0; i < _factory.Stats.OutputCount; i++)
        //     {
        //         _currentProductionTime = 0;
        //         while (_currentProductionTime < _factory.Stats.ProductionTime / _factory.Stats.OutputCount)
        //         {
        //             _currentProductionTime += Time.deltaTime * _factory.ProductionBoost;
        //             _factory.ElapsedTimeOfProduction += Time.deltaTime * _factory.ProductionBoost;
        //             yield return null;
        //         }
        //
        //         TryAddUnit();
        //     }
        //
        //     _gymAnimator.SetBool(_workHash, false);
        //     _factory.InProductionProcess = false;
        // }

        private void TryAddUnit(){
            if (_factory.HasFreeSpaceForResources(_factory.Config.OutputResources)){
                AddUnitToZone();
            }
        }

        private void AddUnitToZone(){
            var unit = Instantiate(_factory.Config.OutputResources.Prefab,
                _spawnPoint.position,
                _spawnPoint.rotation,
                UnitsManager.Instance.transform).GetComponent<CollectingUnit>();

            unit.BlockCollect = true;

            _outputQueue.AddUnitToQueue(unit);
            _factory.ProduceItem();

            Instantiate(_puffEffect, unit.transform.position, Quaternion.identity);
        }

        private void OnDestroy(){
            _factory.ProductionStarted -= StartProductionFlow;
            _factory.Stats.Upgraded -= StatsOnUpgraded;
        }

        private void StatsOnUpgraded(FactoryUpgradeParameter arg1, int arg2){
            CheckEnabledBarbell(_factory.Stats.ParallelProductionCount);
        }

        public (Transform studyPoint, Transform barbells) GetPositionOnBarbells(CollectingUnit unit){
            var result = _listEquipments.FirstOrDefault(x => x.Unit == null);
            result.Unit = unit;
            return (result.studyPoint.transform, result.BarbellPoint);
        }
    }
}