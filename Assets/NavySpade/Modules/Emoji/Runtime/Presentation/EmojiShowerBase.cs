using UnityEngine;

namespace NavySpade.Modules.Emoji.Runtime.Presentation
{
    public abstract class EmojiShowerBase : MonoBehaviour
    {
        public abstract void TryShow(string key);
    }
}