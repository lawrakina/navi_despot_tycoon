using System;
using NavySpade.pj49.Scripts.Items.Inventory;
using TMPro;
using UnityEngine;

namespace NavySpade.pj49.Scripts.UI
{
    public class ResourceCounterView : MonoBehaviour
    {
        [SerializeField] private ResourceAsset _resourceAsset;
        [SerializeField] private ResourcesInventory _resourcesInventory;
        [SerializeField] private TextMeshProUGUI _textCounter;

        private void Start()
        {
            _resourcesInventory.ResourcesCountChanged += UpdateField;
            UpdateField();
        }

        private void OnDestroy()
        {
            _resourcesInventory.ResourcesCountChanged -= UpdateField;
        }

        private void UpdateField()
        {
            _textCounter.text = "$ " + _resourcesInventory.Count(_resourceAsset);
        }
    }
}