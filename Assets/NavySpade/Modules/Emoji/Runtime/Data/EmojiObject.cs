using System.Collections.Generic;
using UnityEngine;

namespace NavySpade.Modules.Emoji.Runtime.Data
{
    [CreateAssetMenu(fileName = "New Emoji", menuName = "Game/Emoji", order = 51)]
    public class EmojiObject : ScriptableObject, IEmojiData
    {
        [field: SerializeField] public string Key { get; private set; }
        [field: SerializeField] public List<Sprite> Sprites { get; private set; }
    }
}