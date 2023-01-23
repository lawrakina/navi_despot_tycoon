using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using NavySpade.pj49.Scripts.Items.Inventory;
using UnityEngine;

namespace NavySpade.pj49.Scripts.Items
{
    public class ItemStackDisplay : MonoBehaviour
    {
        [Serializable]
        class ItemData
        {
            public ResourceAsset Asset;
            public StackItem ObjectPrefab;
        }

        public class Spawned
        {
            public ResourceAsset Asset;
            public StackItem Instance;
        }
        
        public ResourceGroupAsset GroupAsset;
        public ResourcesInventory Inventory;

        public Vector3 SpawnAxis = Vector3.up;
        public Vector3 RemoveAxis = Vector3.up;
        
        [SerializeField] private ItemData[] _datas;
        [SerializeField] private Transform _startSpawnPoint;

        public List<Spawned> SpawnedDatas { get; private set; }

        private void Awake()
        {
            SpawnedDatas = new List<Spawned>();
        }

        private void Start()
        {
            Inventory.ResourcesCountChanged += InventoryOnResourcesCountChanged;
        }

        private void InventoryOnResourcesCountChanged()
        {
            UpdateStack();
        }

        private void UpdateStack()
        {
            var items = Inventory.GetItemsByGroup(GroupAsset);

            foreach (var item in items)
            {
                var count = SpawnedDatas.Count(c => c.Asset == item.Resource);
                var changeCount = item.Amount - count;

                while (changeCount != 0)
                {
                    var data = _datas.FirstOrDefault(c => c.Asset == item.Resource);
                    
                    if(data == null)
                        break;
                    
                    if (changeCount > 0)
                    {
                        var newItem = AddItem(
                            data,
                            SpawnedDatas.Count > 0 ? SpawnedDatas[SpawnedDatas.Count - 1] : null);
                        
                        SpawnedDatas.Add(newItem);
                        
                        changeCount--;
                    }

                    if (changeCount < 0)
                    {
                        var spawned = SpawnedDatas.First(c => c.Asset == item.Resource);
                        var index = SpawnedDatas.IndexOf(spawned);
                        
                        RemoveItem(spawned, index);
                        
                        changeCount++;
                    }
                }
            }

            for (int i = 0; i < SpawnedDatas.Count; i++)
            {
                var currentData = SpawnedDatas[i];
                
                if(items.Count(i => i.Resource == currentData.Asset) > 0)
                    continue;
                
                RemoveItem(currentData, i);
                i--;
            }
        }

        private Spawned AddItem(ItemData data, [CanBeNull] Spawned topItem)
        {
            var instance = Instantiate(data.ObjectPrefab, transform, true);
            instance.transform.forward = transform.forward;

            if (topItem == null)
            {
                instance.transform.position = _startSpawnPoint.position;
            }
            else
            {
                instance.transform.position =
                    topItem.Instance.transform.position + SpawnAxis * topItem.Instance.Height;
            }

            return new Spawned
            {
                Asset = data.Asset,
                Instance = instance
            };
        }

        private void RemoveItem(Spawned spawned, int indexInSpawns)
        {
            SpawnedDatas.RemoveAt(indexInSpawns);

            for (var i = indexInSpawns; i < SpawnedDatas.Count; i++)
            {
                SpawnedDatas[i].Instance.transform.localPosition -= RemoveAxis * spawned.Instance.Height;
            }
            
            Destroy(spawned.Instance.gameObject);
        }
        
    }
}