using System;
using System.Collections.Generic;
using UnityEngine;

namespace NavySpade.Modules.Emoji.Runtime.Data
{
    [Serializable]
    public struct EmojiEntry : IEmojiData
    {
        [field: SerializeField] public string Key { get; private set; }
        [field: SerializeField] public List<Sprite> Sprites { get; private set; }
        
        public EmojiEntry(string key, List<Sprite> sprites = null)
        {
            Key = key;
            Sprites = sprites;
        }
    }
}