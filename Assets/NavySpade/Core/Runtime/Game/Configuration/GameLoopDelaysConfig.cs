using NavySpade.Modules.Configuration.Runtime.SO;
using UnityEngine;

namespace Core.Game.Configuration
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Config/Game", order = 51)]
    public class GameLoopDelaysConfig : ObjectConfig<GameLoopDelaysConfig>
    {
        [field: Min(0f)]
        [field: SerializeField]
        public float AfterWin { get; private set; } = 3f;

        [field: Min(0f)]
        [field: SerializeField]
        public float AfterLose { get; private set; } = 3f;
    }
}