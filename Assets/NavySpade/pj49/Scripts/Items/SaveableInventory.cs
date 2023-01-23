using System;
using System.Collections.Generic;
using NavySpade.pj49.Scripts.Items.Inventory;
using UnityEditor;

namespace NavySpade.pj49.Scripts.Items
{
    [Serializable]
    public class SaveableInventory
    {
        public List<Item> Items;
    }
}