using System;
using NavySpade.pj49.Scripts.Productions.Factory;
using UnityEngine;

namespace NavySpade.pj49.Scripts.UI
{
    public class FactoryConvertInfo : MonoBehaviour
    {
        [SerializeField] private Factory _factory;
        [SerializeField] private TradeItem[] _inputTradeItems;
        [SerializeField] private TradeItem _outputTradeItems;

        private void Start()
        {
            UpdateInfo();
            _factory.Stats.Upgraded += UpdateInfo;
        }
        
        private void OnDestroy()
        {
            _factory.Stats.Upgraded -= UpdateInfo;
        }
        
        private void UpdateInfo(FactoryUpgradeParameter res, int level)
        {
            UpdateInfo();
        }

        private void UpdateInfo()
        {
            var factoryRequirements = _factory.Config.FactoryRequirements;
            for (int i = 0; i < factoryRequirements.Length; i++)
            {
                _inputTradeItems[i].Init(factoryRequirements[i]);
            }

            var parameter = _factory.Config.OutputCount;
            _outputTradeItems.Init(
                _factory.Config.OutputResources,
                parameter.GetIntValue(_factory.Stats.GetUpgradeLevel(parameter)));
        }
    }
}