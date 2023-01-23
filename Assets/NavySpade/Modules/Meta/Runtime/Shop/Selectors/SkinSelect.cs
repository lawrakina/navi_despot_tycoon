using System;
using Core.Meta.Skins;
using NavySpade.Meta.Runtime.Skins.Configuration;
using NavySpade.Modules.Meta.Runtime.Skins.Configuration;

namespace Core.Meta.Shop.Selectors
{
    [Serializable]
    [CustomSerializeReferenceName("Set Skin")]
    public class SkinSelect : ISelected
    {
        public SkinData Skin;
        
        public void Select()
        {
            SkinsManager.SelectedSkinIndex = SkinsConfig.Instance.GetSkinIndex(Skin);
        }

        public void Deselect()
        {
            
        }

        public bool IsSelected()
        {
            var config = SkinsConfig.Instance;
            var currentSkinIndex = config.GetSkinIndex(Skin);
            
            return SkinsManager.SelectedSkinIndex == currentSkinIndex;
        }
    }
}