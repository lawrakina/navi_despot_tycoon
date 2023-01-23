using System;
using System.Collections;
using System.Linq;
using Guirao.UltimateTextDamage;
using NavySpade.pj49.Scripts.Items.Inventory;
using NavySpade.pj49.Scripts.Tutorial;
using UnityEngine;


namespace NavySpade.pj49.Scripts.Items{
    public class PickupableResource : MonoBehaviour{
        public ResourceAsset ResourceAsset;
        public ResourcesInventory Source;
        public float _delayBetweenPick;
        [SerializeField]
        private float _verticalOffsetToSHowPopup = 5f;

        [SerializeField]
        private bool _isMomentumCharge = false;
        // [SerializeField]
        // private float _delayBeforePickup = 0;


        #region PrivateData

        private bool _pickCooldown;
        private float _deltaTimeBeforePickup;
        private Collider _other;
        private bool _startDeferredCharge;
        private UltimateTextDamageManager _popupManager;

        #endregion


        private void Awake(){
            _popupManager = FindObjectOfType<UltimateTextDamageManager>();
        }

        // private void OnTriggerEnter(Collider other){
        //     _deltaTimeBeforePickup = _delayBeforePickup;
        //     _other = other;
        //     _startDeferredCharge = true;
        //     StartCoroutine(DeferredCharge());
        // }

        // private IEnumerator DeferredCharge(){
        //     if (_isMomentumCharge)
        //         while (_startDeferredCharge){
        //             // _deltaTimeBeforePickup -= Time.deltaTime;
        //             // if (_deltaTimeBeforePickup > 0) continue;
        //
        //             var targetItem = Source.Items.FirstOrDefault(i => i.Resource == ResourceAsset);
        //
        //             if (targetItem == null){
        //                 _startDeferredCharge = false;
        //                 _other = null;
        //                 yield break;
        //             }
        //
        //             var availableCount = targetItem.Amount;
        //
        //             if (availableCount <= 0){
        //                 _startDeferredCharge = false;
        //                 _other = null;
        //                 yield break;
        //             }
        //
        //             if (_other.TryGetComponent<ResourcesInventory>(out var inventory) == false){
        //                 _startDeferredCharge = false;
        //                 _other = null;
        //                 yield break;
        //             }
        //
        //             int itemCount = 0;
        //             if (availableCount >= ResourceAsset.CountInVisual){
        //                 itemCount = ResourceAsset.CountInVisual;
        //             } else{
        //                 itemCount = availableCount;
        //             }
        //
        //             Item item = ResourceAsset.CreateItem(itemCount);
        //             if (inventory.TryAddResource(item, out var count, _other.transform.position)){
        //                 _popupManager.Add($"+{count}", _other.transform, "money");
        //                 Source.TryRemoveResource(ResourceAsset.CreateItem(count));
        //                 TutorialController.InvokeAction(TutorAction.PickMoneyInMarket);
        //             }
        //         }
        // }

        private void OnTriggerStay(Collider other){
            if (_pickCooldown)
                return;

            var targetItem = Source.Items.FirstOrDefault(i => i.Resource == ResourceAsset);

            if (targetItem == null)
                return;

            var availableCount = targetItem.Amount;

            if (availableCount <= 0)
                return;

            if (other.TryGetComponent<ResourcesInventory>(out var inventory) == false)
                return;

            int itemCount = 0;
            if (availableCount >= ResourceAsset.CountInVisual){
                itemCount = ResourceAsset.CountInVisual;
            } else{
                itemCount = availableCount;
            }

            Item item = ResourceAsset.CreateItem(itemCount);
            if (inventory.TryAddResource(item, out var count, other.transform.position)){
                Source.TryRemoveResource(ResourceAsset.CreateItem(count));
                if (_isMomentumCharge && item.Resource.TypeResources == TypeResources.Money){
                    _popupManager.Add($"+{count}",new Vector3(
                        other.transform.position.x,
                        other.transform.position.y + _verticalOffsetToSHowPopup,
                        other.transform.position.z
                        ), "money");
                } else{
                    StartCoroutine(StartCooldown());
                }
                TutorialController.InvokeAction(TutorAction.PickMoneyInMarket);
            }
        }

        private IEnumerator StartCooldown(){
            _pickCooldown = true;
            yield return new WaitForSeconds(_delayBetweenPick);
            _pickCooldown = false;
        }
    }
}