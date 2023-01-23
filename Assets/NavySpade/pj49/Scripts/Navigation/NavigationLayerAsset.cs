using NaughtyAttributes;
using UnityEngine;

namespace NavySpade.pj49.Scripts.Navigation
{
    [CreateAssetMenu(fileName = "FILENAME", menuName = "Game/pj49/navigation layer", order = 0)]
    public class NavigationLayerAsset : ScriptableObject
    {
        [field: Foldout("Debug")] 
        [field: SerializeField] public Color DebugColor { get; private set; }
    }
}