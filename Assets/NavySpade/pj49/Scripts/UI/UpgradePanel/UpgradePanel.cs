using System.Collections;
using EventSystem.Runtime.Core.Dispose;
using EventSystem.Runtime.Core.Managers;
using NavySpade.pj49.Scripts.Productions.Factory;
using UnityEngine;
using UnityEngine.UI;

namespace NavySpade.pj49.Scripts.UI
{
    public class UpgradePanel : MonoBehaviour
    {
        [SerializeField] private GameObject _panel;
        [SerializeField] private Button[] _closeButtons;
        [SerializeField] private Animator _animator;
        [SerializeField] private float _animSpeed = 1;
      
        
        protected EventDisposal _disposal = new EventDisposal();
        
        private readonly int _closeAnimHash = Animator.StringToHash("upgrademoveDOWN");
        private readonly int _motionTimeHash = Animator.StringToHash("MotionTime");
        
        protected virtual void Start()
        {
            foreach (var closeButton in _closeButtons)
            {
                closeButton.onClick.AddListener(Close);
            }
        }
        
        private void OnDestroy()
        {
            _disposal.Dispose();
        }

        protected void ShowPopup()
        {
            StopAllCoroutines();
            // Debug.Log($" ---------------------- Show popup");

            StartCoroutine(ShowPanel());
        }
        
        public void Close()
        {
            StopAllCoroutines();
            // Debug.Log($" ---------------------- Close");
            StartCoroutine(HidePanel());
        }

        private IEnumerator ShowPanel()
        {
            _panel.SetActive(true);
            float startedValue = _animator.GetFloat(_motionTimeHash);
            while (startedValue < 1)
            {
                startedValue += Time.deltaTime * _animSpeed;
                _animator.SetFloat(_motionTimeHash, startedValue);
                yield return null;
            }
        }

        private IEnumerator HidePanel()
        {
            float startedValue = _animator.GetFloat(_motionTimeHash);
            while (startedValue > 0)
            {
                startedValue -= Time.deltaTime * _animSpeed;
                _animator.SetFloat(_motionTimeHash, startedValue);
                yield return null;
            }
            
            _panel.SetActive(false);
        }
    }
}