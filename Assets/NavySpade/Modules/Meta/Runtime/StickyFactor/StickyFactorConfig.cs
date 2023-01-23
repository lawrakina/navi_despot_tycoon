using NavySpade.Modules.Configuration.Runtime.Attributes;
using NavySpade.Modules.Configuration.Runtime.SO;
using UnityEngine;
using ShopItem = NavySpade.Meta.Runtime.Shop.Items.ShopItem;

namespace NavySpade.Modules.Meta.Runtime.StickyFactor
{
    [ConfigGroup(MetaConstants.ModuleName)]
    public class StickyFactorConfig : ObjectConfig<StickyFactorConfig>
    {
        [field: Min(0), SerializeField]
        public uint DelayBetweenNextRewardInSeconds { get; private set; } = 60 * 60 * 24;

        [field: SerializeField] public ShopItem Rewards { get; private set; }
    }
}