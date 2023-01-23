using System.Collections;
using NavySpade.Core.Runtime.Player.Logic;
using NavySpade.Modules.Extensions.UnityTypes;
using NavySpade.pj49.Scripts.Items;
using NavySpade.pj49.Scripts.Items.Inventory;
using UnityEngine;

namespace NavySpade.pj49.Scripts.Productions
{
    public abstract class ItemsReceiver : ExtendedMonoBehavior
    {
        [field: SerializeField] public float DelayToPickupItem { get; private set; } = .3f;

        private Coroutine _pickupItemsRoutine;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<ResourcesInventory>(out var inventory) == false)
                return;

            if (_pickupItemsRoutine == null)
            {
                _pickupItemsRoutine = StartCoroutine(PickupItemsForWorkRoutine(inventory));
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<ResourcesInventory>(out _) == false)
                return;

            if (_pickupItemsRoutine != null)
            {
                StopCoroutine(_pickupItemsRoutine);
                _pickupItemsRoutine = null;
            }
        }

        private IEnumerator PickupItemsForWorkRoutine(ResourcesInventory playerInventory)
        {
            // var playerMovement = SinglePlayer.Instance.GetComponent<PlayerMovement>();
            //
            // while (playerMovement.InputDirection != Vector2.zero)
            // {
            //     yield return null;
            // }

            while (true)
            {
                yield return new WaitForSeconds(DelayToPickupItem);
                PickupItemsToThis(playerInventory);
            }
        }

        public abstract void PickupItemsToThis(ResourcesInventory playerInventory);
    }
}