using System;
using System.Collections.Generic;
using NavySpade.pj49.Scripts.Items.Inventory;
using NavySpade.pj49.Scripts.Productions.ProductionStates;
using NavySpade.pj49.Scripts.Saving;
using UnityEngine;
using Object = UnityEngine.Object;


namespace NavySpade.pj49.Scripts.Productions.Factory
{
    public class FactoryStats
    {
        private Dictionary<FactoryUpgradeParameter, int> _upgradesDict = new Dictionary<FactoryUpgradeParameter, int>();
        private FactoryConfig _config;

        public void Init(FactoryConfig config, FactorySavingData factorySavingData)
        {
            _config = config;
            for (int i = 0; i < _config.ChangeableUpgrades.Length; i++)
            {
                int level = factorySavingData.UpgradesLevel == null ? 0 : factorySavingData.UpgradesLevel[i];
                _upgradesDict.Add(_config.ChangeableUpgrades[i], level);
            }
        }

        public event Action<FactoryUpgradeParameter, int> Upgraded;

        public static event Action UpgradedGlobal;
        
        public float ProductionTime => _config.ProductionTime.GetIntValue(GetUpgradeLevel(_config.ProductionTime));

        public float ProductionCooldown => _config.ProductionCooldown.GetValue(GetUpgradeLevel(_config.ProductionCooldown));

        public int OutputMax => _config.OutputMax.GetIntValue(GetUpgradeLevel(_config.OutputMax));
        
        public int OutputCount => _config.OutputCount.GetIntValue(GetUpgradeLevel(_config.OutputCount));
        public int ParallelProductionCount => _config.ParallelProductionCount.GetIntValue(GetUpgradeLevel(_config.ParallelProductionCount));
        public Conserve OutputPrefab => _config.OutputResources.Prefab.GetComponent<Conserve>();
        public int MuscleUnitPowerAttack => _config.MuscleUnitPowerAttack.GetIntValue(GetUpgradeLevel(_config.MuscleUnitPowerAttack));
        public int MaxInputCapacity => _config.MaxInputCapacity.GetIntValue(GetUpgradeLevel(_config.MaxInputCapacity));
        public ResourceAsset ResourceForRestartProduction => _config.ResourceForRestartProduction;

        public int GetUpgradeLevel(FactoryUpgradeParameter parameter)
        {
            if (_upgradesDict.TryGetValue(parameter, out int level))
            {
                return level;
            }

            return 0;
        }

        public bool HasUpgrade(FactoryUpgradeParameter parameter)
        {
            if (_upgradesDict.TryGetValue(parameter, out int level))
            {
                return level < parameter.UpgradesCount - 1;
            }

            return false;
        }
        
        public FactoryUpgradeParameter.UpgradeInfo GetNextUpgrade(FactoryUpgradeParameter parameter)
        {
            if (_upgradesDict.TryGetValue(parameter, out int level))
            {
                return parameter.GetUpgrade(level + 1);
            }

            return parameter.GetUpgrade(0);
        }
        
        public void LevelUp(FactoryUpgradeParameter parameter)
        {
            if (_upgradesDict.ContainsKey(parameter))
            {
                int level = _upgradesDict[parameter];
                level = Mathf.Clamp(level + 1, 0, parameter.UpgradesCount);
                _upgradesDict[parameter] = level;
                
                Upgraded?.Invoke(parameter, level);
                UpgradedGlobal?.Invoke();
            }
        }

        public float GetValueOf(FactoryUpgradeParameter parameter)
        {
            if (_upgradesDict.TryGetValue(parameter, out int level))
            {
                return parameter.GetValue(level);
            }

            return parameter.GetValue(0);
        }
        
        public int GetIntValueOf(FactoryUpgradeParameter parameter)
        {
            if (_upgradesDict.TryGetValue(parameter, out int level))
            {
                return (int) parameter.GetValue(level);
            }

            return (int) parameter.GetValue(0);
        }
    }
}