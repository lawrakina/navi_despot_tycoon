using NavySpade.pj49.Scripts.Items.Inventory;
using NavySpade.pj49.Scripts.Productions;
using NavySpade.pj49.Scripts.Productions.Factory;
using UnityEngine;


namespace NavySpade.pj49.Scripts.Items{
    public class ResourceTaker : ItemsReceiver{
        [SerializeField]
        private ResourceAsset _resourceAsset;
        [SerializeField]
        private Factory _factory;
        [SerializeField]
        private bool _isSecondRequiredResourceToProduction = false;

        public override void PickupItemsToThis(ResourcesInventory playerInventory){
            if (_isSecondRequiredResourceToProduction){
                if (_factory.CanPickupSecondItem(_resourceAsset)){
                    Item secondItem = _resourceAsset.CreateItem();
                    if (playerInventory.TryRemoveResource(secondItem)){
                        _factory.PickupItem(secondItem);
                    }
                }
            }
            if (_factory.CanPickupItem(_resourceAsset) == false)
                return;

            Item item = _resourceAsset.CreateItem();
            if (playerInventory.TryRemoveResource(item)){
                _factory.PickupItem(item);
            }
        }
    }
}