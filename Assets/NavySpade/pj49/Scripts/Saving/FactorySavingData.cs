using System;
using System.Collections.Generic;
using NavySpade.pj49.Scripts.Items;
using NavySpade.pj49.Scripts.Items.Inventory;

namespace NavySpade.pj49.Scripts.Saving
{
    [Serializable]
    public class FactorySavingData
    {
        public SaveableInventory InputItems;
        public SaveableInventory OutputItems;
        public List<int> UpgradesLevel;
        public List<int> BuildProgress;
    }
}