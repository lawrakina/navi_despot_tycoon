using System;
using NavySpade.Core.Runtime.Levels.Data.Additional;
using NavySpade.Modules.Utils.Serialization.SerializeReferenceExtensions.Runtime.Obsolete.SR;
using UnityEngine;

namespace NavySpade.Core.Runtime.Levels.Data
{
    public abstract class LevelDataBase : ScriptableObject
    {
        [Serializable]
        public struct AdditionData
        {
            [field: SR]
            [field: SerializeReference]
            public ILevelExtensionData ExtensionData { get; private set; }
        }

        [field: SerializeField] public AdditionData[] AdditionsData { get; private set; }
    }
}