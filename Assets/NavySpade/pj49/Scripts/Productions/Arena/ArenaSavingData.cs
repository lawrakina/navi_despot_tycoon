using System;
using System.Collections.Generic;
using NavySpade.pj49.Scripts.Items;


namespace NavySpade.pj49.Scripts.Productions.Arena{
    [Serializable]
    public class ArenaSavingData
    {
        public SaveableInventory Items;
        public List<int> UpgradesLevel;
        public int HealthPoints;
    }
}