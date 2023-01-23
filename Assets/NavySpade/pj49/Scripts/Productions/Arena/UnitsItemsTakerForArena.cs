using System;
using System.Collections.Generic;
using System.Linq;
using NavySpade.pj49.Scripts.Items;
using NavySpade.pj49.Scripts.Items.Inventory;
using NavySpade.pj49.Scripts.Productions.Factory;
using NavySpade.pj49.Scripts.UnitsQueues;
using NavySpade.pj49.Scripts.UnitsQueues.Positions;
using UnityEngine;


namespace NavySpade.pj49.Scripts.Productions.Arena{
    public class UnitsItemsTakerForArena : ItemsReceiver{
        [SerializeField]
        private ArenaLogic _arena;
        [SerializeField]
        private UnitQueue _unitsQueue;
        [SerializeField]
        private PointsHolder _pointsHolder;
        [SerializeField]
        private ResourceGroupAsset _unitGroup;
        [SerializeField]
        private ResourceAsset _unitAsset;

        private ArenaConfig _arenaConfig;

        private ResourcesInventory _playerInventory;
        private ArenaConfig.FactoryRequirement _arenaRequirement;
        private int _itemsInProgress;
        private event Action<int> ChangeItems;
        private int ItemsInProgress{
            get => _itemsInProgress;
            set{
                _itemsInProgress = value;
                ChangeItems?.Invoke(value);
            }
        }

        private void Awake(){
            _arena.ArenaInitialise += OnArenaInitialise;
        }

        private void OnArenaInitialise(){
            _unitsQueue.UnitReached += AddToInventory;
            _arenaRequirement = _arena.Settings.GetGroupRequirement(_unitGroup);

            ChangeItems += OnAttackAndChangeItems;
            _arenaConfig = _arena.Settings;
            _arena.Model.Upgraded += OnStatsUpgraded;
            _arena.Model.Units = _unitsQueue;
            UpdateZone(-1);
            SpawnUnits();
        }

        private void OnAttackAndChangeItems(int obj){
            // _arena.Model.CountAttackedUnits.Value = _unitsQueue.TotalUnits;
        }

        private void OnStatsUpgraded(FactoryUpgradeParameter parameter, int level){
            if (_arenaRequirement.MaxInputCapacity == parameter){
                UpdateZone(level);
            }
        }

        private void OnDestroy(){
            _arena.ArenaInitialise -= OnArenaInitialise;

            _unitsQueue.UnitReached -= AddToInventory;
            _arena.Model.Upgraded -= OnStatsUpgraded;
        }

        private void SpawnUnits(){
            List<Item> items = new List<Item>();
            if (_unitAsset){
                items.Add(_arena.Inventory.GetItem(_unitAsset));
            } else{
                items = _arena.Inventory.GetItemsByGroup(_unitGroup).ToList();
            }

            foreach (var item in items){
                for (int i = 0; i < item.Amount; i++){
                    var unit = Instantiate(item.Resource.Prefab, UnitsManager.Instance.transform);
                    _unitsQueue.AddUnitToQueueInstant(unit.GetComponent<CollectingUnit>());
                }
            }
        }

        public override void PickupItemsToThis(ResourcesInventory playerInventory){
            if (IsCanGet(playerInventory)){
                _playerInventory = playerInventory;

                UnitSquad unitSquad = UnitSquad.Instance;
                if (unitSquad.Units.Count == 0)
                    return;

                CollectingUnit collectingUnit = null;
                if (_unitAsset){
                    if (unitSquad.TryGetResourceUnit(_unitAsset, out collectingUnit) == false)
                        return;
                } else{
                    collectingUnit = unitSquad.Units[unitSquad.Units.Count - 1];
                    unitSquad.Remove(collectingUnit);
                }

                _unitsQueue.AddUnitToQueue(collectingUnit);
                var item = collectingUnit.UnitResource.CreateItem();
                _playerInventory.TryRemoveResource(item);
                ItemsInProgress++;
            }
        }

        private bool IsCanGet(ResourcesInventory playerInventory){
            int inInventory = 0;
            if (_unitAsset){
                inInventory = playerInventory.Count(_unitAsset);
                return inInventory > 0 &&
                       CanAddItems(_arena.Inventory.Count(_unitAsset)) &&
                       _arena.IsWeakUnit;
            }

            inInventory = playerInventory.Count(_unitGroup);
            return inInventory > 0 &&
                   CanAddItems(_arena.Inventory.Count(_unitGroup)) &&
                   _arena.IsWeakUnit;
        }

        private bool CanAddItems(int inInventory){
            int totalItems = inInventory;
            totalItems += ItemsInProgress;

            var parameter = _arenaRequirement.MaxInputCapacity;
            return totalItems < parameter.GetIntValue(_arena.Model.GetUpgradeLevel(parameter));
        }

        private void AddToInventory(CollectingUnit unit){
            _arena.AddedUnit(unit);
            ItemsInProgress--;
        }

        private void UpdateZone(int levelIndex){
            var param = _arenaRequirement.MaxInputCapacity;
            _pointsHolder.ChangeSize(_arena.Model.GetIntValueOf(param));
        }
    }
}