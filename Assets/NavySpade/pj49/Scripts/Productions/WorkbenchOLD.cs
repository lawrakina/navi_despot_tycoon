using System;
using System.Collections;
using NavySpade.Core.Runtime.Player.Logic;
using NavySpade.pj49.Scripts.Items;
using NavySpade.pj49.Scripts.Items.Inventory;
using UnityEngine;
using UnityEngine.Events;

namespace NavySpade.pj49.Scripts.Productions
{
    /*
——————————No Clean Code?—————————————
⠀⣞⢽⢪⢣⢣⢣⢫⡺⡵⣝⡮⣗⢷⢽⢽⢽⣮⡷⡽⣜⣜⢮⢺⣜⢷⢽⢝⡽⣝
⠸⡸⠜⠕⠕⠁⢁⢇⢏⢽⢺⣪⡳⡝⣎⣏⢯⢞⡿⣟⣷⣳⢯⡷⣽⢽⢯⣳⣫⠇
⠀⠀⢀⢀⢄⢬⢪⡪⡎⣆⡈⠚⠜⠕⠇⠗⠝⢕⢯⢫⣞⣯⣿⣻⡽⣏⢗⣗⠏⠀
⠀⠪⡪⡪⣪⢪⢺⢸⢢⢓⢆⢤⢀⠀⠀⠀⠀⠈⢊⢞⡾⣿⡯⣏⢮⠷⠁⠀⠀
⠀⠀⠀⠈⠊⠆⡃⠕⢕⢇⢇⢇⢇⢇⢏⢎⢎⢆⢄⠀⢑⣽⣿⢝⠲⠉⠀⠀⠀⠀
⠀⠀⠀⠀⠀⡿⠂⠠⠀⡇⢇⠕⢈⣀⠀⠁⠡⠣⡣⡫⣂⣿⠯⢪⠰⠂⠀⠀⠀⠀
⠀⠀⠀⠀⡦⡙⡂⢀⢤⢣⠣⡈⣾⡃⠠⠄⠀⡄⢱⣌⣶⢏⢊⠂⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⢝⡲⣜⡮⡏⢎⢌⢂⠙⠢⠐⢀⢘⢵⣽⣿⡿⠁⠁⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠨⣺⡺⡕⡕⡱⡑⡆⡕⡅⡕⡜⡼⢽⡻⠏⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⣼⣳⣫⣾⣵⣗⡵⡱⡡⢣⢑⢕⢜⢕⡝⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⣴⣿⣾⣿⣿⣿⡿⡽⡑⢌⠪⡢⡣⣣⡟⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⡟⡾⣿⢿⢿⢵⣽⣾⣼⣘⢸⢸⣞⡟⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠁⠇⠡⠩⡫⢿⣝⡻⡮⣒⢽⠋⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
———————————————————————————
     */
    [Obsolete]
    public class WorkbenchOLD : ProductionElement
    {
        [Serializable]
        class Callbacks
        {
            public UnityEvent UnitAdded;
            public UnityEvent UnitStartWork;
            public UnityEvent UnitStopWork;
            public UnityEvent UnitDestroy;
        }
        
        public ResourceAsset RequestingHuman;
        public float HumanAlliveTime;
        
        public float DelayToSpawnProduct;
        public ResourceAsset RewardedProduct;
        public ResourcesInventory BoxInventory;
        public PickupableResource RewardResource;
        public int ResourceLimitCount;

        [SerializeField] private Callbacks _events;

        private Timer _timer;
        private Timer _unitLife;
        private Coroutine _unitUpdateLifeTime;
        private bool _isUnitWork;
        private bool _isUnitAlive;
        private bool _isSkipAutoStartCall;

        private bool IsUnitAlive
        {
            get => _isUnitAlive;
            set
            {
                _isUnitAlive = value;

                if (value)
                {
                    _unitLife ??= new Timer(HumanAlliveTime);
                    _unitLife.Reload();
                    
                    _events.UnitAdded.Invoke();
                }
                else
                {
                    _events.UnitDestroy.Invoke();
                }
            }
        }

        private bool IsUnitWork
        {
            get => _isUnitWork;
            set
            {
                _isUnitWork = value;

                if (value)
                {
                    _events.UnitStartWork.Invoke();
                    _unitUpdateLifeTime = StartCoroutine(UnitUpdateLifeRoutine());
                }
                else
                {
                    _events.UnitStopWork.Invoke();
                    
                    if(_unitUpdateLifeTime != null)
                        StopCoroutine(_unitUpdateLifeTime);
                }

                IEnumerator UnitUpdateLifeRoutine()
                {
                    while (_unitLife.IsFinish() == false)
                    {
                        yield return null;
                        _unitLife.Update(Time.deltaTime);
                    }
                    
                    DeathUnit();
                }
            }
        }

        private void Awake()
        {
            _timer = new Timer(DelayToSpawnProduct);
        }

        protected override void Start()
        {
            base.Start();
            var inventory = SinglePlayer.Instance.GetComponent<ResourcesInventory>();
            inventory.ResourcesCountChanged += CheckPowerAmountForAutoStartWork;
        }

        protected void OnTriggerEnter(Collider other)
        {
            _isSkipAutoStartCall = true;
            if (IsUnitAlive == false)
            {
                var inventory = SinglePlayer.Instance.GetComponent<ResourcesInventory>();
                
                if(inventory.TryRemoveResource(RequestingHuman.CreateItem()) == false)
                {
                    _isSkipAutoStartCall = false;
                    return;
                }

                IsUnitAlive = true;
            }
            
            _isSkipAutoStartCall = false;
        }

        private void CheckPowerAmountForAutoStartWork()
        {
            var inventory = SinglePlayer.Instance.GetComponent<ResourcesInventory>();
            
            if(IsUnitAlive == false || IsWork || _isSkipAutoStartCall)
                return;

            _isSkipAutoStartCall = true;
            
            if (inventory.TryRemoveResource(UnitsResourceAsset.CreateItem(RequestingUnitsCountToStartWork)) == false)
            {
                _isSkipAutoStartCall = false;
                return;
            }
            _isSkipAutoStartCall = false;

            StartWork();
        }

        private void DeathUnit()
        {
            IsUnitAlive = false;
            IsUnitWork = false;
            EndWork();
        }

        public override void StartWork()
        {
            if(IsUnitAlive == false)
                return;
            
            if (IsUnitWork == false)
            {
                IsUnitWork = true;
            }
            
            base.StartWork();
        }

        public override void EndWork()
        {
            base.EndWork();
            
            var inventory = SinglePlayer.Instance.GetComponent<ResourcesInventory>();
            _isSkipAutoStartCall = true;
            if (inventory.TryRemoveResource(UnitsResourceAsset.CreateItem(RequestingUnitsCountToStartWork)))
            {
                _isSkipAutoStartCall = false;
                StartWork();
            }
            else
            {
                _isSkipAutoStartCall = false;
                IsUnitWork = false;
            }
        }

        public override void UpdateOnWork()
        {
            if (_timer.IsFinish())
            {
                if (BoxInventory.ItemsCount < ResourceLimitCount)
                {
                    BoxInventory.TryAddResource(RewardedProduct.CreateItem(), out var realAdded);

                    RewardResource.ResourceAsset = RewardedProduct;
                }
                
                _timer.Reload();
            }
            
            _timer.Update(Time.deltaTime);
        }
    }
}