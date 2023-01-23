using System;
using Core.Visual;
using NavySpade.Modules.Visual.Runtime.Data;
using UnityEngine;

namespace Core.Game.Configuration
{
    [Serializable]
    public class GameViewParameters
    {
        [field: SerializeField] public SkyParameters Sky { get; private set; }
        [field: SerializeField] public FogParameters Fog { get; private set; }
    }
}