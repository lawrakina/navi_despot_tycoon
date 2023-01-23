using System;
using System.Collections;
using System.Linq;
using NavySpade.Core.Runtime.Player.Logic;
using NavySpade.pj49.Scripts.Items.Inventory;
using pj40.Core.Tweens;
using UnityEngine;


namespace NavySpade.pj49.Scripts.Productions.ProductionStates{
    public class BuildResourceBehaviour : MonoBehaviour{
        public enum WithdrawType{
            OneOneAll,
            OneTwoThree,
        }

        [SerializeField]
        private ResourceGroupAsset _itemsGroup;
        [SerializeField]
        private BuildingBuilder _buildingBuilder;
        [SerializeField]
        private GameObject _resourcePuffEffect;
        [SerializeField]
        private float _arkHeight;
        [SerializeField]
        private float _speed;
        [SerializeField]
        private float _delay;

        [Header("Incremental cash withdrawal")]
        [SerializeField]
        private WithdrawType _withdrawType;
        [SerializeField]
        private int _incrementChanges;
        [SerializeField]
        private int _incrementCount;

        private int _currentIteration;

        private void OnEnable(){
            //_buildingBuilder.ResourcePickedUp += SpawnAnimation;
            _buildingBuilder.ResourcePickingAction += PickUp;
        }

        private void OnDisable(){
            //_buildingBuilder.ResourcePickedUp -= SpawnAnimation;
            _buildingBuilder.ResourcePickingAction -= PickUp;
        }

        private void OnTriggerExit(Collider other){
            if (other.TryGetComponent<ResourcesInventory>(out _) == false)
                return;

            _currentIteration = 0;
        }

        private void PickUp(ResourcesInventory from, ResourceAsset resourceAsset){
            if (resourceAsset.Group != null && resourceAsset.Group != _itemsGroup)
                return;

            Item item = GetWithdrawalItem(from, resourceAsset);

            if (from.TryRemoveResource(item) == false)
                return;

            int count = item.Amount * resourceAsset.Multiplier;
            _buildingBuilder.AddResToProgress(resourceAsset, count);
            SpawnAnimation(resourceAsset, count);
            _currentIteration++;
        }

        private Item GetWithdrawalItem(ResourcesInventory from, ResourceAsset resourceAsset){
            Item item = null;
            int itemInInventory = 0;
            int needToCompleteBuild = 0;
            switch (_withdrawType){
                case WithdrawType.OneOneAll:
                    if (_currentIteration < _incrementChanges){
                        item = resourceAsset.CreateItem();
                    } else{
                        itemInInventory = from.Count(resourceAsset);
                        needToCompleteBuild = _buildingBuilder.HowManyLeftCountToComplete(resourceAsset);
                        item = resourceAsset.CreateItem(Mathf.Min(itemInInventory, needToCompleteBuild));
                    }

                    break;
                case WithdrawType.OneTwoThree:
                    int takeItemCount = (1 + (_currentIteration / _incrementChanges) * _incrementCount);
                    needToCompleteBuild = Mathf.Min(takeItemCount,
                        _buildingBuilder.HowManyLeftCountToComplete(resourceAsset));
                    itemInInventory = from.Count(resourceAsset);
                    item = resourceAsset.CreateItem(Mathf.Min(itemInInventory, needToCompleteBuild));
                    break;
            }

            return item;
        }

        private void SpawnAnimation(ResourceAsset res, int value){
            var item = Instantiate(res.Prefab, SinglePlayer.Instance.transform.position, Quaternion.identity);
            var tween = new MoveToTransformTween<ParabolaMovement>(
                item.transform,
                new ParabolaMovement(_arkHeight),
                Vector3.zero, .01f);

            tween.StartTween(_buildingBuilder.TargetPoint, _speed, () => {
                Instantiate(_resourcePuffEffect, _buildingBuilder.TargetPoint.position, Quaternion.identity);
                Destroy(item);
                StartCoroutine(UpdateProgressDelay(res, value));
            });

            StartCoroutine(UpdateTweenProgress(tween));
        }

        private IEnumerator UpdateProgressDelay(ResourceAsset res, int value){
            yield return new WaitForSeconds(_delay);
            _buildingBuilder.ApplyProgress(res, value, _withdrawType == WithdrawType.OneOneAll ? true : false);
        }

        private IEnumerator UpdateTweenProgress(MoveToTransformTween<ParabolaMovement> tween){
            while (tween.IsFinished == false){
                tween.Update(Time.deltaTime);
                yield return null;
            }
        }
    }
}