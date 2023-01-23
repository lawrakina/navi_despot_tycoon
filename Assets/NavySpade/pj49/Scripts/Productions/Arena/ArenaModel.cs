using System;
using System.Collections.Generic;
using System.Threading;
using NavySpade.Core.Runtime.Levels;
using NavySpade.Core.Runtime.Player.Logic;
using NavySpade.pj49.Scripts.Items;
using NavySpade.pj49.Scripts.Productions.Arena.Boss;
using NavySpade.pj49.Scripts.Productions.Factory;
using UniRx;
using UnityEngine;


namespace NavySpade.pj49.Scripts.Productions.Arena{
    public class ArenaModel{
        private Dictionary<FactoryUpgradeParameter, int> _upgradesDict = new Dictionary<FactoryUpgradeParameter, int>();
        private ArenaConfig _config;
        private ReactiveProperty<int> _bossHp;

        public void Init(ArenaConfig config, ArenaSavingData arenaSavingData)
        {
            _config = config;
            for (int i = 0; i < _config.ChangeableUpgrades.Length; i++)
            {
                int level = arenaSavingData.UpgradesLevel == null ? 0 : arenaSavingData.UpgradesLevel[i];
                _upgradesDict.Add(_config.ChangeableUpgrades[i], level);
            }

            if (arenaSavingData.HealthPoints == 0)
                arenaSavingData.HealthPoints = (int) _config.VictoryConditionsByLevels.GetValue(LevelManager.CurrentLevelIndex);
            
            _bossHp = new ReactiveProperty<int>(arenaSavingData.HealthPoints);
            OnBossDie = new ReactiveProperty<bool>(false);
            OnShowKey = new ReactiveProperty<bool>(false);
        }


        public event Action<FactoryUpgradeParameter, int> Upgraded;

        public static event Action UpgradedGlobal;

        // public float ProductionTime => _config.ProductionTime.GetIntValue(GetUpgradeLevel(_config.ProductionTime));

        // public float ProductionCooldown => _config.ProductionCooldown.GetValue(GetUpgradeLevel(_config.ProductionCooldown));

        // public int ParallelProductionCount => _config.ParallelProductionCount.GetIntValue(GetUpgradeLevel(_config.ParallelProductionCount));
        public float BossAttackSpeed => _config.BossAttackSpeed.GetValue(LevelManager.CurrentLevelIndex);
        public int BossAttackValue => _config.BossAttackValue.GetIntValue(LevelManager.CurrentLevelIndex);

        public ReactiveProperty<int> BossHp => _bossHp;
        public ReactiveProperty<bool> OnBossDie{ get; private set; }
        public ReactiveProperty<bool> OnShowKey{ get; private set; }
        public UnitQueue Units{ get; set; }
        // public int VictoryCondition => _config.VictoryConditionsByLevels.GetIntValue(LevelManager.CurrentLevelIndex);
        public float DelayBeforeAttack => _config.DelayBeforeAttack;
        public (int PointCount, float Radius) InputZoneSize => _config.InputZoneSize;
        public int InputMaxCount => _config.InputMaxCount;

        public ReactiveProperty<int> UnitsHp = new ReactiveProperty<int>();

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