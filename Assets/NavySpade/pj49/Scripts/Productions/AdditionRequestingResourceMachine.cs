using System;
using NavySpade.Core.Runtime.Player.Logic;
using NavySpade.pj49.Scripts.Items;
using NavySpade.pj49.Scripts.Items.Inventory;
using UnityEngine;

namespace NavySpade.pj49.Scripts.Productions
{
    public class AdditionRequestingResourceMachine : ProductionElement
    {
        [Serializable]
        class Request
        {
            public ResourceAsset AdditionRequestingResource;
            public int RequestingCount;

            public Item GetItem => AdditionRequestingResource.CreateItem(RequestingCount);
        }

        [SerializeField] private Request[] _additionResourceToStart;


        private bool _updateBlocker;

        protected override void Start()
        {
            base.Start();
            
            var playerInventory = SinglePlayer.Instance.GetComponent<ResourcesInventory>();
            
            playerInventory.ResourcesCountChanged += PlayerInventoryOnResourcesCountChanged;
        }

        private void PlayerInventoryOnResourcesCountChanged()
        {
            if(_updateBlocker || IsWork)
                return;
            
            _updateBlocker = true;

            TryStartWork();

            _updateBlocker = false;
        }

        protected override bool IsContainResourcesForStart()
        {
            var playerInventory = SinglePlayer.Instance.GetComponent<ResourcesInventory>();
            
            foreach (var request in _additionResourceToStart)
            {
                if (playerInventory.Contains(request.GetItem) == false)
                    return false;
            }
            
            return base.IsContainResourcesForStart();
        }

        protected override void GetStartingResources()
        {
            var playerInventory = SinglePlayer.Instance.GetComponent<ResourcesInventory>();
            
            foreach (var request in _additionResourceToStart)
            {
                playerInventory.TryRemoveResource(request.GetItem);
            }
            
            base.GetStartingResources();
        }
        
        [field: Header("Result Of Work")] 
        [field: SerializeField] public float DelayToGetWorkResult { get; private set; }
        [field: SerializeField] public ResourceAsset RewardItem { get; private set; }
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
                Inventory.TryAddResource(RewardItem.CreateItem(RewardItemCountPerResult));

                _timer.Reload();
            }
            
            _timer.Update(Time.deltaTime);
        }
    }
}