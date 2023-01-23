using System.Collections.Generic;
using NavySpade.Core.Runtime.Levels;
using NavySpade.Modules.Utils.Singletons.Runtime.Unity;
using NavySpade.pj49.Scripts.Items;
using NavySpade.pj49.Scripts.Productions.Arena;
using NavySpade.pj49.Scripts.Saving;
using SaveManager = NavySpade.Modules.Saving.Runtime.SaveManager;

namespace NavySpade.pj49.Scripts
{
    public class LevelSaver : MonoSingleton<LevelSaver>
    {
        private const string SAVE_LEVEL_PREFIX = "LEVEL_";
        private const string SAVE_BUILDING_PREFIX = "_BUILDING_";
        private const string SAVE_ARENA_PREFIX = "ARENA_";
        private const string SAVE_MARKET = "MARKET";
        private const string SAVE_PLAYER = "PLAYER";
        private const string SAVE_WONDER = "WONDER";

        private List<ISaveable> _saveable = new List<ISaveable>();
        private const string SAVE_ARENA = "ARENA";

        public static FactorySavingData LoadBuildSaving(int buildIndex)
        {
            string path = SAVE_LEVEL_PREFIX + LevelManager.LevelIndex + 
                          SAVE_BUILDING_PREFIX + buildIndex;
            
            return SaveManager.Load(path, new FactorySavingData());
        }

        public static void SaveBuildSaving(int buildIndex, FactorySavingData factorySavingData)
        {
            string path = SAVE_LEVEL_PREFIX + LevelManager.LevelIndex + 
                          SAVE_BUILDING_PREFIX + buildIndex;
            
            SaveManager.Save(path, factorySavingData);
        }

        public static ArenaSavingData LoadArenaSaving(){
            var path = SAVE_ARENA_PREFIX + LevelManager.LevelIndex + SAVE_ARENA;
            return SaveManager.Load(path, new ArenaSavingData());
        }

        public static SaveableInventory LoadMarketSaving()
        {
            string path = SAVE_LEVEL_PREFIX + LevelManager.LevelIndex +
                          SAVE_MARKET;
            
            return SaveManager.Load(path, new SaveableInventory());
        }

        public static void SaveMarket(SaveableInventory saveableInventory)
        {
            string path = SAVE_LEVEL_PREFIX + LevelManager.LevelIndex +
                          SAVE_MARKET;
            
            SaveManager.Save(path, saveableInventory);
        }

        public static PlayerSavingData LoadPlayer()
        {
            string path = SAVE_LEVEL_PREFIX + LevelManager.LevelIndex +
                          SAVE_PLAYER;
            
            return SaveManager.Load(path, new PlayerSavingData());
        }

        public static void SavePlayer(PlayerSavingData savingPlayer)
        {
            string path = SAVE_LEVEL_PREFIX + LevelManager.LevelIndex +
                          SAVE_PLAYER;
            
            SaveManager.Save(path, savingPlayer);
        }

        public static WonderSavingData LoadWonderSaving()
        {
            string path = SAVE_LEVEL_PREFIX + LevelManager.LevelIndex +
                          SAVE_WONDER;
            
            return SaveManager.Load(path, new WonderSavingData());
        }

        public static void SaveArena(ArenaSavingData data){
            string path = SAVE_ARENA_PREFIX + LevelManager.LevelIndex + SAVE_ARENA;
            SaveManager.Save(path, data);
        }

        public static void SaveWonder(WonderSavingData wonderSavingData)
        {
            string path = SAVE_LEVEL_PREFIX + LevelManager.LevelIndex +
                          SAVE_WONDER;
            
            SaveManager.Save(path, wonderSavingData);
        }

        public static void ResetSaves(int levelIndex, int totalBuildings)
        {
            for (int i = 0; i < totalBuildings; i++)
            {
                SaveManager.DeleteKey(SAVE_LEVEL_PREFIX + levelIndex + SAVE_BUILDING_PREFIX + i);
            }
            SaveManager.DeleteKey(SAVE_LEVEL_PREFIX + levelIndex + SAVE_MARKET);
            SaveManager.DeleteKey(SAVE_LEVEL_PREFIX + levelIndex + SAVE_PLAYER);
            SaveManager.DeleteKey(SAVE_LEVEL_PREFIX + levelIndex + SAVE_WONDER);
        }

        protected override void OnDestroyInternal()
        {
            base.OnDestroyInternal();
            SaveAll();
        }

        public void Register(ISaveable saveable)
        {
            _saveable.Add(saveable);
        }

        protected override void OnApplicationQuit()
        {
            base.OnApplicationQuit();
            SaveAll();
        }

        private void OnApplicationPause(bool pause)
        {
            if (pause)
                SaveAll();
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if(!hasFocus)
                SaveAll();
        }

        private void SaveAll()
        {
            foreach (var saveable in _saveable)
            {
                saveable.Save();
            }
        }
    }
}