using NavySpade.Meta.Runtime.Skins.Configuration;
using NavySpade.Modules.Meta.Runtime.Skins.Configuration;
using UnityEngine;

namespace Core.Meta.Skins
{
    public class SkinVariant : MonoBehaviour
    {
        [SerializeField] private SkinData _attachedSkin;

        private static SkinsConfig Config => SkinsConfig.Instance;
        
        private void Awake()
        {
            SkinsManager.SelectedSkinChange += OnVisualChange;
        }

        private void Start()
        {
            var usedSkin = Config.GetSkin(SkinsManager.SelectedSkinIndex);
            OnVisualChange(usedSkin);
        }

        private void OnDestroy()
        {
            SkinsManager.SelectedSkinChange -= OnVisualChange;
        }

        private void OnVisualChange(SkinData visual)
        {
            gameObject.SetActive(_attachedSkin == visual);
        }
    }
}