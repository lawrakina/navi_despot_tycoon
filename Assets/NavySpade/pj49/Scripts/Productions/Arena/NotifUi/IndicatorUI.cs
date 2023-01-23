using System;
using NavySpade.Modules.Extensions.UnityTypes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace NavySpade.pj46.UI.PoppingText
{
    public class IndicatorUI : ExtendedMonoBehavior
    {
        [SerializeField] private Vector3 _appearOffsetRange;
        
        [field: SerializeField] public RectTransform _rectTransform { get; private set; }
        
        [field: SerializeField] public TMP_Text _text { get; private set; }
        
        [field: SerializeField] public float LifeTime { get; private set; }
        
        public UnityEvent OnStartAnimation;

        public event Action<IndicatorUI> OnReturnedToPool;

        public ObjectPoolOld<IndicatorUI> Pool;

        private Transform _attachedTransform;
        private Vector3 _offset;
        private Vector3 _animationOffset;

        private void Awake()
        {
            gameObject.SetActive(false);
        }
        
        public void Initilaze(string text, Transform attachedTransform, Vector3 animationOffset = new Vector3())
        {
            _attachedTransform = attachedTransform;
            _animationOffset = animationOffset;
            _text.text = text;
            _offset = new Vector3(
                Random.Range(-_appearOffsetRange.x, _appearOffsetRange.x),
                Random.Range(-_appearOffsetRange.y, _appearOffsetRange.y),
                Random.Range(-_appearOffsetRange.z, _appearOffsetRange.z));
            
            UpdatePosition();
        }

        private void LateUpdate()
        {
            if(_attachedTransform == null)
                return;
            
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            var worldPos = _attachedTransform.transform.position + _offset + _animationOffset;
            var screenPos = Camera.main.WorldToScreenPoint(worldPos) ;
            _rectTransform.position = screenPos;
            _rectTransform.localScale = Vector3.one;
            _animationOffset += _animationOffset;
        }

        public void OnTakeFromPool()
        {
            if(gameObject == null)
                return;
                
            gameObject.SetActive(true);
            OnStartAnimation?.Invoke();

            InvokeAtTime(LifeTime, OnReturnToPool);
        }

        public void OnReturnToPool()
        {
            gameObject.SetActive(false);
            Pool.Return(this);
            OnReturnedToPool?.Invoke(this);
        }
    }
}