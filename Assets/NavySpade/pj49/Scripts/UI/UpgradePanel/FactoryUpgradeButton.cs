using NavySpade.Core.Runtime.Player.Logic;
using NavySpade.pj49.Scripts.Items.Inventory;
using NavySpade.pj49.Scripts.Productions.Factory;
using UnityEngine;

namespace NavySpade.pj49.Scripts.UI
{
    public class FactoryUpgradeButton : UpgradeButton
    {
        private FactoryStats _factoryStats;
        private FactoryUpgradeParameter _upgradeParameter;
        private FactoryUpgradeParameter.UpgradeInfo _nextUpgradeInfo;
        
        private void OnDestroy()
        {
            FactoryStats.UpgradedGlobal -= UpdateUI;
        }
        
        public void Init(FactoryStats factoryStats, FactoryUpgradeParameter upgradeParameter)
        {
            FactoryStats.UpgradedGlobal += UpdateUI;
            _upgradeParameter = upgradeParameter;
            _factoryStats = factoryStats;
            UpdateUI();
        }

        private void UpdateUI()
        {
            _icon.sprite = _upgradeParameter.VisualData.Icon;
            _nameField.text = _upgradeParameter.VisualData.Name;
            _resourceIcon.gameObject.SetActive(_upgradeParameter.VisualData.HasResourceIcon);
            _resourceIcon.sprite = _upgradeParameter.VisualData.ResourceIcon;
            
            _nextUpgradeInfo = _factoryStats.GetNextUpgrade(_upgradeParameter);
            _costField.text = _nextUpgradeInfo.Price.Value.ToString();
            _upgradeValueField.text = _upgradeParameter.VisualData.Prefix + " " +
                                      _nextUpgradeInfo.value +
                                      " " + _upgradeParameter.VisualData.Suffix;
            
            UpdateLevelView();
            UpdateButtonState();
        }

        private void UpdateLevelView()
        {
            if (_factoryStats.HasUpgrade(_upgradeParameter))
            {
                _levelField.text = _factoryStats.GetUpgradeLevel(_upgradeParameter).ToString();
            }
            else
            {
                _levelField.text = "Max";
            }
        }

        private void UpdateButtonState()
        {
            bool canBuy = _factoryStats.HasUpgrade(_upgradeParameter) && CanBuy();
            _button.interactable = canBuy;
            _grayMode.GrayMode = canBuy == false;
        }
        
        protected override void TryUpgrade()
        {
            if (_factoryStats.HasUpgrade(_upgradeParameter))
            {
                FactoryUpgradeParameter.UpgradePrice priceInfo = _nextUpgradeInfo.Price;
                Item item = priceInfo.Resource.CreateItem(priceInfo.Value);
                if (SinglePlayer.Instance.Inventory.TryRemoveResource(item))
                {
                    _factoryStats.LevelUp(_upgradeParameter);
                    UpdateUI();
                }
            }
        }

        private bool CanBuy()
        {
            FactoryUpgradeParameter.UpgradePrice priceInfo =_nextUpgradeInfo.Price;
            Item item = priceInfo.Resource.CreateItem(priceInfo.Value);
            return SinglePlayer.Instance.Inventory.Contains(item);
        }
    }
}