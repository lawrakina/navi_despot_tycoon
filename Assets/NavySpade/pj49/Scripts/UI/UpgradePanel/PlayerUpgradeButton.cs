using NavySpade.Core.Runtime.Player.Logic;
using NavySpade.pj49.Scripts.Items.Inventory;
using NavySpade.pj49.Scripts.Productions.Factory;

namespace NavySpade.pj49.Scripts.UI
{
    public class PlayerUpgradeButton : UpgradeButton
    {
        private FactoryUpgradeParameter _upgradeParameter;
        private FactoryUpgradeParameter.UpgradeInfo _nextUpgradeInfo;
        private UnitsCapacityHandler _capacityHandler;
        
        public void Init(UnitsCapacityHandler capacityHandler)
        {
            _capacityHandler = capacityHandler;
            _upgradeParameter = _capacityHandler.Parameter;
            capacityHandler.Upgraded += UpdateUI;
            UpdateUI();
        }
        
        private void UpdateUI()
        {
            _icon.sprite = _upgradeParameter.VisualData.Icon;
            _nameField.text = _upgradeParameter.VisualData.Name;
            _resourceIcon.gameObject.SetActive(_upgradeParameter.VisualData.HasResourceIcon);
            _resourceIcon.sprite = _upgradeParameter.VisualData.ResourceIcon;

            _nextUpgradeInfo = _upgradeParameter.GetUpgrade(_capacityHandler.Level + 1);
            _costField.text = _nextUpgradeInfo.Price.Value.ToString();
            _upgradeValueField.text = _upgradeParameter.VisualData.Prefix +
                                      _nextUpgradeInfo.value +
                                      _upgradeParameter.VisualData.Suffix;
            
            UpdateLevelView();
            UpdateButtonState();
        }

        private void UpdateLevelView()
        {
            if (_capacityHandler.Level < _upgradeParameter.UpgradesCount - 1)
            {
                _levelField.text = _capacityHandler.Level.ToString();
            }
            else
            {
                _levelField.text = "Max";
            }
        }

        private void UpdateButtonState()
        {
            bool canBuy = _capacityHandler.Level < _upgradeParameter.UpgradesCount - 1 && CanBuy();
            _button.interactable = canBuy;
            _grayMode.GrayMode = canBuy == false;
        }
        
        protected override void TryUpgrade()
        {
            if (_capacityHandler.Level < _upgradeParameter.UpgradesCount - 1)
            {
                FactoryUpgradeParameter.UpgradePrice priceInfo = _nextUpgradeInfo.Price;
                Item item = priceInfo.Resource.CreateItem(priceInfo.Value);
                if (SinglePlayer.Instance.Inventory.TryRemoveResource(item))
                {
                    _capacityHandler.LevelUp();
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