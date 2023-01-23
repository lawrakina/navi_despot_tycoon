using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace NavySpade.Modules.Emoji.Runtime.Data
{
    public interface IEmojiData
    {
        [PublicAPI]
        string Key { get; }
        
        [PublicAPI]
        List<Sprite> Sprites { get; }
    }
}