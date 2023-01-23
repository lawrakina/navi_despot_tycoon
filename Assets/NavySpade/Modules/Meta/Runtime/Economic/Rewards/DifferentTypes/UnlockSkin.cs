using System;
using Core.Meta.Skins;
using NavySpade.Meta.Runtime.Economic.Rewards.Interfaces;
using NavySpade.Meta.Runtime.Skins.Configuration;
using NavySpade.Modules.Meta.Runtime.Skins.Configuration;
using UnityEngine;

namespace NavySpade.Meta.Runtime.Economic.Rewards.DifferentTypes
{
    [Serializable]
    [AddTypeMenu("Unlock Skin")]
    public class UnlockSkin : IReward
    {
        [field: SerializeField] public SkinData SkinData { get; private set; }

        public void TakeReward()
        {
            var config = SkinsConfig.Instance;
            var index = config.GetSkinIndex(SkinData);

            SkinsManager.OpenSkin(index);
        }

        public bool IsLocked()
        {
            var config = SkinsConfig.Instance;
            var index = config.GetSkinIndex(SkinData);

            return SkinsManager.IsSkinOpen(index);
        }
    }
}