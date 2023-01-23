using System;
using NavySpade.pj49.Scripts.Items;
using NavySpade.pj49.Scripts.Items.Inventory;
using NavySpade.pj49.Scripts.Navigation;
using NavySpade.pj49.Scripts.UnitsQueues;
using UnityEngine;

namespace NavySpade.pj49.Scripts.Productions.ProductionStates
{
    public class TradeMarketLogic : MonoBehaviour
    {
        [SerializeField] private ItemsTrader _itemsTrader;
        [SerializeField] private ResourceGroupAsset _unitGroup;
        [SerializeField] private AIPoint _enterPoint;
        [SerializeField] private GameObject _pufEffect;
        
        private void Start()
        {
            _itemsTrader.ResourceTraded += OnResourceTraded;
        }

        private void OnDestroy()
        {
            _itemsTrader.ResourceTraded -= OnResourceTraded;
        }

        private void OnResourceTraded(ResourceAsset resource)
        {
            if (resource.Group == _unitGroup)
            {
                UnitSquad unitSquad = UnitSquad.Instance;
                if(unitSquad.Units.Count == 0)
                    return;
                
                var collectingUnit = unitSquad.Units[unitSquad.Units.Count - 1];
                unitSquad.Remove(collectingUnit);

                collectingUnit.MovementToPath.MovementSpeed = 5;
                collectingUnit.StartMoveToPath(_enterPoint, () =>
                {
                    Instantiate(_pufEffect, collectingUnit.transform.position, Quaternion.identity);
                    Destroy(collectingUnit.gameObject);
                });
                collectingUnit.BlockCollect = true;
            }
        }
    }
}