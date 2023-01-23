using System;
using System.Collections.Generic;
using System.Linq;
using NavySpade.pj49.Scripts.Items.Inventory;
using NavySpade.pj49.Scripts.Productions;
using NavySpade.pj49.Scripts.Productions.Factory;
using NavySpade.pj49.Scripts.UnitsQueues;
using NavySpade.pj49.Scripts.UnitsQueues.Positions;
using UnityEngine;

namespace NavySpade.pj49.Scripts.Items
{
    public class UnitsItemsTaker : ItemsReceiver
    {
        [SerializeField] private Factory _factory;
        [SerializeField] private UnitQueue _unitsQueue;
        [SerializeField] private PointsHolder _pointsHolder;
        [SerializeField] private ResourceGroupAsset _unitGroup;
        [SerializeField] private ResourceAsset _unitAsset;
        
        private ResourcesInventory _playerInventory;
        private FactoryConfig.FactoryRequirement _factoryRequirement;
        private int _itemsInProgress;
        
        private void Awake(){
            _factory.FactoryInitialise += FactoryOnFactoryInitialise;
        }

        private void FactoryOnFactoryInitialise(){
            _unitsQueue.UnitReached += AddToInventory;
            _factoryRequirement = _factory.Config.GetGroupRequirement(_unitGroup);
            _factory.Stats.Upgraded += OnStatsUpgraded;
            UpdateZone(-1);
            SpawnUnits();
        }

        private void OnStatsUpgraded(FactoryUpgradeParameter parameter, int level)
        {
            if (_factoryRequirement.MaxInputCapacity == parameter)
            {
                UpdateZone(level);
            }
        }
        
        private void OnDestroy()
        {
            _factory.FactoryInitialise -= FactoryOnFactoryInitialise;
            
            _unitsQueue.UnitReached -= AddToInventory;
            _factory.Stats.Upgraded -= OnStatsUpgraded;
        }
        
        private void SpawnUnits()
        {
            List<Item> items = new List<Item>();
            if (_unitAsset)
            {
                items.Add(_factory.InputInventory.GetItem(_unitAsset));
            }
            else
            {
                items = _factory.InputInventory.GetItemsByGroup(_unitGroup).ToList();
            }

            foreach (var item in items)
            {
                for (int i = 0; i < item.Amount; i++)
                { 
                    var unit = Instantiate(item.Resource.Prefab, UnitsManager.Instance.transform);
                    _unitsQueue.AddUnitToQueueInstant(unit.GetComponent<CollectingUnit>());
                }
            }
        }
        
        public override void PickupItemsToThis(ResourcesInventory playerInventory)
        {
            if (IsCanGet(playerInventory))
            {
                _playerInventory = playerInventory;
                
                UnitSquad unitSquad = UnitSquad.Instance;
                if(unitSquad.Units.Count == 0)
                    return;

                CollectingUnit collectingUnit = null;
                if (_unitAsset)
                {
                    if(unitSquad.TryGetResourceUnit(_unitAsset, out collectingUnit) == false)
                        return;
                }
                else
                {
                    collectingUnit = unitSquad.Units[unitSquad.Units.Count - 1];
                    unitSquad.Remove(collectingUnit);
                }
                
                _unitsQueue.AddUnitToQueue(collectingUnit);
                var item = collectingUnit.UnitResource.CreateItem();
                _playerInventory.TryRemoveResource(item);
                _itemsInProgress++;
            }
        }

        private bool IsCanGet(ResourcesInventory playerInventory)
        {
            int inInventory = 0;
            if (_unitAsset)
            {
                inInventory = playerInventory.Count(_unitAsset);
                return inInventory > 0 && CanAddItems(_factory.InputInventory.Count(_unitAsset));
            }

            inInventory = playerInventory.Count(_unitGroup);
            return inInventory > 0 && CanAddItems(_factory.InputInventory.Count(_unitGroup));
        }

        private bool CanAddItems(int inInventory)
        {
            int totalItems = inInventory;
            totalItems += _itemsInProgress;

            if (_factoryRequirement == null)
                FactoryOnFactoryInitialise();
            var parameter = _factoryRequirement.MaxInputCapacity;
            return totalItems < parameter.GetIntValue(_factory.Stats.GetUpgradeLevel(parameter));
        }
        
        private void AddToInventory(CollectingUnit unit)
        {
            _itemsInProgress--;
            var item = unit.UnitResource.CreateItem();
            _factory.PickupItem(item);
        }
        
        private void UpdateZone(int levelIndex)
        {
            var param = _factoryRequirement.MaxInputCapacity;
            _pointsHolder.ChangeSize(_factory.Stats.GetIntValueOf(param));
        }
    }
}
