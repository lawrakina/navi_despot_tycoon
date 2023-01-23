using UnityEngine;

namespace NavySpade.Core.Runtime.Levels.Data
{
    [CreateAssetMenu(fileName = "New Level", menuName = "Game/Level/Prefab", order = 51)]
    public class PrefabLevelData : LevelDataBase
    {
        [field:SerializeField] public GameObject Prefab { get; private set; }
    }
}