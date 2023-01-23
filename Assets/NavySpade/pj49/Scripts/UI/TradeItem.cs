using System;
using NavySpade.pj49.Scripts.Items.Inventory;
using NavySpade.pj49.Scripts.Productions.Factory;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NavySpade.pj49.Scripts.UI
{
    public class TradeItem : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _resourceCount;

        private FactoryConfig.FactoryRequirement _requirement;
        
        public void Init(FactoryConfig.FactoryRequirement requirement)
        {
            _requirement = requirement;
            _icon.sprite = GetIcon();
            _resourceCount.text = "x " + requirement.NeedCountToStart;
        }

        public void Init(ResourceAsset asset, int need)
        {
            _icon.sprite = asset.Icon;
            _resourceCount.text = "x " + need;
        }

        private Sprite GetIcon()
        {
            if (_requirement.UseGroupAsset)
            {
                return _requirement.ResourceGroup.Icon;
            }
            else
            {
                return _requirement.Resource.Icon;
            }
        }
    }
}