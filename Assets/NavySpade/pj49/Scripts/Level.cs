using System;
using System.Collections;
using NavySpade.Core.Runtime.Levels;
using NavySpade.Core.Runtime.Player.Logic;
using NavySpade.Modules.Utils.Singletons.Runtime.Unity;
using NavySpade.pj49.Scripts.Items.Inventory;
using NavySpade.pj49.Scripts.Productions;
using NavySpade.pj49.Scripts.Productions.Arena;
using NavySpade.pj49.Scripts.Productions.Factory;
using NavySpade.pj49.Scripts.Productions.ProductionStates;
using UnityEngine;


namespace NavySpade.pj49.Scripts{
    public class Level : MonoBehaviour{
        [SerializeField]
        private Market _market;
        [SerializeField]
        private Wonder _wonder;
        [SerializeField]
        private SinglePlayer _player;
        [SerializeField]
        private ArenaLogic _arena;

        private Factory[] _factories;

        public int LevelIndex{ get; set; }
        public int FactoriesCount => _factories.Length;

        private void Awake(){
            _factories = GetComponentsInChildren<Factory>();
            _market = GetComponentInChildren<Market>();
            _arena = GetComponentInChildren<ArenaLogic>();
        }

        private void Start(){
            //yield return null;
            _player.Init();
            InitBuildings();
        }

        private void InitBuildings(){
            for (int i = 0; i < _factories.Length; i++){
                _factories[i].Init(i);
            }

            _arena?.Init();
            _market?.Init();
            _wonder?.Init();
        }
    }
}