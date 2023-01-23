using System;
using NavySpade.Core.Runtime.Player.Logic;
using NavySpade.pj49.Scripts.Items.Inventory;
using NavySpade.pj49.Scripts.UnitsQueues;
using UnityEngine;

namespace NavySpade.pj49.Scripts.Productions.ProductionStates
{
    public class BuilderUnitBehaviour : MonoBehaviour
    {
        [SerializeField] private ResourceGroupAsset _unitGroup;
        [SerializeField] private BuildingBuilder _buildingBuilder;
        [SerializeField] private GameObject _puffEffect;
        
        private void OnEnable()
        {
            _buildingBuilder.ResourcePickingAction += PickUp;
        }
        
        private void OnDisable()
        {
            _buildingBuilder.ResourcePickingAction -= PickUp;
        }
        
        private void PickUp(ResourcesInventory from, ResourceAsset res)
        {
            if(res.Group != null && res.Group != _unitGroup)
                return;
            
            if(from.TryRemoveResource(res.CreateItem()) == false)
                return;
            
            int count = 1 * res.Multiplier;
            _buildingBuilder.AddResToProgress(res, count);
            GetUnitFromPlayerQueue(count);
        }


        private void GetUnitFromPlayerQueue(int value)
        {
            var playerInventory = SinglePlayer.Instance.Inventory;
                
            UnitSquad unitSquad = UnitSquad.Instance;
            if(unitSquad.Units.Count == 0)
                return;

            if (playerInventory.Count(_unitGroup) < unitSquad.Units.Count)
            {
                var collectingUnit = unitSquad.Units[unitSquad.Units.Count - 1];
                unitSquad.Remove(collectingUnit);
                MoveUnitToPoint(collectingUnit, _buildingBuilder.TargetPoint.position, value);
            }
        }

        private void MoveUnitToPoint(CollectingUnit collectingUnit, Vector3 point, int value)
        {
            collectingUnit.StartMoveToPoint(point, () => {

                Instantiate(_puffEffect, collectingUnit.transform.position, Quaternion.identity);
                Destroy(collectingUnit.gameObject);
                _buildingBuilder.ApplyProgress(collectingUnit.UnitResource, value);
            });
            
            collectingUnit.GetComponent<UnitChain>().enabled = false;
            collectingUnit.GetComponent<Rigidbody>().isKinematic = true;
            collectingUnit.BlockCollect = true;
        }
    }
}