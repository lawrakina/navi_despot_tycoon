using NavySpade.Core.Runtime.Levels.Data;
using NavySpade.Modules.Configuration.Runtime.SO;
using UnityEngine;

namespace NavySpade.Core.Runtime.Levels.Configuration
{
    [CreateAssetMenu(fileName = "LevelsConfig", menuName = "Config/Levels", order = 51)]
    public class LevelsConfig : ObjectConfig<LevelsConfig>
    {
        [field: SerializeField] public LevelDataBase[] Tutorial { get; private set; }
        [field: SerializeField] public LevelDataBase[] Main { get; private set; }
    }
}