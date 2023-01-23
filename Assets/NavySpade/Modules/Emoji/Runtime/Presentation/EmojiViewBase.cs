using NavySpade.Modules.Emoji.Runtime.Data;
using NavySpade.UI.VariableViews.Base;

namespace NavySpade.Modules.Emoji.Runtime.Presentation
{
    public abstract class EmojiViewBase : ViewBase
    {
        public abstract void UpdateEmoji(IEmojiData emojiData);
    }
}