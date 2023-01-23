using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EventSystem.Runtime.Core.Dispose;
using EventSystem.Runtime.Core.Managers;
using Misc.RootProviders.Runtime.Base;
using Mono.CSharp;
using NavySpade.Core.Runtime.Player.Logic;
using NavySpade.pj49.Scripts.Items;
using NavySpade.pj49.Scripts.Items.Inventory;
using NavySpade.pj49.Scripts.UnitsQueues;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NavySpade.pj49.Scripts.UI
{
    public class ResourceUI : MonoBehaviour
    {
        public ResourceAsset Asset;
        public TMP_Text Text;
        public string LimitedResourceFormat = "{0}/{1}";
        public bool IsDisplayMaxCount;
        
        [SubclassSelector] [SerializeReference] public RootProvider Root;

        [SerializeField] private PickedResUI _pickedResPrefab;
        [SerializeField] private Image _resIcon;
        [SerializeField] private bool _useAnimation;

        private EventDisposal _disposal = new EventDisposal();
        private ResourcesInventory _inventory;

        protected void Start()
        {
            EventManager.Add<SinglePlayer>(GenerateEnumEM.SetPlayer, Init).AddTo(_disposal);
        }

        private void OnDestroy()
        {
            _disposal.Dispose();
        }

        protected virtual void Init(SinglePlayer player)
        {
            SetInventory(player);
        }

        private void SetInventory(SinglePlayer player)
        {
            _inventory = SinglePlayer.Instance.GetComponent<ResourcesInventory>();
            _inventory.ResourcePicked += OnResourcePicked;
            _inventory.ResourceSpend += OnResourceSpent;
            Root.HideInstantly();
            CheckUIState();
            UpdateCount();
        }

        private void OnResourceSpent(ResourceAsset resourceAsset)
        {
            if(NeedUpdate(resourceAsset) == false)
                return;
            
            CheckUIState();
            UpdateCount();
        }

        private void OnResourcePicked(ResourceAsset resourceAsset, Vector3 pickedObjPos)
        {
            if(NeedUpdate(resourceAsset) == false)
                return;
            
            if (_useAnimation)
            {
                CheckUIState();
                StartCoroutine(PlayResourceAnimation(resourceAsset, pickedObjPos));
            }
            else
            {
                CheckUIState();
                UpdateCount();
            }
        }

        private bool NeedUpdate(ResourceAsset resourceAsset)
        {
            if (IsDisplayMaxCount)
            {
                if(resourceAsset.Group != Asset.Group)
                    return false;
            }
            else
            {
                if(resourceAsset != Asset)
                    return false;
            }

            return true;
        }

        private IEnumerator PlayResourceAnimation(ResourceAsset resourceAsset, Vector3 pickedObjPos)
        {
            //wait one frame to fix bug with incorrect position
            yield return null;
            
            Vector3 spawnPos = ResourceCanvas.Instance.GetUIPosition(pickedObjPos);
            var item = Instantiate(
                _pickedResPrefab, spawnPos, 
                Quaternion.identity, 
                ResourceCanvas.Instance.transform);
            
            item.Init(resourceAsset.Icon);
            yield return item.MoveResTo(_resIcon.transform.position);
            Destroy(item.gameObject);
            UpdateCount();
        }
        
        protected void CheckUIState()
        {
            var inventory = SinglePlayer.Instance.GetComponent<ResourcesInventory>();
            
            // if (inventory.Items == null)
            //     return;
            
            if (IsDisplayMaxCount)
            {
                var count = inventory.GetItemsByGroup(Asset.Group).Sum(i => i.Amount);
                var maxCount = Asset.Group.LimitCount;

                if(count > 0)
                {
                    if (Root.IsActive == false)
                        Root.Show(() => { });
                }
                else
                {
                    if(Root.IsActive)
                        Root.Hide(() => { });
                }

                return;
            }

            var item = inventory.Items.FirstOrDefault(r => r.Resource == Asset);

            if (item == null)
            {
                if(Root.IsActive)
                    Root.Hide(() => { });
                
                return;
            }

            if (Root.IsActive == false)
                Root.Show(() => { });

            if (item is LimitedItem limitedItem && item.Resource.UseItemGroup == false)
            {
                Text.text = string.Format(LimitedResourceFormat, limitedItem.Amount, limitedItem.Limit);
                return;
            }

            Text.text = item.Amount.ToString();
        }

        protected void UpdateCount()
        {
            var inventory = SinglePlayer.Instance.GetComponent<ResourcesInventory>();
            if(inventory.Items == null)
                return;
            
            if (IsDisplayMaxCount)
            {
                var count = inventory.GetItemsByGroup(Asset.Group).Sum(i => i.Amount);
                var maxCount = Asset.Group.LimitCount;
                Text.text = string.Format(LimitedResourceFormat, count, maxCount);

                // CheckErrorInSquad();
                
                return;
            }
            
            var item = inventory.Items.FirstOrDefault(r => r.Resource == Asset);
            
            if(item == null)
                return;
            
            if (item is LimitedItem limitedItem && item.Resource.UseItemGroup == false)
            {
                Text.text = string.Format(LimitedResourceFormat, limitedItem.Amount, limitedItem.Limit);
                return;
            }
            
            Text.text = item.Amount.ToString();
        }

        private void CheckErrorInSquad(){
            StartCoroutine(DeferredCheck());
            
        }

        private IEnumerator DeferredCheck(){
            yield return new WaitForSeconds(1);
            var unitsInUi = SinglePlayer.Instance.GetComponent<ResourcesInventory>().GetItemsByGroup(Asset.Group).ToList();
            var unitsInScene = SinglePlayer.Instance.GetComponent<UnitSquad>().Units;

            foreach (var item in unitsInUi){
                if (item.Resource == Asset){
                    Debug.Log($"{item.Amount}");
                    
                    
                    
                    if (unitsInScene.Count > item.Amount){
                        var counts = unitsInScene.Count - item.Amount;
                        var listForDestroy = new List<CollectingUnit>();
                        while (counts > 0){
                            listForDestroy.Add(unitsInScene[unitsInScene.Count - (counts-1)]);
                            counts--;
                        }

                        for (int i = 0; i < listForDestroy.Count; i++){
                            Destroy(listForDestroy[i].gameObject);
                        }
                    }
                }
            }
            Debug.Log($"{unitsInScene.Count} | {unitsInUi.Count}");
            yield break;

        }
    }
}