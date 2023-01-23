using System.Collections.Generic;
using NaughtyAttributes;
using NavySpade.Core.Runtime.Player.Logic;
using NavySpade.pj49.Scripts.Items.Inventory;
using NavySpade.pj49.Scripts.Productions.Factory;
using NavySpade.pj49.Scripts.Tutorial;
using NavySpade.pj49.Scripts.UnitsQueues;
using NavySpade.pj49.Scripts.UnitsQueues.Positions;
using UnityEngine;

namespace NavySpade.pj49.Scripts.Items
{
    public class PickableUnitsResource : PickupableResource
    {
        [SerializeField] private Factory _factory;
        [SerializeField] private PointsHolder _pointsHolder;
        [SerializeField] private UnitQueue _unitQueue;
        
        private void Start()
        {
            _factory.Stats.Upgraded += OnStatsUpgraded;
            Source.ResourcesCountChanged += ResourceCountChanged;
            UpdateZone(-1);
            SpawnUnits();
        }

        private void OnStatsUpgraded(FactoryUpgradeParameter parameter, int level)
        {
            if (_factory.Config.OutputMax == parameter)
            {
                UpdateZone(level);
            }
        }

        private void OnDestroy()
        {
            _factory.Stats.Upgraded -= OnStatsUpgraded;
            Source.ResourcesCountChanged -= ResourceCountChanged;
        }
        
        private void UpdateZone(int level)
        {
            var param = _factory.Config.OutputMax;
            _pointsHolder.ChangeSize(_factory.Stats.GetIntValueOf(param));
        }
        
        private void SpawnUnits()
        {
            Item item = _factory.OutputInventory.GetItem(ResourceAsset);
            for (int i = 0; i < item.Amount; i++)
            { 
                var unit = Instantiate(item.Resource.Prefab, UnitsManager.Instance.transform);
                _unitQueue.AddUnitToQueueInstant(unit.GetComponent<CollectingUnit>());
            }
        }
        
        private void ResourceCountChanged()
        {
            if (Source.Count(ResourceAsset) < _unitQueue.TotalUnits)
            {
                var unit = _unitQueue.GetFirstInQueue();
                SinglePlayer.Instance.AddUnitToSquad(unit);
                _unitQueue.RemoveFromQueue(unit);
                TutorialController.InvokeAction(TutorAction.GetStickman);
            }
        }
    }
}