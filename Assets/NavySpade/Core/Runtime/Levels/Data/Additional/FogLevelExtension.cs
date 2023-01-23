using System;
using UnityEngine;

namespace NavySpade.Core.Runtime.Levels.Data.Additional
{
    [Serializable]
    [AddTypeMenu("Fog")]
    public class FogLevelExtension : ILevelExtensionData
    {
        [SerializeField] private bool _enable;
        [SerializeField] private Color _color;
        [SerializeField] private float _startDistance;
        [SerializeField] private float _endDistance;

        public void Apply()
        {
            RenderSettings.fog = _enable;
            RenderSettings.fogColor = _color;
            RenderSettings.fogStartDistance = _startDistance;
            RenderSettings.fogEndDistance = _endDistance;
        }

        public void Clear()
        {
        }
    }
}