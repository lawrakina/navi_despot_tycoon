using UnityEngine;

namespace NavySpade.Modules.Emoji.Runtime.Presentation
{
    public class EmojiCanvasShower : EmojiShowerBase
    {
        [SerializeField] private EmojiViewBase _view;

        private bool _locked;

        public override void TryShow(string key)
        {
            if (_locked)
            {
                return;
            }

            if (EmojiManager.TryGetEmoji(key, out var emojiData) == false)
            {
                Debug.LogWarning("Emoji not found!");
                return;
            }

            _view.UpdateEmoji(emojiData);
            
            ShowAndHideCoroutine();
        }

        private void ShowAndHideCoroutine()
        {
            _view.Root.TryShow(() => { _view.Root.TryHide(() => { _locked = false; }); });
        }
    }
}