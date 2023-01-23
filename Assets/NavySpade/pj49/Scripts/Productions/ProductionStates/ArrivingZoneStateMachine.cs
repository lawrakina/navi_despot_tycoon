using System.Collections;
using NavySpade.pj49.Scripts.Items;
using NavySpade.pj49.Scripts.Items.Inventory;
using NavySpade.pj49.Scripts.Tutorial;
using NavySpade.pj49.Scripts.UnitsQueues;
using UnityEngine;


namespace NavySpade.pj49.Scripts.Productions.ProductionStates{
    public class ArrivingZoneStateMachine : FactoryStateMachine{
        [Header("Bus")]
        [SerializeField]
        private Bus _bus;
        [SerializeField]
        private Transform _startPoint;
        [SerializeField]
        private Transform _endPoint;
        [SerializeField]
        private Transform _stopPoint;
        [SerializeField]
        private float _delayForBus;
        [SerializeField]
        private float _delayForBarrier;

        [Header("SpawnUnits")]
        [SerializeField]
        private CollectingUnit _unitPrefab;
        [SerializeField]
        private int _forTutorCountUnitsAfterStart = 4;
        [SerializeField]
        private float _spawnDelay;
        [SerializeField]
        private Transform _unitSpawnPoint;
        [SerializeField]
        private UnitQueue _unitsQueue;
        [SerializeField]
        private GameObject _puffEffect;
        [SerializeField]
        private Animator _barrierAnimator;

        private readonly int _isOpenHash = Animator.StringToHash("IsOpen");
        private bool _isSecondSpawn;

        private void Start(){
            _factory.ProductionStarted += StartProductionFlow;
        }

        private void OnDestroy(){
            _factory.ProductionStarted -= StartProductionFlow;
        }

        // public void Init()
        // {
        //     SpawnUnits();
        // }
        //
        // private void SpawnUnits()
        // {
        //     Item unitItem = _factory.Inventory.GetItem(_unitPrefab.UnitResource);
        //     for (int i = 0; i < unitItem.Amount; i++)
        //     { 
        //         var unit = Instantiate(_unitPrefab, UnitsManager.Instance.transform);
        //         _unitsQueue.AddUnitToQueueInstant(unit);
        //     }
        // }

        private void StartProductionFlow(){
            StopAllCoroutines();

            if (_isSecondSpawn == false &&
                TutorialController.InstanceExists &&
                TutorialController.Instance.TutorDone == false){
                if (_unitsQueue.ListUnits.Count < _forTutorCountUnitsAfterStart)
                    for (int i = 0; i < _forTutorCountUnitsAfterStart; i++){
                        AddToQueueInstant();
                    }

                // while (_factory.HasFreeSpaceForResources(_factory.Config.OutputResources))
                // {
                //     AddToQueueInstant();
                // }

                _isSecondSpawn = true;
            } else{
                StartCoroutine(StartProductionCoroutine());
            }
        }

        private IEnumerator StartProductionCoroutine(){
            _bus.transform.position = _startPoint.position;
            StartCoroutine(BarrierOpenCoroutine());
            yield return _bus.MoveToCoroutine(_stopPoint.position);
            yield return AddUnitsToZone();
            yield return new WaitForSeconds(_delayForBus);
            yield return _bus.MoveToCoroutine(_endPoint.position);
            _barrierAnimator.SetBool(_isOpenHash, false);
        }

        private IEnumerator BarrierOpenCoroutine(){
            yield return new WaitForSeconds(_delayForBarrier);
            _barrierAnimator.SetBool(_isOpenHash, true);
        }

        private void AddToQueueInstant(){
            var unit = Instantiate(_unitPrefab,
                _unitSpawnPoint.position,
                _unitSpawnPoint.rotation,
                UnitsManager.Instance.transform);

            _unitsQueue.AddUnitToQueueInstant(unit);
            _factory.ProduceItem();
        }

        private IEnumerator AddUnitsToZone(){
            int spawnedUnits = 0;
            while (CheckSpawnCondition(spawnedUnits)){
                var unit = Instantiate(_unitPrefab,
                    _unitSpawnPoint.position,
                    _unitSpawnPoint.rotation,
                    UnitsManager.Instance.transform);

                _unitsQueue.AddUnitToQueue(unit);
                _factory.ProduceItem();
                Instantiate(_puffEffect, unit.transform.position, Quaternion.identity);
                spawnedUnits++;
                yield return new WaitForSeconds(_spawnDelay);
            }
        }

        private bool CheckSpawnCondition(int alreadySpawned){
            // if (_isSecondSpawn == false && 
            //     TutorialController.InstanceExists && 
            //     TutorialController.Instance.TutorDone == false)
            // {
            //     return _factory.HasFreeSpaceForResources(_factory.Config.OutputResources);
            // }

            return alreadySpawned < _factory.Stats.GetValueOf(_factory.Config.OutputCount) &&
                   _factory.HasFreeSpaceForResources(_factory.Config.OutputResources);
        }
    }
}