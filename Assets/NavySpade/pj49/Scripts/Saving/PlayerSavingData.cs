using System;
using NavySpade.pj49.Scripts.Items;
using UnityEngine;

namespace NavySpade.pj49.Scripts.Saving
{
    [Serializable]
    public class PlayerSavingData
    {
        public SaveableInventory Items;
        public int HumanGroupCapacityLevel;
    }
}