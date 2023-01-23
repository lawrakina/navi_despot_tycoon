using NavySpade.Common.Runtime.Api.Context;
using NavySpade.Modules.Utils.Serialization.Interfaces.Runtime;
using UnityEngine;

namespace NavySpade.Modules.Emoji.Runtime.Presentation
{
    public class EmojiSpawner : EmojiShowerBase
    {
        [SerializeField] private InterfaceReference<ITransformContextProvider> _context;
        [SerializeField] private EmojiImageView _prefab;
        [SerializeField] private Transform _origin;

        public override void TryShow(string key)
        {
            if (EmojiManager.TryGetEmoji(key, out var emojiData) == false)
            {
                Debug.LogWarning("Emoji not found!");
                return;
            }

            var emojiInstance = Instantiate(_prefab, _context.Value.Root);
            emojiInstance.transform.SetPositionAndRotation(_origin.position, _origin.rotation);
            emojiInstance.UpdateEmoji(emojiData);
        }
    }
}