using System;
using NavySpade.pj49.Scripts.Items.Inventory;
using NavySpade.pj49.Scripts.Productions.Factory;
using UnityEngine;

namespace NavySpade.pj49.Scripts
{ 
    public class UnitsCapacityHandler : MonoBehaviour
    {
        [SerializeField] private ResourceGroupAsset _unitsGroup;
        [SerializeField] private FactoryUpgradeParameter _upgradeParameter;

        public int Level { get; private set; }

        public FactoryUpgradeParameter Parameter => _upgradeParameter;

        public event Action Upgraded;

        public void Init(int level)
        {
            Level = level;
            UpdateCapacity();
        }

        public void LevelUp()
        {
            Level = Mathf.Clamp(Level + 1, 0, _upgradeParameter.UpgradesCount);
            UpdateCapacity();
        }

        private void UpdateCapacity()
        {
            Level = Mathf.Clamp(Level, 0, _upgradeParameter.UpgradesCount - 1);
            _unitsGroup.LimitCount = _upgradeParameter.GetIntValue(Level);
            Upgraded?.Invoke();
        }
    }
}