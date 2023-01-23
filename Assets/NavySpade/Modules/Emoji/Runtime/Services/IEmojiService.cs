using JetBrains.Annotations;
using NavySpade.Modules.Emoji.Runtime.Data;

namespace NavySpade.Modules.Emoji.Runtime.Services
{
    public interface IEmojiService
    {
        [PublicAPI]
        bool TryGetEmoji(string key, out IEmojiData emoji);
    }
}