using UnityEngine;

namespace NavySpade.Core.Runtime.Levels.Data
{
    public class SceneLevelData : LevelDataBase
    {
        [field: SerializeField] public int BuildIndex { get; private set; }
    }
}