using NavySpade.Modules.Emoji.Runtime.Data;
using NavySpade.Modules.Emoji.Runtime.Services;

namespace NavySpade.Modules.Emoji.Runtime
{
    public static class EmojiManager
    {
        private static IEmojiService Service { get; }

        public static bool TryGetEmoji(string name, out IEmojiData emojiData)
        {
            return Service.TryGetEmoji(name, out emojiData);
        }

        static EmojiManager()
        {
            Service = new EmojiService();
        }
    }
}