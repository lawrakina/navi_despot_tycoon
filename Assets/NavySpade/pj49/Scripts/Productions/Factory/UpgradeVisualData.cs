using UnityEngine;

namespace NavySpade.pj49.Scripts.Productions.Factory
{
    [CreateAssetMenu(fileName = "UpgradeVisualData", menuName = "Game/pj49/UpgradeVisualData")]
    public class UpgradeVisualData : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        
        [field: SerializeField] public string Suffix { get; private set; }
        
        [field: SerializeField] public string Prefix { get; private set; }
        
        [field: SerializeField] public bool HasResourceIcon { get; private set; }
        
        [field: SerializeField] public Sprite ResourceIcon { get; private set; }
    }
}