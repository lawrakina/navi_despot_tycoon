using NaughtyAttributes;
using NavySpade.Modules.Configuration.Runtime.SO;
using UnityEngine;

namespace NavySpade.Core.Runtime.App
{
    public class LoadingConfig : ObjectConfig<LoadingConfig>
    {
        [field: SerializeField, Scene] public string GameSceneName { get; private set; }
    }
}