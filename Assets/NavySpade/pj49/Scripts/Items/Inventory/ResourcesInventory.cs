using System;
using System.Collections.Generic;
using System.Linq;
using NavySpade.Modules.Extensions.UnityTypes;
using UnityEngine;


namespace NavySpade.pj49.Scripts.Items.Inventory{
    public class ResourcesInventory : ExtendedMonoBehavior<ResourcesInventory>{
        [SerializeField]
        private bool _useGroupLimit;

        public event Action ResourcesCountChanged;
        public event Action<ResourceAsset, Vector3> ResourcePicked;
        public event Action<ResourceAsset> ResourceSpend;

        public List<Item> Items{ get; private set; }

        public int ItemsCount => Items.Sum(i => i.Amount);

        protected override void Awake(){
            base.Awake();
        }

        private void Init(){
            Items = new List<Item>();
        }

        public void Init(SaveableInventory saveableInventory){
            if (saveableInventory != null && saveableInventory.Items != null){
                Items = saveableInventory.Items;
            } else{
                Init();
            }

            ResourcesCountChanged?.Invoke();
        }

        public Item GetItem(ResourceAsset asset){
            foreach (var item in Items){
                if (item.Resource == asset)
                    return item;
            }

            return new Item(asset, 0);
        }

        public Item[] GetItemsByGroup(ResourceGroupAsset group){
            var list = new List<Item>();

            for (int i = 0; i < Items.Count; i++){
                var item = Items[i];

                if (group == item.Resource.Group && item.Resource.UseItemGroup)
                    list.Add(item);
            }

            return list.ToArray();
        }

        public int CountResource(TypeResources resources){
            foreach (var item in Items){
                if (item.Resource.TypeResources == resources)
                    return item.Amount;
            }

            return 0;
        }

        private int CountItems(IEnumerable<Item> items){
            var itemsCount = 0;

            foreach (var item in items){
                itemsCount += item.Amount;
            }

            return itemsCount;
        }

        public bool Contains(Item item){
            foreach (var i in Items){
                if (i.Resource != item.Resource)
                    continue;

                if (i.Amount >= item.Amount)
                    return true;

                return false;
            }

            return false;
        }

        public bool TryAddResource(Item item){
            return TryAddResource(item, out _);
        }

        public bool TryAddResource(Item item, out int addedCount, Vector3 pos){
            if (TryAddResource(item, out addedCount)){
                ResourcePicked?.Invoke(item.Resource, pos);
                return true;
            }

            return false;
        }

        public bool TryAddResource(Item item, out int addedCount){
            for (int i = 0; i < Items.Count; i++){
                if (Items[i].Resource != item.Resource)
                    continue;

                var count = 0;
                if (_useGroupLimit && item.Resource.Group != null && item.Resource.UseItemGroup){
                    var allItemsCount = CountItems(GetItemsByGroup(item.Resource.Group));
                    var maxAddCount = item.Resource.Group.LimitCount - allItemsCount;

                    count = Mathf.Clamp(item.Amount, 0, maxAddCount);
                } else{
                    count = item.Amount;
                }

                Items[i].TryAdd(ref count);
                addedCount = count;

                if (count <= 0){
                    return false;
                }

                ResourcesCountChanged?.Invoke();
                return true;
            }

            if (_useGroupLimit && item.Resource.Group != null && item.Resource.UseItemGroup){
                var allItemsCount = CountItems(GetItemsByGroup(item.Resource.Group));
                var maxAddCount = item.Resource.Group.LimitCount - allItemsCount;

                var count = Mathf.Clamp(item.Amount, 0, maxAddCount);
                addedCount = count;

                if (count <= 0)
                    return false;

                item.Amount = count;
                Items.Add(item);
                ResourcesCountChanged?.Invoke();
                return true;
            }

            addedCount = item.Amount;
            Items.Add(item);
            ResourcesCountChanged?.Invoke();
            return true;
        }

        public bool TryRemoveResource(Item item){
            for (var i = 0; i < Items.Count; i++){
                if (Items[i].Resource != item.Resource)
                    continue;

                Items[i].Take(item.Amount);

                ResourceSpend?.Invoke(Items[i].Resource);

                if (Items[i].Amount <= 0)
                    Items.RemoveAt(i);

                ResourcesCountChanged?.Invoke();
                return true;
            }

            return false;
        }

        public bool TryRemoveResource(ResourceGroupAsset group, int count){
            int left = count;
            for (var i = 0; i < Items.Count; i++){
                if (Items[i].Resource.Group != group)
                    continue;

                int takeCount = left;
                if (Items[i].Amount < left){
                    takeCount = Items[i].Amount;
                }

                Items[i].Take(takeCount);
                if (Items[i].Amount <= 0)
                    Items.RemoveAt(i);

                ResourcesCountChanged?.Invoke();
                ResourceSpend?.Invoke(Items[i].Resource);

                left -= takeCount;
                if (left <= 0){
                    return true;
                }
            }

            return false;
        }

        public bool IsContainsItems(ResourceAsset res){
            foreach (var i in Items){
                if (i.Resource == res)
                    return true;
            }

            return false;
        }

        public int Count(ResourceGroupAsset group){
            var items = GetItemsByGroup(group);
            return CountItems(items);
        }

        public int Count(ResourceAsset resource){
            if (Items == null) return 0;
            Item itemInInventory = Items.FirstOrDefault(i => i.Resource == resource);
            int itemCount = itemInInventory?.Amount ?? 0;
            return itemCount;
        }

        public SaveableInventory GetSaveableInventory(){
            return ResourceUtility.ConvertToSavableList(Items);
        }
    }
}