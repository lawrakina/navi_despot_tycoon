using System.Collections.Generic;
using NavySpade.Modules.Emoji.Runtime.Data;
using NavySpade.Modules.Extensions.CsharpTypes;

namespace NavySpade.Modules.Emoji.Runtime.Services
{
    public class EmojiService : IEmojiService
    {
        private static EmojiConfig Config => EmojiConfig.Instance;

        private bool _isInitialized;
        private readonly Dictionary<string, List<IEmojiData>> _nameToSpriteList;

        public bool TryGetEmoji(string key, out IEmojiData emoji)
        {
            if (_isInitialized == false || _nameToSpriteList.ContainsKey(key) == false)
            {
                emoji = null;
                return false;
            }

            emoji = GetRandomEmoji(key);
            return true;
        }

        public IEmojiData GetRandomEmoji(string key)
        {
            if (_isInitialized == false || _nameToSpriteList.ContainsKey(key) == false)
            {
                return default;
            }

            var emoji = _nameToSpriteList[key].RandomElement();
            return emoji;
        }

        public EmojiService()
        {
            _nameToSpriteList = new Dictionary<string, List<IEmojiData>>();
            Init();
        }
        
        private void Init()
        {
            foreach (var sound in Config.GetAllEmojis())
            {
                if (_nameToSpriteList.ContainsKey(sound.Key) == false)
                {
                    _nameToSpriteList.Add(sound.Key, new List<IEmojiData>());
                }

                _nameToSpriteList[sound.Key].Add(sound);
            }

            _isInitialized = true;
        }
    }
}