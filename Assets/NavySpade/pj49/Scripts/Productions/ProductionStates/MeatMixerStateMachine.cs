using System;
using System.Collections;
using System.Collections.Generic;
using NavySpade.pj49.Scripts.Items;
using NavySpade.pj49.Scripts.Items.Inventory;
using NavySpade.pj49.Scripts.Navigation;
using UnityEngine;

namespace NavySpade.pj49.Scripts.Productions.ProductionStates
{
    public class MeatMixerStateMachine : FactoryStateMachine
    {
        [Header("Visual")]
        [SerializeField] private UnitQueue _unitsQueue;
        [SerializeField] private ParticleSystem _bloodParticle;
        [SerializeField] private Transform _conveyorStart;
        [SerializeField] private Transform _conveyorEnd;
        [SerializeField] private ParticleSystem _moneyParticle;
        //[SerializeField] private float _delayBeforeSpawn;
        [SerializeField] private float _delayConserveEnableGraphics;
        [SerializeField] private float _delayConserveDisableGraphics;
        
        private List<UnitMeatMixerBehaviour> _resourcesInProduction = new List<UnitMeatMixerBehaviour>();
        private Conserve _activeConserve;

        private void Start()
        {
            _factory.ProductionStarted += StartProductionFlow;
        }

        private void OnDestroy()
        {
            _factory.ProductionStarted -= StartProductionFlow;
        }

        public void StartProductionFlow()
        {
            _factory.InProductionProcess = true;
            CollectingUnit unit = _unitsQueue.GetFirstInQueue();
            UnitMeatMixerBehaviour stateMachine = unit.GetComponent<UnitMeatMixerBehaviour>();
            stateMachine.Init(this);
            stateMachine.StateChanged += UpdateSmt;
            stateMachine.SetState(FactoryStates.MoveToProductionPoint);
            _resourcesInProduction.Add(stateMachine);
            _unitsQueue.RemoveFromQueue(unit);
        }

        private void UpdateSmt(UnitMeatMixerBehaviour resource)
        {
            switch (resource.State)
            {
                case FactoryStates.MoveToProductionPoint:
                    break;
                case FactoryStates.Producing:
                    break;
                case FactoryStates.Destroying:
                    _resourcesInProduction.Remove(resource);
                    StartCoroutine(PlayDestroyingFlow(resource));
                    break;
                case FactoryStates.Finish:
                    break;
            }
        }

        private IEnumerator PlayDestroyingFlow(UnitMeatMixerBehaviour resource)
        {
            _bloodParticle.Play();
            yield return Producing();
            _factory.InProductionProcess = false;
            
            var conserve = Instantiate(_factory.Stats.OutputPrefab, transform);
            conserve.transform.position = _conveyorStart.position;
            conserve.GetComponent<MeshRenderer>().enabled = false;
            yield return new WaitForSeconds(_delayConserveEnableGraphics);
            
            conserve.GetComponent<MeshRenderer>().enabled = true;
            yield return conserve.MoveHandler.MoveCoroutine(_conveyorStart.position, _conveyorEnd.position);
            
            _moneyParticle.Play();
            yield return new WaitForSeconds(_delayConserveDisableGraphics);
            
            conserve.GetComponent<MeshRenderer>().enabled = false;
            _factory.ConvertItemsFrom(resource.ResourceAsset);
        }

        private IEnumerator Producing()
        {
            float currentProductionTime = 0;
            _factory.ElapsedTimeOfProduction = 0;
            while (currentProductionTime < _factory.Stats.ProductionTime)
            {
                currentProductionTime += Time.deltaTime * _factory.ProductionBoost;
                _factory.ElapsedTimeOfProduction += Time.deltaTime * _factory.ProductionBoost;
                yield return null;
            }
        }
    }
}