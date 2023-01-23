using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NavySpade.pj49.Scripts.Items;
using NavySpade.pj49.Scripts.Items.Inventory;
using NavySpade.pj49.Scripts.UnitsQueues.Positions;
using Unity.Mathematics;
using UnityEngine;


namespace NavySpade.pj49.Scripts.Productions.ProductionStates{
    public class MineController : FactoryStateMachine{
        [SerializeField]
        private UnitQueue _unitsQueue;
        [SerializeField]
        private CirclePoints _points;
        [SerializeField]
        private float _radiusOfMine = 3.0f;
        [SerializeField]
        private ParticleSystem _puff;

        private List<UnitMineBehaviour> _unitsInProduction = new List<UnitMineBehaviour>();
        private List<UnitMineBehaviour> _unitsInProgress = new List<UnitMineBehaviour>();
        private List<MineWorkplace> _listWorkplaces = new List<MineWorkplace>();
        
        public float ProductionTime => _factory.Stats.ProductionTime;
        public int TotalUnitInProgress => _unitsInProduction.Count + _unitsInProgress.Count;

        private void Start(){
            _factory = GetComponent<Factory.Factory>();
            CheckEnabledWorkplace(_factory.Stats.MaxInputCapacity);
            _factory.ProductionStarted += StartProductionFlow;
            _factory.FactoryInitialise += Init;
        }

        private void Init(){
        }

        private void OnDestroy(){
            _factory.ProductionStarted -= StartProductionFlow;
            _factory.FactoryInitialise -= Init;
        }

        private void CheckEnabledWorkplace(int count){
            foreach (var position in _points.GetPositions(count, _radiusOfMine)){
                _listWorkplaces.Add(new MineWorkplace{Workplace = position, Unit = null});
            }
        }

        private void StartProductionFlow(){
            for (int i = 0; i < _factory.GetResourceCountToStart(); i++){
                _factory.InProductionProcess = true;
                CollectingUnit unit = _unitsQueue.GetFirstInQueue();
                UnitMineBehaviour unitMineBehaviour = unit.GetComponent<UnitMineBehaviour>();
                unitMineBehaviour.Init(this);
                unitMineBehaviour.StateChanged += UpdateSmt;
                unitMineBehaviour.SetState(FactoryStates.MoveToProductionPoint);
                _unitsQueue.RemoveFromQueue(unit);
                _unitsInProgress.Add(unitMineBehaviour);

                _factory.InProductionProcess = TotalUnitInProgress >= _factory.Stats.MaxInputCapacity;
            }
        }

        private void UpdateSmt(UnitMineBehaviour resource){
            switch (resource.State){
                case FactoryStates.MoveToProductionPoint:
                    break;
                case FactoryStates.Producing:
                    _unitsInProgress.Remove(resource);
                    _unitsInProduction.Add(resource);
                    resource.transform.LookAt(_points.transform);
                    break;
                case FactoryStates.Destroying:
                    KillUInit(resource);
                    break;
                case FactoryStates.Finish:
                    _factory.InProductionProcess = TotalUnitInProgress >= _factory.Stats.MaxInputCapacity;
                    break;
            }
        }

        private void KillUInit(UnitMineBehaviour unit){
            _unitsInProduction.Remove(unit);
            foreach (var workplace in _listWorkplaces){
                if (workplace.Unit == unit)
                    workplace.Unit = null;
                break;
            }

            Destroy(unit.gameObject);
            StartCoroutine(CreatePuffEffect(_puff, unit.transform.position));
            unit.SetState(FactoryStates.Finish);
        }

        private IEnumerator CreatePuffEffect(ParticleSystem puff, Vector3 position){
            var effect = Instantiate(puff, position, quaternion.identity).GetComponent<ParticleSystem>();
            effect.Play();
            yield return new WaitForSeconds(2.0f); //Magic number 8-) Yes, is`t true, because I don`t give a fuck
            Destroy(effect.gameObject);
        }

        public Vector3 GetPositions(CollectingUnit unit){
            var result = _listWorkplaces.FirstOrDefault(x => x.Unit == null);
            result.Unit = unit.GetComponent<UnitMineBehaviour>();
            return result.Workplace;
        }

        public bool TryGetMeat(){
            var resource = _factory.Stats.ResourceForRestartProduction;
            if (_factory.CanPickupSecondItem(resource)){
                _factory.InputInventory.TryRemoveResource(resource.CreateItem(1));
                return true;
            }

            return false;
        }

        public void WorkCompleted(){
            _factory.ProduceItem();
        }
    }

    internal class MineWorkplace{
        public Vector3 Workplace;
        public UnitMineBehaviour Unit;
    }
}