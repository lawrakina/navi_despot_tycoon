using UnityEngine;

namespace NavySpade.Core.Runtime.Levels.Generation.Segments
{
    [RequireComponent(typeof(LevelSegmentBase))]
    public class LevelSegmentVisual : MonoBehaviour
    {
        [SerializeField] private Renderer _background;

        private LevelSegmentBase _element;

        private void Awake()
        {
            _element = GetComponent<LevelSegmentBase>();
        }

        public void ShowBackground()
        {
            _background.enabled = true;
        }

        public void HideBackground()
        {
            _background.enabled = false;
        }
    }
}
