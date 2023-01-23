using NavySpade.Core.Runtime.Levels.Data;
using NavySpade.Core.Runtime.Levels.Generation.Abstract;
using NavySpade.pj49.Scripts;
using UnityEngine;

namespace NavySpade.Core.Runtime.Levels.Generation.Prefab
{
    public class SimpleLevelGenerator : LevelGenerator<PrefabLevelData>
    {
        private Level _level;

        protected override void StartGeneration(PrefabLevelData levelData)
        {
            _level = Instantiate(levelData.Prefab).GetComponent<Level>();
            _level.LevelIndex = LevelManager.LevelIndex;
        }

        protected override void OnCleanUp()
        {
            LevelSaver.ResetSaves(_level.LevelIndex, _level.FactoriesCount); 
            Destroy(_level.gameObject);
        }
    }
}