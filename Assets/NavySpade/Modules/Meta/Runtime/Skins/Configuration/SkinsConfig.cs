using System;
using System.Collections.Generic;
using Core.Meta.Unlocks;
using NavySpade.Meta.Runtime.Analytics;
using NavySpade.Meta.Runtime.Skins.Configuration;
using NavySpade.Meta.Runtime.Unlocks;
using NavySpade.Modules.Configuration.Runtime.Attributes;
using NavySpade.Modules.Configuration.Runtime.SO;
using NavySpade.Modules.Visual.Runtime;
using NavySpade.Modules.Visual.Runtime.Data;
using UnityEngine;

namespace NavySpade.Modules.Meta.Runtime.Skins.Configuration
{
    [ConfigGroup(MetaConstants.ModuleName)]
    public class SkinsConfig : ObjectConfig<SkinsConfig>
    {
        [Serializable]
        public struct SkinInfo : IUnlockable
        {
            [field: SerializeField] public SkinDataByVisual[] SkinVariantsByVisual { get; private set; }

            private UnlockableItem _unlockableItem;

            public UnlockableItem UnlockableItem
            {
                get
                {
                    //     return _unlockableItem == null
                    //         ? _unlockableItem = new UnlockableItem(SavingKey, UnlockConditions, IsUnlockFromStart, IsEarnedFromStart, Reward.TakeReward)
                    //        : _unlockableItem;
                    return null;
                }
            }

            public IUnlockCondition[] UnlockConditions { get; }
            public string SavingKey { get; }
            public bool IsUnlockFromStart { get; set; }

            public bool TryUnlock()
            {
                throw new NotImplementedException();
            }

            public void ForceUnlock()
            {
                throw new NotImplementedException();
            }

            public bool IsUnlocked()
            {
                throw new NotImplementedException();
            }

            public bool IsEarnedFromStart { get; set; }

            public void ForceEarnReward()
            {
                throw new NotImplementedException();
            }

            public bool TryEarnReward()
            {
                throw new NotImplementedException();
            }

            public bool IsEarnedReward()
            {
                throw new NotImplementedException();
            }
        }

        [Serializable]
        public struct SkinDataByVisual
        {
            public VisualData Visual;
            public SkinData Skin;
        }

        [field: SerializeField] public int UnlockFromStartIndex { get; private set; }

        [field: SerializeField] public SkinInfo[] UnlockedSkinsByGameProgress { get; private set; }

        //[field: SerializeField] public UnlocksSkinInfo[] UnlockSkinInfo;
        [field: SerializeField] public TrackingVariable LevelPassedVariable { get; private set; }

        [field: SerializeField]
        //   public int SkinsCount => _unlockableSkinsByGameProgress.Length;
        
        public List<SkinData> GetAllSkins()
        {
            return GetAllSkins(VisualManager.SelectedVisual);
        }

        public List<SkinData> GetAllSkins(VisualData variant)
        {
            return null;
            /*
            var skins = new List<SkinData>(_unlockableSkinsByGameProgress.Length);
            
            foreach (var skinInfo in _unlockableSkinsByGameProgress)
            {
                foreach (var skinDataByVisual in skinInfo.SkinVariantsByVisual)
                {
                    if (skinDataByVisual.Visual == variant)
                    {
                        skins.Add(skinDataByVisual.Skin);
                        break;
                    }
                }
            }
            
            return skins;
            */
        }

        public int GetSkinIndex(SkinData skin)
        {
            /*
            for (var i = 0; i < _unlockableSkinsByGameProgress.Length; i++)
            {
                var skinInfo = _unlockableSkinsByGameProgress[i];
                if (skinInfo.SkinVariantsByVisual.Any(skinData => skinData.Skin == skin))
                {
                    return i;
                }
            }
            */

            throw new Exception($"Skin not found: {skin.name}");
        }

        public SkinData GetSkin(int index)
        {
            return GetSkin(index, VisualManager.SelectedVisual);
        }

        public SkinData GetSkin(int index, VisualData visualVariant)
        {
            /*
            foreach (var variant in _unlockableSkinsByGameProgress[index].SkinVariantsByVisual)
            {
                if (variant.Visual == visualVariant)
                {
                    return variant.Skin;
                }
            }
            */
            throw new ArgumentException($"Visual not found: {visualVariant.name}");
        }
    }
}