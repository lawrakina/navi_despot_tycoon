using System.Collections.Generic;
using NavySpade.pj49.Scripts.Items.Inventory;

namespace NavySpade.pj49.Scripts.Items
{
    public static class ResourceUtility
    {
        public static SaveableInventory ConvertToSavableList(List<Item> item)
        {
            SaveableInventory saveableInventory = new SaveableInventory();
            saveableInventory.Items = item;
            return saveableInventory;
        }
    }
}