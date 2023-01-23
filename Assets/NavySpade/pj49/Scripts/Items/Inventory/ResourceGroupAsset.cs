using UnityEngine;

namespace NavySpade.pj49.Scripts.Items.Inventory
{
    [CreateAssetMenu(fileName = "FILENAME", menuName = "Game/pj49/Resource Group", order = 0)]
    public class ResourceGroupAsset : ScriptableObject
    {
        [field: SerializeField] public Sprite Icon { get; private set; }
        
        [field: SerializeField] public int LimitCount { get; set; }
    }
}