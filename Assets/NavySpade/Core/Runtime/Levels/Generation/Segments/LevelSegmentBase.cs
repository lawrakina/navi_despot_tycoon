using JetBrains.Annotations;
using UnityEngine;

namespace NavySpade.Core.Runtime.Levels.Generation.Segments
{
    public abstract class LevelSegmentBase : MonoBehaviour
    {
        [PublicAPI]
        public abstract Transform Origin { get; }
        
        [PublicAPI]
        public abstract Vector3 Size { get; }
    }
}