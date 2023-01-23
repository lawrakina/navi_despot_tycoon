using NavySpade.Core.Runtime.Levels.Generation.Runner;
using NavySpade.Core.Runtime.Levels.Generation.Segments;
using UnityEngine;

namespace NavySpade.Core.Runtime.Levels.Data
{
    [CreateAssetMenu(fileName = "New Level", menuName = "Game/Level/Legacy Runner", order = 51)]
    public class LegacyRunnerLevelData : LevelDataBase
    {
        [Min(0f)] [SerializeField] private float _distance = 50f;
        [SerializeField] private LevelSegmentsData _segments;
        
        public float Distance => _distance;

        public LevelSegmentsData Segments => _segments;
    }
}