using System;
using System.Collections;
using System.Collections.Generic;
using NavySpade.pj49.Scripts.Items.Inventory;
using NavySpade.pj49.Scripts.Tutorial;
using UnityEngine;

namespace NavySpade.pj49.Scripts.Items
{
    public class ItemsTrader : MonoBehaviour
    {
        [SerializeField] private ResourcesInventory _toInventory;
        [SerializeField] private List<TradeOffer> _tradeOffers;
        [SerializeField] private float _tradeDelay;
        [SerializeField] private GameObject _moneyEffect;
        [SerializeField] private Transform _moneyEffectParent;

        private Coroutine _tradeCoroutine;
        private WaitForSeconds _waitTrade;

        public List<TradeOffer> TradeOffers => _tradeOffers;

        public event Action<ResourceAsset> ResourceTraded;

        private void Start()
        {
            _waitTrade = new WaitForSeconds(_tradeDelay);
        }

        private void OnTriggerStay(Collider other)
        {
            if(other.TryGetComponent<ResourcesInventory>(out var inventory) == false)
                return;

            if (_tradeCoroutine == null)
            {
               _tradeCoroutine = StartCoroutine(TradeWith(inventory, _toInventory));
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (_tradeCoroutine != null)
            {
                StopCoroutine(_tradeCoroutine);
                _tradeCoroutine = null;
            }
        }

        private IEnumerator TradeWith(ResourcesInventory fromInventory, ResourcesInventory toInventory)
        {
            while (true)
            {
                foreach (var tradeOffer in _tradeOffers)
                {
                    if (fromInventory.TryRemoveResource(tradeOffer.From.CreateItem(tradeOffer.FromCount)))
                    {
                        toInventory.TryAddResource(tradeOffer.To.CreateItem(tradeOffer.ToCount));
                        ResourceTraded?.Invoke(tradeOffer.From);
                        TutorialController.InvokeAction(TutorAction.MarketTraded);
                        // Instantiate(_moneyEffect, _moneyEffectParent);
                    }

                    yield return _waitTrade;
                }
            }
        }

        [Serializable]
        public struct TradeOffer
        {
            public ResourceAsset From;
            [Min(1)] public int FromCount;
            public ResourceAsset To;
            [Min(1)] public int ToCount;
            
        }
    }
}