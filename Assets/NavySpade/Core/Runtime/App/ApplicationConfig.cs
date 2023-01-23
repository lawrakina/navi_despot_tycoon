using NavySpade.Modules.Configuration.Runtime.Attributes;
using NavySpade.Modules.Configuration.Runtime.SO;
using UnityEngine;

namespace NavySpade.Core.Runtime.App
{
    [Config("App", 0)]
    public class ApplicationConfig : ObjectConfig<ApplicationConfig>
    {
        [field: Min(0), SerializeField] public int TargetFrameRate { get; private set; } = 60;
    }
}