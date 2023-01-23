using System;
using System.Collections.Generic;
using NavySpade.pj49.Scripts.Items.Inventory;
using NavySpade.pj49.Scripts.UnitsQueues;
using NavySpade.pj49.Scripts.UnitsQueues.Positions;
using UnityEngine;


namespace NavySpade.pj49.Scripts.Items{
    public class UnitQueue : MonoBehaviour{
        [SerializeField]
        private PointsHolder _points;

        private List<CollectingUnit> _unitsInQueue = new List<CollectingUnit>();
        private List<CollectingUnit> _unitsInMove = new List<CollectingUnit>();
        [SerializeField]
        private bool _isMoveTypeIsTeleport;

        public int TotalUnits => _unitsInMove.Count + _unitsInQueue.Count;

        public int UnitsInQueue => _unitsInQueue.Count;
        public List<CollectingUnit> ListUnits => _unitsInQueue;

        public event Action<CollectingUnit> UnitReached;

        public event Action<List<CollectingUnit>> ChangeListUnits = delegate(List<CollectingUnit> list){ };

        public void AddUnitToQueue(CollectingUnit collectingUnit){
            if (_unitsInQueue.Contains(collectingUnit))
                return;

            if (_isMoveTypeIsTeleport){
                AddUnitToQueueInstant(collectingUnit);
                UnitReached?.Invoke(collectingUnit);
                return;
            }
            MoveUnitToQueuePoint(collectingUnit, TotalUnits, true);
            _unitsInMove.Add(collectingUnit);
        }

        public CollectingUnit GetFirstInQueue(){
            CollectingUnit unit;
            if (_unitsInQueue.Count == 0){
                unit = _unitsInMove[0];
            } else{
                unit = _unitsInQueue[0];
            }

            return unit;
        }

        public CollectingUnit GetLastInQueue(){
            CollectingUnit unit;
            if (_unitsInQueue.Count == 0){
                unit = _unitsInMove[_unitsInMove.Count - 1];
            } else{
                unit = _unitsInQueue[_unitsInQueue.Count - 1];
            }

            return unit;
        }

        public void RemoveFromQueue(CollectingUnit unit, bool isReculcPositions = true){
            _unitsInQueue.Remove(unit);
            _unitsInMove.Remove(unit);
            if (isReculcPositions)
                UpdateUnitsPosition();
            // ChangeListUnits?.Invoke(_unitsInQueue);
        }

        private void UpdateUnitsPosition(){
            for (int i = 0; i < _unitsInQueue.Count; i++){
                MoveUnitToQueuePoint(_unitsInQueue[i], i, false);
            }

            for (int i = 0; i < _unitsInMove.Count; i++){
                MoveUnitToQueuePoint(_unitsInMove[i], _unitsInQueue.Count + i, false);
            }
        }

        private void MoveUnitToQueuePoint(CollectingUnit collectingUnit, int indexInQueue, bool callAction){
            collectingUnit.StartMoveToPoint(_points.GetPosition(indexInQueue), () => {
                SetUnitRotationInQueue(collectingUnit, indexInQueue);

                if (_unitsInQueue.Contains(collectingUnit) == false){
                    _unitsInQueue.Add(collectingUnit);
                    ChangeListUnits?.Invoke(_unitsInQueue);
                    _unitsInMove.Remove(collectingUnit);
                    UnitReached?.Invoke(collectingUnit);
                }
            });

            collectingUnit.GetComponent<UnitChain>().enabled = false;
            collectingUnit.GetComponent<Rigidbody>().isKinematic = true;
            collectingUnit.BlockCollect = true;
        }

        private void SetUnitRotationInQueue(CollectingUnit unit, int indexInQueue){
            if (indexInQueue == 0){
                Vector3 pointPos = _points.GetPosition(indexInQueue);
                var dir = (pointPos + Vector3.forward) - pointPos;
                unit.transform.rotation = Quaternion.LookRotation(dir);
            } else{
                var dir = _points.GetPosition(indexInQueue - 1) - _points.GetPosition(indexInQueue);
                unit.transform.rotation = Quaternion.LookRotation(dir);
            }
        }

        public void AddUnitToQueueInstant(CollectingUnit unit){
            unit.transform.position = _points.GetPosition(TotalUnits);
            SetUnitRotationInQueue(unit, TotalUnits);

            unit.MovementToPath.enabled = false;
            unit.MovementToPlayer.enabled = false;
            unit.GetComponent<UnitChain>().enabled = false;
            unit.GetComponent<Rigidbody>().isKinematic = true;
            unit.BlockCollect = true;

            _unitsInQueue.Add(unit);
            ChangeListUnits?.Invoke(_unitsInQueue);
        }
    }
}