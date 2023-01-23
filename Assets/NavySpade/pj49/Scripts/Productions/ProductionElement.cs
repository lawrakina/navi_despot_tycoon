using System.Collections;
using System.Linq;
using NavySpade.pj49.Scripts.Items;
using NavySpade.pj49.Scripts.Items.Inventory;
using UnityEngine;

namespace NavySpade.pj49.Scripts.Productions
{
    public abstract class ProductionElement : ItemsReceiver
    {
        public float UnitLifeTime;

        [SerializeField] private float _tryStartWorkTime;

        [field: SerializeField] public ResourceAsset UnitsResourceAsset { get; private set; }

        [field: SerializeField] public int RequestingUnitsCountToStartWork { get; private set; }

        [field: SerializeField] public int UnitsContainedLimit { get; private set; }

        [field: SerializeField] public ResourcesInventory Inventory { get; private set; }


        public Item RequestingItemToStartWork => UnitsResourceAsset.CreateItem(RequestingUnitsCountToStartWork);

        public bool IsWork { get; private set; }

        public Animator Animator;
        private static readonly int Work = Animator.StringToHash("Work");

        private Coroutine _workRoutine;

        protected virtual void Start()
        {
            if (Animator != null)
                Animator.SetBool(Work, false);

            StartCoroutine(TryWork());
        }

        public bool CanPickupItems()
        {
            var holdingItems = Inventory.Items.FirstOrDefault(i => i.Resource == UnitsResourceAsset);

            var inventoryItems = holdingItems == null ? 0 : holdingItems.Amount;
            var unitsCountWithWorkers = IsWork ? inventoryItems + RequestingUnitsCountToStartWork : inventoryItems;

            if (unitsCountWithWorkers >= UnitsContainedLimit)
                return false;

            return true;
        }

        public override void PickupItemsToThis(ResourcesInventory fromInventory)
        {
            if (CanPickupItems() == false)
                return;

            var holdingItems = Inventory.Items.FirstOrDefault(i => i.Resource == UnitsResourceAsset);
            var item = UnitsResourceAsset.CreateItem();
            if (fromInventory.TryRemoveResource(item))
            {
                if (holdingItems == null || holdingItems.Amount < UnitsContainedLimit)
                    Inventory.TryAddResource(item);
            }

            //TryStartWork();
        }

        private IEnumerator TryWork()
        {
            while (true)
            {
                TryStartWork();
                yield return new WaitForSeconds(_tryStartWorkTime);
            }
        }

        protected bool TryStartWork()
        {
            if (IsWork)
                return false;

            if (IsContainResourcesForStart())
            {
                GetStartingResources();
                //StartWork();
                return true;
            }

            return false;
        }

        protected virtual bool IsContainResourcesForStart()
        {
            if (Inventory.Contains(RequestingItemToStartWork) == false)
                return false;

            return true;
        }

        protected virtual void GetStartingResources()
        {
            Inventory.TryRemoveResource(RequestingItemToStartWork);
        }

        public virtual void StartWork()
        {
            if (IsWork)
                return;

            if (Animator != null)
                Animator.SetBool(Work, true);

            _workRoutine = StartCoroutine(WorkRoutine());
            IsWork = true;
            InvokeAtTime(UnitLifeTime, WorkCheckEnding);
        }

        private void WorkCheckEnding()
        {
            if (IsContainResourcesForStart())
            {
                GetStartingResources();
                InvokeAtTime(UnitLifeTime, WorkCheckEnding);
            }
            else
            {
                EndWork();
            }

            OnCompleteSequence();
        }

        protected virtual void OnCompleteSequence()
        {
        }

        private IEnumerator WorkRoutine()
        {
            while (true)
            {
                UpdateOnWork();
                yield return null;
            }
        }

        public virtual void EndWork()
        {
            if (IsWork == false)
                return;

            if (Animator != null)
                Animator.SetBool(Work, false);

            StopCoroutine(_workRoutine);
            IsWork = false;
        }

        public abstract void UpdateOnWork();
    }
}