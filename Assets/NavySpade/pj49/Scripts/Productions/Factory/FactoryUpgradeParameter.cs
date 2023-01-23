using System;
using NaughtyAttributes;
using NavySpade.pj49.Scripts.Items.Inventory;
using UnityEngine;

namespace NavySpade.pj49.Scripts.Productions.Factory
{
    [CreateAssetMenu(fileName = "FactoryUpgradeParameter", menuName = "Game/pj49/FactoryUpgradeParameter")]
    public class FactoryUpgradeParameter : ScriptableObject
    {
        [SerializeField] private UpgradeVisualData _visualData;
        [SerializeField] private UpgradeInfo[] _upgrades;
        
        public UpgradeVisualData VisualData => _visualData;

        public int UpgradesCount => _upgrades.Length;
        
        public float GetValue(int level)
        {
            level = Mathf.Clamp(level, 0, UpgradesCount - 1);
            return _upgrades[level].value;
        }
        
        public int GetIntValue(int level)
        {
            level = Mathf.Clamp(level, 0, UpgradesCount - 1);
            return (int) _upgrades[level].value;
        }

        public UpgradeInfo GetUpgrade(int level)
        {
            level = Mathf.Clamp(level, 0, UpgradesCount - 1);
            return _upgrades[level];
        }
        
        [Serializable]
        public struct UpgradeInfo
        {
            public float value;
            public UpgradePrice Price;
        }
        
        [Serializable]
        public struct UpgradePrice
        {
            public ResourceAsset Resource;
            public int Value;
        }
    }
}