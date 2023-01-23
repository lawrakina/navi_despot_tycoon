using NavySpade.pj49.Scripts.Items;
using NavySpade.pj49.Scripts.Items.Inventory;
using NavySpade.pj49.Scripts.Saving;
using UnityEngine;

namespace NavySpade.pj49.Scripts.Productions
{
    public class Market : MonoBehaviour, ISaveable
    {
        [SerializeField] private ResourcesInventory _inventory;
        
        public void Init()
        {
            LevelSaver.Instance.Register(this);
            SaveableInventory saveInventory = LevelSaver.LoadMarketSaving();
            _inventory.Init(saveInventory);
        }

        public void Save()
        {
            LevelSaver.SaveMarket(_inventory.GetSaveableInventory());
        }
    }
}