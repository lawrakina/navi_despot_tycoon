using System;
using System.Collections;
using System.Linq;
using NaughtyAttributes;
using NavySpade.pj49.Scripts.Items.Inventory;
using UnityEngine;


namespace NavySpade.pj49.Scripts.Productions.Factory{
    [CreateAssetMenu(fileName = "FactoryConfig", menuName = "Game/pj49/FactoryConfig")]
    public class FactoryConfig : ScriptableObject{
        [field: Header("Input resources")]
        [SerializeField]
        private FactoryRequirement[] _factoryRequirements;

        [field: Header("Output resources")]
        [SerializeField]
        private ResourceAsset _outputResources;
        [SerializeField]
        private FactoryUpgradeParameter _outputCount;
        [SerializeField]
        private bool _hasOutputMax;

        [SerializeField]
        [ShowIf(nameof(_hasOutputMax))]
        private FactoryUpgradeParameter _outputMax;

        [field: Header("Production Flow")]
        [SerializeField]
        private bool _hasProductionCooldown;

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
        [Header("For Mine production")]
        [SerializeField]
        private ResourceAsset _resourceForRestartProduction;
        [SerializeField]
        private FactoryRequirement[] _resourcesForRestartProduction;

        [Header("Units configs")]
        [SerializeField]
        private FactoryUpgradeParameter _muscleUnitHealthPoints;
        [SerializeField]
        private FactoryUpgradeParameter _muscleUnitPowerAttack;
        [SerializeField]
        private FactoryUpgradeParameter _maxInputCapacity;
        public bool HasInputResource => _factoryRequirements.Length > 0;

        public bool HasProductionCooldown => _hasProductionCooldown;

        public FactoryRequirement[] FactoryRequirements => _factoryRequirements;

        public ResourceAsset OutputResources => _outputResources;

        public FactoryUpgradeParameter OutputCount => _outputCount;

        public bool HasOutputMax => _hasOutputMax;

        public FactoryUpgradeParameter OutputMax => _outputMax;

        public FactoryUpgradeParameter ProductionCooldown => _productionCooldown;

        public FactoryUpgradeParameter ProductionTime => _productionTime;
        public FactoryUpgradeParameter ParallelProductionCount => _parallelProductionCount;

        public FactoryUpgradeParameter[] ChangeableUpgrades => _changeableUpgrades;
        public FactoryUpgradeParameter MuscleUnitHealthPoints => _muscleUnitHealthPoints;
        public FactoryUpgradeParameter MuscleUnitPowerAttack => _muscleUnitPowerAttack;
        public bool UseResourceMultiplier => _useResourceMultiplier;
        public FactoryUpgradeParameter MaxInputCapacity => _maxInputCapacity;
        public ResourceAsset ResourceForRestartProduction => _resourceForRestartProduction;
        public FactoryRequirement[] ResourcesForRestartProduction => _resourcesForRestartProduction;

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

        public ResourceAsset GetFactoryRequirements(TypeResources typeResources){
            return _factoryRequirements.First(x => x.Resource.TypeResources == typeResources).Resource;
        }
    }
}