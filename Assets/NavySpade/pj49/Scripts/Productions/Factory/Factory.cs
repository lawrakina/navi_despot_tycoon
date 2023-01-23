using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using NavySpade.pj49.Scripts.Items.Inventory;
using NavySpade.pj49.Scripts.Productions.EnergyFactory;
using NavySpade.pj49.Scripts.Productions.ProductionStates;
using NavySpade.pj49.Scripts.Saving;
using UnityEngine;


namespace NavySpade.pj49.Scripts.Productions.Factory{
    public class Factory : MonoBehaviour, ISaveable{
        [SerializeField]
        private FactoryConfig _config;
        [SerializeField]
        private ResourcesInventory inputInventory;
        [SerializeField]
        private ResourcesInventory outputInventory;
        [SerializeField]
        private BuildingBuilder _builder;
        [SerializeField]
        private GameObject[] _buildings;

        [Header("Boosters")]
        [SerializeField]
        private bool _hasElectrostation;

        [SerializeField]
        [ShowIf(nameof(_hasElectrostation))]
        private ElectroStationStateMachine _electroStation;

        private int _buildIndex;
        private bool _isCooldown;
        private FactoryStats _factoryStats;

        public bool InProductionProcess{ get; set; }

        public float ElapsedTimeOfProduction{ get; set; }

        public float ProductionBoost{
            get{
                if (_hasElectrostation && _electroStation.IsWork){
                    return _electroStation.BoostValue;
                }

                return 1;
            }
        }

        public ResourcesInventory InputInventory => inputInventory;
        public ResourcesInventory OutputInventory => outputInventory;

        public FactoryConfig Config => _config;

        public FactoryStats Stats => _factoryStats;

        public event Action ProductionStarted;

        public event Action FactoryInitialise;

        public void Init(int buildIndex){
            _buildIndex = buildIndex;
            LevelSaver.Instance.Register(this);
            FactorySavingData savingData = LevelSaver.LoadBuildSaving(_buildIndex);
            _builder.Init(savingData.BuildProgress);
            // _builder.OnFinish += FinishBuild;
            inputInventory.Init(savingData.InputItems);
            OutputInventory.Init(savingData.OutputItems);

            _factoryStats = new FactoryStats();
            _factoryStats.Init(_config, savingData);
            InitBuildState();
            FactoryInitialise?.Invoke();
        }

        private void FinishBuild(){
            
        }

        public void InitBuildState(){
            if (_builder.IsBuilt){
                _builder.gameObject.SetActive(false);
            } else{
                _builder.gameObject.SetActive(true);
            }

            foreach (var building in _buildings){
                building.SetActive(_builder.IsBuilt);
            }
        }

        private void Update(){
            if (_builder.IsBuilt == false)
                return;

            if (TryStartProduce()){
                GetStartingResources();
                ProductionStarted?.Invoke();

                if (_config.HasProductionCooldown){
                    StartCoroutine(StartProductionCooldown());
                }
            }
        }

        private IEnumerator StartProductionCooldown(){
            _isCooldown = true;
            yield return new WaitForSeconds(_factoryStats.ProductionCooldown);
            _isCooldown = false;
        }

        private bool TryStartProduce(){
            return _isCooldown == false &&
                   InProductionProcess == false &&
                   HasResourcesForStart() &&
                   HasFreeSpaceForResources(_config.OutputResources);
        }

        private void RestartWork(){
            // if(_isWorking)
            //     return;
            // if (_workCoroutine != null)
            // {
            //     StopCoroutine(_workCoroutine);
            // }
            //
            // StartWork();
        }

        public void PickupItem(Item item){
            inputInventory.TryAddResource(item);
            RestartWork();
        }

        private bool HasResourcesForStart(){
            if (_config.HasInputResource == false)
                return true;

            var factoryRequirements = _config.FactoryRequirements;
            foreach (var requirement in factoryRequirements){
                if (requirement.UseGroupAsset){
                    int countInInventory = inputInventory.Count(requirement.ResourceGroup);
                    if (countInInventory < requirement.NeedCountToStart)
                        return false;
                } else{
                    Item item = requirement.Resource.CreateItem(requirement.NeedCountToStart);
                    if (inputInventory.Contains(item) == false)
                        return false;
                }
            }

            return true;
        }

        private void GetStartingResources(){
            var factoryRequirements = _config.FactoryRequirements;
            foreach (var requirement in factoryRequirements){
                if (requirement.UseGroupAsset){
                    inputInventory.TryRemoveResource(requirement.ResourceGroup, requirement.NeedCountToStart);
                } else{
                    Item item = requirement.Resource.CreateItem(requirement.NeedCountToStart);
                    inputInventory.TryRemoveResource(item);
                }
            }
        }

        public int GetResourceCountToStart(){
            int total = 0;
            foreach (var requirement in _config.FactoryRequirements){
                total += requirement.NeedCountToStart;
            }

            return total;
        }

        public bool HasFreeSpaceForResources(ResourceAsset asset){
            if (_config.HasOutputMax == false)
                return true;

            int countInInventory = OutputInventory.Count(asset);
            return countInInventory < _factoryStats.OutputMax;
        }

        public void ConvertItemsFrom(ResourceAsset from){
            int totalCount = _factoryStats.OutputCount;
            if (_config.UseResourceMultiplier){
                totalCount *= from.Multiplier;
            }

            var item = _config.OutputResources.CreateItem(totalCount);
            OutputInventory.TryAddResource(item);
            RestartWork();
        }

        public void ProduceItem(){
            var item = _config.OutputResources.CreateItem(1);
            OutputInventory.TryAddResource(item);
            RestartWork();
        }

        public bool CanPickupItem(ResourceAsset resourceAsset){
            int count = inputInventory.Count(resourceAsset);
            foreach (var requirement in _config.FactoryRequirements){
                if (requirement.Resource != resourceAsset)
                    continue;

                var parameter = requirement.MaxInputCapacity;
                return count < parameter.GetIntValue(_factoryStats.GetUpgradeLevel(parameter));
            }

            return false;
        }

        public bool CanPickupSecondItem(ResourceAsset resourceAsset){
            var count = inputInventory.Count(resourceAsset);
            foreach (var requirement in _config.ResourcesForRestartProduction){
                if (requirement.Resource != resourceAsset)
                    continue;

                return count > 0;
            }

            return false;
        }

        public void Save(){
            List<int> levelOfUpgrades = new List<int>();
            for (int i = 0; i < _config.ChangeableUpgrades.Length; i++){
                levelOfUpgrades.Add(_factoryStats.GetUpgradeLevel(_config.ChangeableUpgrades[i]));
            }

            FactorySavingData savingData = new FactorySavingData();
            savingData.InputItems = inputInventory.GetSaveableInventory();
            savingData.OutputItems = OutputInventory.GetSaveableInventory();
            savingData.UpgradesLevel = levelOfUpgrades;
            savingData.BuildProgress = _builder.GetValuesResourceRequirements;
            LevelSaver.SaveBuildSaving(_buildIndex, savingData);
        }
    }
}