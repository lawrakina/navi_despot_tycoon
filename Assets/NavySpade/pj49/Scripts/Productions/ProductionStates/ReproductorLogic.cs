using System;
using System.Collections;
using NavySpade.pj49.Scripts.Items;
using NavySpade.pj49.Scripts.UnitsQueues;
using UnityEngine;

namespace NavySpade.pj49.Scripts.Productions.ProductionStates
{
    public class ReproductorLogic : FactoryStateMachine
    {
        [SerializeField] private UnitQueue _inputQueue;
        [SerializeField] private UnitQueue _outputQueue;
        [SerializeField] private Animator _cageAnimator;
        [SerializeField] private GameObject _puffEffect;
        [SerializeField] private Transform _spawnPoint;

        private readonly int _workHash = Animator.StringToHash("Work");
        
        private int _unitsReachedDestination;
        private float _currentProductionTime;
        
        private void Start()
        {
            _factory.ProductionStarted += StartProductionFlow;
        }

        private void OnDestroy()
        {
            _factory.ProductionStarted -= StartProductionFlow;
        }

        private void StartProductionFlow()
        {
            StopAllCoroutines();
            _factory.InProductionProcess = true;
            MoveUnitsToCage();
        }

        private void MoveUnitsToCage()
        {
            _unitsReachedDestination = 0;
            for (int i = 0; i < _factory.GetResourceCountToStart(); i++)
            {
                CollectingUnit unit = _inputQueue.GetFirstInQueue();
                unit.StartMoveToPath(EnterPoint, () =>
                {
                    Instantiate(_puffEffect, unit.transform.position, Quaternion.identity);
                    Destroy(unit.gameObject);
                    _unitsReachedDestination++;
                    CheckMergeCondition();
                });
                _inputQueue.RemoveFromQueue(unit);
            }
        }

        private void CheckMergeCondition()
        {
            if (_unitsReachedDestination >= _factory.GetResourceCountToStart())
            {
                StartCoroutine(Producing());
            }
        }

        private IEnumerator Producing()
        {
            _cageAnimator.SetBool(_workHash, true);

            _factory.ElapsedTimeOfProduction = 0;
            for (int i = 0; i < _factory.Stats.OutputCount; i++)
            {
                _currentProductionTime = 0;
                while (_currentProductionTime < _factory.Stats.ProductionTime / _factory.Stats.OutputCount)
                {
                    _currentProductionTime += Time.deltaTime * _factory.ProductionBoost;
                    _factory.ElapsedTimeOfProduction += Time.deltaTime * _factory.ProductionBoost;
                    yield return null;
                }
                
                TryAddUnit();
            }
            
            _cageAnimator.SetBool(_workHash, false);
            _factory.InProductionProcess = false;
        }

         private void TryAddUnit()
        {
            if (_factory.HasFreeSpaceForResources(_factory.Config.OutputResources)){
                AddUnitToZone();
            }
        }
        
        private void AddUnitToZone()
        {
            var unit = Instantiate(_factory.Config.OutputResources.Prefab, 
                _spawnPoint.position, 
                _spawnPoint.rotation, 
                UnitsManager.Instance.transform).GetComponent<CollectingUnit>();
            
            unit.BlockCollect = true;
            
            _outputQueue.AddUnitToQueue(unit);
            _factory.ProduceItem();
            
            Instantiate(_puffEffect, unit.transform.position, Quaternion.identity);
        }
    }
}