using System;
using System.Collections.Generic;
using System.Linq;
using NavySpade.Modules.Extensions.UnityTypes;
using NavySpade.pj49.Scripts.Items;
using NavySpade.pj49.Scripts.Items.Inventory;

namespace NavySpade.pj49.Scripts.UnitsQueues
{
    public class UnitSquad : ExtendedMonoBehavior<UnitSquad>
    {
        public ResourceGroupAsset Group;
        public List<CollectingUnit> Units { get; private set; }
        public ResourcesInventory Inventory;

        public event Action UnitSetsInSquad;

        public void InvokeSetsInSquad()
        {
            UnitSetsInSquad?.Invoke();
        }

        protected override void Awake()
        {
            base.Awake();
            Units = new List<CollectingUnit>();
        }

        public bool TryAddUnit(CollectingUnit unit, bool addInInventory = true)
        {
            if (Units.Contains(unit))
                return false;
            
            Units.Add(unit);

            if (addInInventory && Inventory.TryAddResource(unit.UnitResource.CreateItem()) == false)
            {
                Units.Remove(unit);
                return false;
            }
            
            return true;
        }
        
        public bool TryGetResourceUnit(ResourceAsset asset, out CollectingUnit resUnit)
        {
            foreach (var unit in Units)
            {
                if (unit.UnitResource == asset)
                {
                    resUnit = unit;
                    Units.Remove(unit);
                    UpdateUnitsPosition();
                    return true;
                }
            }

            resUnit = null;
            return false;
        }

        private void UpdateUnitsPosition()
        {
            for (int i = 0; i < Units.Count; i++)
            {
                Units[i].MovementToPlayer.UpdateQueuePosition(i + 1);
                Units[i].GetComponent<UnitChain>().SetChain();
            }
        }

        public void InventoryOnResourcesCountChanged()
        {
            var count = Inventory.Items
                .Where(i => i.Resource.Group != null && i.Resource.Group == Group)
                .Sum(i => i.Amount);

            for (var i = 0; i < Units.Count - count; i++)
            {
                var unit = Units[Units.Count - 1];
                
                Units.RemoveAt(Units.Count - 1);
            }
        }

        public void Remove(CollectingUnit unit)
        {
            Units.Remove(unit);
        }
    }
}