using NavySpade.Modules.Configuration.Runtime.SO;
using UnityEngine;

namespace NavySpade.Commands.Configuration
{
    public class ConsoleConfig : ObjectConfig<ConsoleConfig>
    {
        [SerializeField] private bool _enableDebugConsole = false;
        [SerializeField] private bool _isAutoEnableIfDebugBuild = true;

        public bool EnableDebugConsole =>
            (_isAutoEnableIfDebugBuild && Debug.isDebugBuild && Application.isEditor == false) || _enableDebugConsole;
    }
}