using System;
using UnityEngine;

namespace pj40.Core.Tweens
{
    [Serializable]
    public class MoveToTransformTween<T> where T : MovementTypeBase
    {
        public Vector3 TargetOffset { get; }

        public MoveToTransformTween(Transform tweenObject, T movementTypeBase, Vector3 targetOffset,
            float minimumDistance = .2f)
        {
            TargetOffset = targetOffset;
            _movementType = movementTypeBase;
            _tweenObject = tweenObject;
            _minimumDistance = minimumDistance;
        }

        private Transform _tweenObject;

        private Vector3 _targetPosition;
        private Transform _targetTransform;

        public Vector3 TargetPosition
        {
            get
            {
                if (_targetTransform != null)
                    return _targetTransform.position;

                if (_getTarget != null)
                    return _getTarget.Invoke();

                return _targetPosition;
            }
        }

        public Transform TweenObject => _tweenObject;

        private T _movementType;
        private float _minimumDistance;
        private Func<Vector3> _getTarget;

        private bool _isFinished = true;
        private Action _completeAction;
        private Transform _forward;

        public bool IsFinished => _isFinished;
        
        public void StartTween(Transform target, float speed, Action onComplete = default)
        {
            _targetTransform = target;
            _isFinished = false;

            _movementType.Speed = speed;
            _movementType.Init(TweenObject.position, target.position);

            _completeAction = onComplete;
        }
        
        public void StartTween(Func<Vector3> target, float speed, Action onComplete = default)
        {
            _getTarget = target;
            _isFinished = false;

            _movementType.Speed = speed;
            _movementType.Init(TweenObject.position, _getTarget.Invoke());

            _completeAction = onComplete;
        }
        
        public void StartTween(Func<Vector3> target, float speed, Transform forwardLooking, Action onComplete = default)
        {
            _forward = forwardLooking;
            _getTarget = target;
            _isFinished = false;

            _movementType.Speed = speed;
            _movementType.Init(TweenObject.position, _getTarget.Invoke());

            _completeAction = onComplete;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns>is finish</returns>
        public bool Update(float dt)
        {
            if (_isFinished)
                return true;
            
            if(_targetTransform != null)
                _targetPosition = _targetTransform.position;

            var position = _movementType.NextPosition(TargetPosition, dt);
            TweenObject.position = position;

            var currentDistance = CalculateFullDistance(TweenObject.position, TargetPosition);

            if (_forward != null)
                TweenObject.transform.forward = _forward.forward;
            
            if (currentDistance > _minimumDistance)
                return false;

            _isFinished = true;

            _completeAction?.Invoke();
            return true;
        }

        private float CalculateFullDistance(Vector3 currentPosition, Vector3 targetPosition)
        {
            return Mathf.Sqrt(
                Mathf.Pow(targetPosition.x - currentPosition.x, 2) +
                Mathf.Pow(targetPosition.y - currentPosition.y, 2) +
                Mathf.Pow(targetPosition.z - currentPosition.z, 2)
            );
        }
    }
}