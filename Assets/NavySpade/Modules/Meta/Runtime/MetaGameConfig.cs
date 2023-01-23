using NavySpade.Modules.Configuration.Runtime.SO;
using UnityEngine;

namespace Core.Meta
{
    public class MetaGameConfig : ObjectConfig<MetaGameConfig>
    {
        [field: SerializeField] public bool EnableMetaGame { get; private set; } = true;

        [SerializeField] private bool _enableQuests = true;
        [SerializeField] private bool _enableStickyFactor = true;
        [SerializeField] private bool _enableSkinsShop = true;
        
        public bool EnableQuests => EnableMetaGame && _enableQuests;
        public bool EnableStickyFactor => EnableMetaGame && _enableStickyFactor;
        public bool EnableSkinsShop => EnableMetaGame && _enableSkinsShop;
    }
}