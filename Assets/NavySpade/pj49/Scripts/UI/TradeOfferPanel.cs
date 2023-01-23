using System;
using NavySpade.pj49.Scripts.Items;
using UnityEngine;

namespace NavySpade.pj49.Scripts.UI
{
    public class TradeOfferPanel : MonoBehaviour
    {
        [SerializeField] private TradeItem[] _inputItems;
        [SerializeField] private TradeItem[] _outputItems;
        [SerializeField] private ItemsTrader _itemsTrader;

        private void Start()
        {
            UpdateUI();
        }

        private void UpdateUI()
        {
            for (int i = 0; i < _itemsTrader.TradeOffers.Count; i++)
            {
                ItemsTrader.TradeOffer tradeOffer = _itemsTrader.TradeOffers[i];
                _inputItems[i].Init(tradeOffer.From, tradeOffer.FromCount);
                _outputItems[i].Init(tradeOffer.To, tradeOffer.ToCount);
            }
        }
    }
}