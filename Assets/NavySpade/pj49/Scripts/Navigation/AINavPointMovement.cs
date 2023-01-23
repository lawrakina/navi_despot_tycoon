using System;
using NaughtyAttributes;
using NavySpade.Modules.Extensions.UnityTypes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NavySpade.pj49.Scripts.Navigation
{
    public class AINavPointMovement : ExtendedMonoBehavior<AINavPointMovement>
    {
        [field: SerializeField] public float MovementSpeed { get; set; }
        public NavigationLayerAsset LayerAsset;
        
        [SerializeField] private float LookAtSpeed;
        
        [field: SerializeField] public float ChangeToStopAtThisTick;
        [MinMaxSlider(0, 10)] public Vector2 StopDelay;
        
        public Rigidbody Rigidbody;
        public bool IsStayForewer;

        private AIPoint _currentPoint;
        private float _progress;
        private bool _isStoped;
        private Timer _stopTimer;
        private bool _isReachEnd;
        private bool _lookOnlyForward;

        private Vector3 _targetRandomRotationAtStayState;
        private bool _isDetachAtPath;

        private event Action ReachedEndOfPath;

        private void Start()
        {
            //StartMoving();
        }

        public void StartMoving()
        {
            var result = AIPoint.GetNearestPathPoint(transform.position, LayerAsset);

            transform.position = result.position;
            _currentPoint = result.aiPoint;
            _progress = InverseLerp(result.aiPoint.transform.position, result.aiPoint.NextPoint.transform.position, result.position);
            
            var currentPointPos = _currentPoint.transform.position;
            var nextPointPos = _currentPoint.NextPoint.transform.position;
            var dir = (nextPointPos - currentPointPos).normalized;
            transform.rotation = Quaternion.LookRotation(dir);
        }

        public void MoveToAiPoint(AIPoint targetPoint, Action reachedCallback)
        {
            _currentPoint = targetPoint;
            _progress = InverseLerp(
                targetPoint.transform.position,
                targetPoint.NextPoint.transform.position, 
                transform.position);
            
            var currentPointPos = _currentPoint.transform.position;
            var nextPointPos = _currentPoint.NextPoint.transform.position;
            var dir = (nextPointPos - currentPointPos).normalized;
            transform.rotation = Quaternion.LookRotation(dir);
            ReachedEndOfPath = reachedCallback;
            ChangeToStopAtThisTick = 0;
            _lookOnlyForward = true;
            IsStayForewer = false;
            _progress = 0;
        }
        
        public static float InverseLerp(Vector3 a, Vector3 b, Vector3 value)
        {
            Vector3 AB = b - a;
            Vector3 AV = value - a;

            float dotAB = Vector3.Dot(AB, AB);
            return Vector3.Dot(AV, AB) / dotAB;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _isDetachAtPath = true;
        }

        private void FixedUpdate()
        {
            if(_isReachEnd)
                return;
            
            if (_isDetachAtPath)
            {
                if (MoveToPathPoint())
                {
                    _isDetachAtPath = false;
                }
                else
                {
                    return;
                }
            }

            if (IsStayForewer)
            {
                if(_stopTimer != null)
                    _stopTimer.currentTime = -10;
                
                return;
            }
            
            if (Random.value <= ChangeToStopAtThisTick && (_stopTimer == null || _stopTimer.IsFinish()))
            {
                _stopTimer = new Timer(Random.Range(StopDelay.x, StopDelay.y));
                _targetRandomRotationAtStayState = Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)) * Vector3.forward;
            }

            if ((_stopTimer != null && _stopTimer.IsFinish() == false) || IsStayForewer)
            {
                StayAtPos();
                _stopTimer.Update(Time.fixedDeltaTime);
                return;
            }
            
            MovementToPath();
        }

        private void StayAtPos()
        {
            LookAtSmooth(_targetRandomRotationAtStayState);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>true if returned to path</returns>
        private bool MoveToPathPoint()
        {
            var currentPointPos = _currentPoint.transform.position;
            var nextPointPos = _currentPoint.NextPoint.transform.position;
            var pathTargetPos = Vector3.Lerp(currentPointPos, nextPointPos, _progress);
            var dir = pathTargetPos - transform.position;
            dir.Normalize();

            Rigidbody.position = Vector3.MoveTowards(Rigidbody.position, pathTargetPos, MovementSpeed * Time.fixedDeltaTime);
            
            LookAtSmooth(dir);

            return Vector3.Distance(transform.position, pathTargetPos) < .1;
        }

        private void MovementToPath()
        {
            var currentPointPos = _currentPoint.transform.position;
            var nextPointPos = _currentPoint.NextPoint.transform.position;
            var distance = Vector3.Distance(currentPointPos, nextPointPos);

            _progress += MovementSpeed / distance * Time.fixedDeltaTime;
            
            Rigidbody.position = Vector3.Lerp(currentPointPos, nextPointPos, _progress);

            if (_progress > 1)
            {
                _currentPoint = _currentPoint.NextPoint;
                _progress = 0;
                _isReachEnd = _currentPoint.NextPoint == null;
                if (_isReachEnd)
                {
                    ReachedEndOfPath?.Invoke();
                }
            }

            var dir = (nextPointPos - currentPointPos).normalized;
            LookAtSmooth(dir);
        }

        private void LookAtSmooth(Vector3 dir)
        {
            if (_lookOnlyForward)
            {
                dir.y = 0;
            }
            
            Rigidbody.rotation = Quaternion.RotateTowards(
                Rigidbody.rotation, 
                Quaternion.LookRotation(dir), 
                LookAtSpeed * Time.fixedDeltaTime);
        }
        
    }
}