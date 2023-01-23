using System;
using UnityEngine;

namespace pj40.Core.Tweens
{
    public class CreateParabolaAnimation : MonoBehaviour
    {
        public Vector3 WorldPosition;
        public float Speed;
        public float ArkHeight = 1;

        private MoveToTransformTween<ParabolaMovement> _tween;

        private void OnEnable()
        {
            
        }

        private void OnDisable()
        {
            
        }

        private void Start()
        {
            _tween = new MoveToTransformTween<ParabolaMovement>(transform, new ParabolaMovement(ArkHeight),
                Vector3.zero, .01f);

            var pos = transform.position;
            var localPos = transform.localPosition;
            
            transform.position = WorldPosition;
            _tween.StartTween(() => pos, Speed, () => transform.localPosition = localPos);
        }

        private void Update()
        {
            _tween.Update(Time.deltaTime);
        }
    }
}