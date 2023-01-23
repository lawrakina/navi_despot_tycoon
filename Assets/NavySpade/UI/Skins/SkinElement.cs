using Core.UI.Main;
using Core.Meta.Skins;
using NavySpade.Meta.Runtime.Skins.Configuration;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Core.UI.Skins
{
    [RequireComponent(typeof(SelectionItem))]
    public class SkinElement : MonoBehaviour
    {
        [SerializeField] private Image _icon;

        private SelectionItem _selection;

        private void Awake()
        {
            _selection = GetComponent<SelectionItem>();
        }

        public void InitView(UnityAction selectedCallback = null)
        {
            if(selectedCallback != null)
                _selection.Selected.AddListener(selectedCallback);
        }

        public void UpdateView(SkinData attachedData)
        {
            _icon.sprite = attachedData.Icon;
        }
    }
}