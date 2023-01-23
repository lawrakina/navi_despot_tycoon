using System;
using NavySpade.pj49.Scripts.Items.Inventory;
using NavySpade.pj49.Scripts.Productions;
using NavySpade.pj49.Scripts.Productions.Factory;
using TMPro;
using UnityEngine;


namespace NavySpade.pj49.Scripts.UI{
    public class BuildProgressVisual : MonoBehaviour{
        [SerializeField]
        private BuildingBuilder _buildingBuilder;
        [SerializeField]
        private TextMeshPro _progressText;
        [SerializeField]
        private ResourceAsset _targetParameter;
        private SpriteRenderer _icon;

        private void Awake(){
            _icon = GetComponentInChildren<SpriteRenderer>();
            if(_targetParameter != null)
                _icon.sprite = _targetParameter.Icon;
        }

        private void OnEnable(){
            _buildingBuilder.ProgressUpdated += UpdateVisual;
            UpdateVisual();
        }

        private void OnDisable(){
            _buildingBuilder.ProgressUpdated -= UpdateVisual;
        }

        private void UpdateVisual(){
            var leftCount = _buildingBuilder.HowManyLeftCountToComplete(_targetParameter);
            // var leftCount = _buildingBuilder.TargetCount - _buildingBuilder.ActualCount;
            leftCount = Mathf.Max(0, leftCount);
            _progressText.text = leftCount.ToString();
        }
    }
}