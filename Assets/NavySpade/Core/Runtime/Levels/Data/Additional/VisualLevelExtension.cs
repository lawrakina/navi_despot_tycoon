using System;
using NavySpade.Modules.Visual.Runtime;
using NavySpade.Modules.Visual.Runtime.Data;
using UnityEngine;

namespace NavySpade.Core.Runtime.Levels.Data.Additional
{
    [Serializable]
    [AddTypeMenu("Visual")]
    public class VisualLevelExtension : ILevelExtensionData
    {
        [SerializeField] private VisualData _targetVisual;

        public void Apply()
        {
            VisualManager.SelectedVisual = _targetVisual;
        }

        public void Clear()
        {
        }
    }
}