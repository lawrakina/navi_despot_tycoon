using System;
using UnityEngine;

namespace NavySpade.pj49.Scripts.Items.Inventory
{
    [Serializable]
    public class Item
    {
        [SerializeField] private int _resourceIndex;
        [SerializeField] private int _amount;

        public Item(ResourceAsset asset, int amount)
        { 
            _resourceIndex = ResourcesHolder.Instance.GetIndexOfResource(asset);
            _amount = amount;
        }

        public Item(int resourceIndex, int amount)
        {
            _resourceIndex = resourceIndex;
            _amount = amount;
        }

        public ResourceAsset Resource => ResourcesHolder.Instance.GetResource(_resourceIndex);

        public event Action AmountChanged;

        public virtual void TryAdd(ref int count)
        {
            Amount += count;
        }

        public virtual void Take(int count)
        {
            Amount -= count;
        }

        public int Amount
        {
            get => _amount; 
            set
            {
                _amount = value;
                
                AmountChanged?.Invoke();
            }
        }
    }

    [Serializable]
    public class LimitedItem : Item
    {
        public int Limit { get; }
        
        public LimitedItem(ResourceAsset resource, int amount, int limit) : base(resource, amount)
        {
            Limit = limit;
        }

        public override void TryAdd(ref int count)
        {
            if (Amount + count > Limit)
                count = Limit - Amount;
            
            base.TryAdd(ref count);
        }
    }
}