using System;
using NavySpade.Core.Runtime.Player.Logic;
using NavySpade.pj49.Scripts.Items.Inventory;
using NavySpade.pj49.Scripts.Productions.Factory;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace NavySpade.pj49.Scripts.UI
{
    public abstract class UpgradeButton : MonoBehaviour
    {
        [SerializeField] protected Image _icon;
        [SerializeField] protected TextMeshProUGUI _nameField;
        [SerializeField] protected TextMeshProUGUI _costField;
        [SerializeField] protected TextMeshProUGUI _levelField;
        [SerializeField] protected TextMeshProUGUI _upgradeValueField;
        [SerializeField] protected Button _button;
        [SerializeField] protected UIGrayMode _grayMode;
        [SerializeField] protected Image _resourceIcon;
        
        private void Start()
        {
            _button.onClick.AddListener(TryUpgrade);
        }

        protected abstract void TryUpgrade();
    }
}