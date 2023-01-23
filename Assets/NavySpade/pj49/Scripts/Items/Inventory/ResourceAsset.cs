using NaughtyAttributes;
using NavySpade.Meta.Runtime.Economic.Prices.Interfaces;
using NavySpade.pj49.Scripts.Saving;
using UnityEngine;


namespace NavySpade.pj49.Scripts.Items.Inventory{
    [CreateAssetMenu(fileName = "FILENAME", menuName = "Game/pj49/Resource", order = 0)]
    public class
        ResourceAsset : ScriptableObject{
        [field: ShowAssetPreview]
        [field: SerializeField]
        public Sprite Icon{ get; private set; }

        [field: SerializeField]
        public TypeResources TypeResources{ get; set; }

        [field: SerializeField]
        public string DisplayName{ get; private set; }

        [field: SerializeField]
        public GameObject Prefab{ get; private set; }

        [field: SerializeField]
        public bool HasLimit{ get; private set; }

        [field: ShowIf(nameof(HasLimit))]
        [field: SerializeField]
        public bool UseItemGroup{ get; private set; }

        [field: ShowIf(nameof(HasLimit))]
        [field: HideIf(nameof(UseItemGroup))]
        [field: SerializeField]
        public int LimitCount{ get; private set; }

        [field: ShowIf(EConditionOperator.And, nameof(HasLimit), nameof(UseItemGroup))]
        [field: SerializeField]
        public ResourceGroupAsset Group{ get; private set; }

        [field: SerializeField]
        public int Multiplier{ get; private set; }

        [field: Min(1)]
        [field: SerializeField]
        public int CountInVisual{ get; private set; } = 1;

        public Item CreateItem(int count = 1){
            if (HasLimit && UseItemGroup == false)
                return new LimitedItem(this, count, LimitCount);

            return new Item(this, count);
        }
    }

    public enum TypeResources{
        Battery,
        Food,
        Money,
        Ore,
        HumanStandard,
        HumanMuscle
    }
}