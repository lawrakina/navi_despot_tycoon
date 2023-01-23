using NavySpade.Modules.Emoji.Runtime.Data;
using NavySpade.Modules.Extensions.CsharpTypes;
using UnityEngine;
using UnityEngine.UI;

namespace NavySpade.Modules.Emoji.Runtime.Presentation
{
    public class EmojiImageView : EmojiViewBase
    {
        [SerializeField] private Image _image;

        public override void UpdateEmoji(IEmojiData emojiData)
        {
            _image.sprite = emojiData.Sprites.RandomElement();
        }
    }
}