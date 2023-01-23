using System;
using NaughtyAttributes;
using NavySpade.Core.Runtime.Levels;
using NavySpade.pj49.Scripts.Items.Inventory;
using NavySpade.pj49.Scripts.Productions.Arena.Boss;
using NavySpade.pj49.Scripts.Productions.Factory;
using UnityEngine;
using Random = UnityEngine.Random;


namespace NavySpade.pj49.Scripts.Productions.Arena{
    [CreateAssetMenu(fileName = nameof(ArenaConfig), menuName = "Game/pj49/" + nameof(ArenaConfig))]
    public class ArenaConfig : ScriptableObject{
        [SerializeField]
        private FlyingBullet[] _bullets;

        [field: Header("Input resources")]
        [SerializeField]
        private FactoryRequirement[] _factoryRequirements;

        [field: Header("Production Flow")]
        [SerializeField]
        private bool _hasProductionCooldown;

        [Header("Victory condition")]
        [SerializeField]
        private FactoryUpgradeParameter _victoryConditionsByLevels;

        [SerializeField]
        [ShowIf(nameof(_hasProductionCooldown))]
        private FactoryUpgradeParameter _productionCooldown;

        [SerializeField]
        private FactoryUpgradeParameter _parallelProductionCount;

        [SerializeField]
        private FactoryUpgradeParameter _productionTime;

        [SerializeField]
        private FactoryUpgradeParameter[] _changeableUpgrades;

        [SerializeField]
        private bool _useResourceMultiplier;

        [SerializeField]
        private FactoryUpgradeParameter _bossAttackSpeed;
        [SerializeField]
        private FactoryUpgradeParameter _bossAttackValue;
        [SerializeField]
        public float DelayBeforeAttack = 0.5f;
        [SerializeField]
        private KeyView _keyPrefab;
        [Header("Size of InputZone")]
        [SerializeField]
        private int _inputZonePointsCount = 10;
        [SerializeField]
        private float _inputZoneRadius = 4.5f;

        public FactoryUpgradeParameter VictoryConditionsByLevels => _victoryConditionsByLevels;
        public FactoryUpgradeParameter ProductionCooldown => _productionCooldown;

        public FactoryUpgradeParameter ProductionTime => _productionTime;

        public FactoryUpgradeParameter ParallelProductionCount => _parallelProductionCount;

        public FactoryUpgradeParameter[] ChangeableUpgrades => _changeableUpgrades;
        public KeyView KeyPrefab => _keyPrefab;
        public FactoryUpgradeParameter BossAttackSpeed => _bossAttackSpeed;
        public FactoryUpgradeParameter BossAttackValue => _bossAttackValue;
        public (int PointCount, float Radius) InputZoneSize => (_inputZonePointsCount, _inputZoneRadius);
        public int InputMaxCount => _factoryRequirements[0].MaxInputCapacity.GetIntValue(LevelManager.CurrentLevelIndex);

        public FlyingBullet RandomBullet(){
            return _bullets[Random.Range(0, _bullets.Length - 1)];
        }

        [Serializable] public class FactoryRequirement{
            [field: SerializeField]
            public bool UseGroupAsset{ get; private set; }

            [field: HideIf(nameof(UseGroupAsset))]
            [field: AllowNesting]
            [field: SerializeField]
            public ResourceAsset Resource{ get; private set; }

            [field: ShowIf(nameof(UseGroupAsset))]
            [field: AllowNesting]
            [field: SerializeField]
            public ResourceGroupAsset ResourceGroup{ get; private set; }

            [field: SerializeField]
            public FactoryUpgradeParameter MaxInputCapacity{ get; private set; }
            [field: SerializeField]
            public int NeedCountToStart{ get; private set; }
        }

        public FactoryRequirement GetGroupRequirement(ResourceGroupAsset groupAsset){
            foreach (var requirement in _factoryRequirements){
                if (requirement.UseGroupAsset == false){
                    if (requirement.Resource.Group == groupAsset){
                        return requirement;
                    }

                    continue;
                }

                if (requirement.ResourceGroup == groupAsset){
                    return requirement;
                }
            }

            return null;
        }
    }
}