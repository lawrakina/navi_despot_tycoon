using NavySpade.Core.Runtime.Player.Logic;
using NavySpade.pj49.Scripts.Items;
using NavySpade.pj49.Scripts.Items.Inventory;
using UnityEngine;

namespace NavySpade.pj49.Scripts.Productions
{
    public class MineMachine : ProductionElement
    {
        [field: Header("Result Of Work")] 
        [field: SerializeField] public float DelayToGetWorkResult { get; private set; }
        [field: SerializeField] public ResourceAsset RewardItem { get; private set; }
        
        [field: Tooltip("если не заполнено то выбирает игрока")]
        [field: SerializeField] 
        public ResourcesInventory TargetInventory { get; private set; }
        
        [field: SerializeField] public int RewardItemCountPerResult { get; private set; }

        private Timer _timer;

        private void Awake()
        {
            _timer = new Timer(DelayToGetWorkResult);
        }

        public override void UpdateOnWork()
        {
            if (_timer.IsFinish())
            {
                var playerInventory = TargetInventory == null ? SinglePlayer.Instance.GetComponent<ResourcesInventory>() : TargetInventory;

                playerInventory.TryAddResource(RewardItem.CreateItem(RewardItemCountPerResult), out var addedCount);
                OnAddedResource(RewardItem, addedCount);
                
                _timer.Reload();
            }
            
            _timer.Update(Time.deltaTime);
        }

        protected virtual void OnAddedResource(ResourceAsset resource, int count)
        {
            
        }
    }
}