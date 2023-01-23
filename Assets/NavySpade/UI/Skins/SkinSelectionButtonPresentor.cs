using Core.Meta.Skins;
using Core.UI.Economic;
using NavySpade.Meta.Runtime.Skins.Configuration;
using NavySpade.Modules.Meta.Runtime.Skins.Configuration;
using TMPro;
using UnityEngine;

namespace Core.UI.Skins
{
    public class SkinSelectionButtonPresentor : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private PricePresentor _price;
        [SerializeField] private string _selectText;
        [SerializeField] private string _selectedText;
        
        public void UpdateView(int skinIndex)
        {
            if (SkinsManager.IsSkinOpen(skinIndex))
            {
                if (SkinsManager.SelectedSkinIndex == skinIndex)
                {
                    _text.text = _selectedText;
                }
                else
                {
                    _text.text = _selectText;
                }
                
                _price.SetActive(false);
            }
            else
            {
                _price.SetActive(true);
                _text.text = "";
                _price.UpdateView(SkinsConfig.Instance.GetSkin(skinIndex).ShopDefaultPrice);
            }
        }
    }
}