using NavySpade.Meta.Runtime.Economic.Prices.Interfaces;
using NavySpade.Modules.Utils.Serialization.SerializeReferenceExtensions.Runtime.Obsolete.SR;
using UnityEngine;

namespace NavySpade.Meta.Runtime.Skins.Configuration
{
    [CreateAssetMenu(fileName = "New Skin", menuName = "Meta/Skin", order = 0)]
    public class SkinData : ScriptableObject
    {
        [field: SerializeField] public Sprite Icon { get; private set; }

        //если будут проекты где будем юзать разную цену для одних и тех же скинов
        //то тогда придёться обворачивать это всё в шопайтем и покупать через него, а не через это поле
        [field: SerializeReference, SubclassSelector]
        public IPrice ShopDefaultPrice { get; private set; }
    }
}