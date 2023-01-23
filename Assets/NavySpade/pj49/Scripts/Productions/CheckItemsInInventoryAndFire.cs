using NavySpade.pj49.Scripts.Items;
using NavySpade.pj49.Scripts.Items.Inventory;
using UnityEngine;
using UnityEngine.Events;

namespace NavySpade.pj49.Scripts.Productions
{
    public class CheckItemsInInventoryAndFire : MonoBehaviour
    {
        public UnityEvent Fire, Cancel;

        public ResourceAsset ResourceAsset;
        public int Count;

        public ResourcesInventory Inventory;
        
        private bool _isWork;

        public bool IsWork
        {
            get => _isWork;
            private set
            {
                if(_isWork == value)
                    return;
                
                if (value)
                {
                    Fire.Invoke();
                }
                else
                {
                    Cancel.Invoke();
                }
                
                _isWork = value;
            }
        }

        private void Awake()
        {
            Cancel.Invoke();
        }

        private void OnEnable()
        {
            Inventory.ResourcesCountChanged += InventoryOnResourcesCountChanged;
        }
        
        private void OnDisable()
        {
            if(Inventory)
                Inventory.ResourcesCountChanged -= InventoryOnResourcesCountChanged;
        }

        private void InventoryOnResourcesCountChanged()
        {
            var requestingItem = ResourceAsset.CreateItem(Count);

            IsWork = Inventory.Contains(requestingItem);
        }
    }
}