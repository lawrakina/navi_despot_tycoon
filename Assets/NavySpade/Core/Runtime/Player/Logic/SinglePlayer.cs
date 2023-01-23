using System;
using System.Collections.Generic;
using System.Linq;
using Core.Damagables;
using EventSystem.Runtime.Core.Managers;
using NavySpade.Modules.Saving.Runtime;
using NavySpade.pj49.Scripts;
using NavySpade.pj49.Scripts.Items;
using NavySpade.pj49.Scripts.Items.Inventory;
using NavySpade.pj49.Scripts.Productions.Factory;
using NavySpade.pj49.Scripts.Saving;
using NavySpade.pj49.Scripts.UnitsQueues;
using UnityEngine;

namespace NavySpade.Core.Runtime.Player.Logic
{
    /// <summary>
    /// класс-метка игрока.
    /// лучше всего не менять этот класс, а сразу декомпозировать логику для последующего реюза
    /// </summary>
    public class SinglePlayer : MonoBehaviour, ISaveable
    {
        [SerializeField] private UnitSquad _unitSquad;
        [SerializeField] private UnitsCapacityHandler _capacityHandler;
        
        [field: SerializeField] public Damageable Damageable { get; private set; }
        
        [field: SerializeField] public ResourcesInventory Inventory { get; private set; }

        public UnitsCapacityHandler CapacityHandler => _capacityHandler;
        
        public static SinglePlayer Instance { get; private set; }
        
        public void Init()
        {
            LevelSaver.Instance.Register(this);
            PlayerSavingData saveablePlayer = LevelSaver.LoadPlayer();
            Inventory.Init(saveablePlayer.Items);
            _capacityHandler.Init(saveablePlayer.HumanGroupCapacityLevel);
            SpawnUnitsInSquad();
            
            EventManager.Invoke(GenerateEnumEM.SetPlayer, Instance);
        }

        private void OnEnable()
        {
            Instance = this;
            Damageable.OnDeath += DamageableOnOnDeath;
        }
        
        private void DamageableOnOnDeath()
        {
            GetComponent<Rigidbody>().isKinematic = true;
            EventManager.Invoke(GameStatesEM.OnFail);
        }

        public IUnitMovementQueue GetQueuePattern()
        {
            var queues = GetComponents<IUnitMovementQueue>();
            var queue = queues.FirstOrDefault(c => c.IsEnable);
            return queue;
        }

        private void SpawnUnitsInSquad()
        {
            Item[] unitsRes = Inventory.GetItemsByGroup(_unitSquad.Group);
            foreach (var res in unitsRes)
            {
                for (int i = 0; i < res.Amount; i++)
                {
                    var unit = Instantiate(res.Resource.Prefab, transform.position, Quaternion.identity, UnitsManager.Instance.transform);
                    AddUnitToSquad(unit.GetComponent<CollectingUnit>());
                }
            }
        }

        public void AddUnitToSquad(CollectingUnit unit)
        {
            unit.TryConnectToSquad(_unitSquad, GetQueuePattern(), false);
        }

        public void Save()
        {
            PlayerSavingData savingData = new PlayerSavingData();
            SaveableInventory sv = Inventory.GetSaveableInventory();
            savingData.Items = sv;
            savingData.HumanGroupCapacityLevel = _capacityHandler.Level;
            LevelSaver.SavePlayer(savingData);
        }
    }
}