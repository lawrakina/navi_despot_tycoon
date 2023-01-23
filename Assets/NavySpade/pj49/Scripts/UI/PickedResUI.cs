using System.Collections;
using UnityEngine;
using UnityEngine.UI;


namespace NavySpade.pj49.Scripts.UI
{
    public class PickedResUI : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Image _icon;

        private readonly int _deathAnimHash = Animator.StringToHash("Death");

        public void Init(Sprite sprite)
        {
            _icon.sprite = sprite;
        }

        public IEnumerator MoveResTo(Vector3 endPos)
        {
            float progress = 0;
            Vector3 startPos = transform.position;
            
            while (progress < 1)
            {
                progress += Time.deltaTime;
                transform.position = Vector3.Slerp(startPos, endPos, progress);
                if (progress > 0.75f)
                {
                    _animator.SetTrigger(_deathAnimHash);
                }
                yield return null;
            }
        }
    }
}