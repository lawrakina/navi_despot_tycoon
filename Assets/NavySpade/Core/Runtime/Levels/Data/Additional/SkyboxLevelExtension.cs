using System;
using UnityEngine;

namespace NavySpade.Core.Runtime.Levels.Data.Additional
{
    [Serializable]
    [AddTypeMenu("Skybox")]
    public class SkyboxLevelExtension : ILevelExtensionData
    {
        [SerializeField] private Material _material;
        
        public void Apply()
        {
            if (QualitySettings.renderPipeline == null)
            {
                RenderSettings.skybox = _material;
            }
            else
            {
                var mainCamera = Camera.main;
                var skyboxComponent = mainCamera.GetComponent<UnityEngine.Skybox>();
                skyboxComponent.material = _material;
            }
        }

        public void Clear()
        {
            
        }
    }
}