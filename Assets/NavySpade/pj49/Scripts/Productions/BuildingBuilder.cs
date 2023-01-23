using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mono.CSharp;
using NavySpade.Modules.Extensions.CsharpTypes;
using NavySpade.pj49.Scripts.Items.Inventory;
using NavySpade.pj49.Scripts.Productions.ProductionStates;
using NavySpade.pj49.Scripts.Tutorial;
using UnityEngine;
using UnityEngine.Events;


namespace NavySpade.pj49.Scripts.Productions{
    public class BuildingBuilder : ItemsReceiver{
        [SerializeField]
        private GameObject _buildingAppearEffect;
        [SerializeField]
        private float _appearDelay;
        [SerializeField]
        private bool _callOnFinishInInit;

        public UnityEvent OnFinish;
        public ResourceRequirement[] ResourceRequirements;

        // private bool _isBuiltCash;

        [field: SerializeField]
        public Transform TargetPoint{ get; private set; }


        public bool IsBuilt{
            get{
                // if (!_isBuiltCash)
                    return IsAllResourcesCompletly();
                // return _isBuiltCash;
            }
        }
        public List<int> GetValuesResourceRequirements =>
            ResourceRequirements.Select(requirement => requirement.Total).ToList();

        public event Action<ResourcesInventory, ResourceAsset> ResourcePickingAction;

        public event Action ProgressUpdated;

        private int[] GetCountsToComplete(){
            var list = new List<int>();
            foreach (var resource in ResourceRequirements){
                list.Add(resource.MaxCount - resource.Total);
            }

            return list.ToArray();
        }

        public int HowManyLeftCountToComplete(ResourceAsset resource){
            int result = 0;
            foreach (var source in ResourceRequirements){
                if (source.ResourceAsset == resource)
                    result = source.MaxCount - source.Total;
            }

            return result;
        }

        public void Init(List<int> currentCount){
            if (currentCount == null) return;
            for (int i = 0; i < currentCount.Count; i++){
                ResourceRequirements[i].AddToActual(currentCount[i]);
            }

            // _isBuiltCash = IsAllResourcesCompletly();
            ProgressUpdated?.Invoke();

            if (IsAllResourcesCompletly() && _callOnFinishInInit){
            // if (_isBuiltCash && _callOnFinishInInit){
                OnFinish?.Invoke();
            }
        }

        public override void PickupItemsToThis(ResourcesInventory playerInventory){
            if (IsAllResourcesCompletly())
                return;

            foreach (var res in ResourceRequirements){
                if (res.Total < res.MaxCount)
                    // if (!res.IsCompletly)
                    ResourcePickingAction?.Invoke(playerInventory, res.ResourceAsset);
            }
        }

        // public void AddResToProgress(int value)
        // {
        //     _countInProgress += value;
        //     _isBuiltCash = IsAllResourcesCompletly();
        // }

        // public void ApplyProgress(int value)
        // {
        //     ActualCount += value;
        //     _countInProgress -= value;
        //     
        //     ProgressUpdated?.Invoke();
        //     TutorialController.InvokeAction(TutorAction.Build);
        //     if (ActualCount >= TargetCount)
        //     {
        //         StartCoroutine(AppearDelay());
        //     }
        // }

        public void AddResToProgress(ResourceAsset resource, int count){
            var res = ResourceRequirements.FirstOrDefault(x => x.ResourceAsset == resource);
            res.AddToProgress(count);
        }

        public void ApplyProgress(ResourceAsset resource, int value, bool allInMoment = true){
            var res = ResourceRequirements.FirstOrDefault(x => x.ResourceAsset == resource);
            res.MoveToActualCounts(value, allInMoment);

            ProgressUpdated?.Invoke();
            TutorialController.InvokeAction(TutorAction.Build);
            if (IsAllResourcesCompletly()){
                StartCoroutine(AppearDelay());
            }
        }

        private bool IsAllResourcesCompletly(){
            int count = 0;
            foreach (var resource in ResourceRequirements){
                if (resource.IsCompletly){
                    count++;
                }
            }

            return count == ResourceRequirements.Length;
        }

        private IEnumerator AppearDelay(){
            Instantiate(_buildingAppearEffect, TargetPoint.transform.position,
                _buildingAppearEffect.transform.rotation);
            yield return new WaitForSeconds(_appearDelay);
            OnFinish.Invoke();
        }
    }

    [Serializable] public class ResourceRequirement{
        public ResourceAsset ResourceAsset;
        public int MaxCount;
        private int _actualCount;
        private int _countInProgress;

        public ResourceRequirement(ResourceAsset resource, int maxCount, int current){
            ResourceAsset = resource;
            MaxCount = maxCount;
            _actualCount = current;
        }

        public bool IsCompletly => _actualCount >= MaxCount;

        public int Total => _actualCount + _countInProgress;

        public void AddToProgress(int value){
            _countInProgress += value;
        }

        public void MoveToActualCounts(int value, bool allInMoment){
            if (allInMoment){
                _actualCount += _countInProgress;
                _countInProgress = 0;
                return;
            }

            _countInProgress -= value;
            _actualCount += value;
        }

        public void AddToActual(int value){
            _actualCount += value;
        }
    }
}