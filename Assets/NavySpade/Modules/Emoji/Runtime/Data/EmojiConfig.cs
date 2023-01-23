using System.Collections.Generic;
using NavySpade.Modules.Configuration.Runtime.SO;
using UnityEngine;

namespace NavySpade.Modules.Emoji.Runtime.Data
{
    public class EmojiConfig : ObjectConfig<EmojiConfig>
    {
        [SerializeField] private EmojiObject[] _emojis;

        public IEnumerable<IEmojiData> GetAllEmojis() => _emojis;
    }
}