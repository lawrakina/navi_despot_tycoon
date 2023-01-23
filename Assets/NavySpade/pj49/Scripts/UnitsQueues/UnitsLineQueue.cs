using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace NavySpade.pj49.Scripts
{
    public class UnitsLineQueue : MonoBehaviour, IUnitMovementQueue
    {
        [SerializeField] private int _pointsCount;
        [SerializeField] private float _distanceBetweenPoints;
        [SerializeField] private float _distanceBetweenUnits;

        [Foldout("Debug")] 
        [SerializeField] private bool _isDrawDebug;

        private Vector3[] _points;
        private int _lastPointIndex;
        private bool _isReachLimit;
        
        private void Awake()
        {
            _points = new Vector3[_pointsCount];
        }

        private void FixedUpdate()
        {
            CalculatePositions();
        }
        
        public Vector3 GetPosition(int unitIndex)
        {
            return GetPositionOverDistance(unitIndex * _distanceBetweenUnits);
        }

        public bool IsEnable => enabled;

        public Vector3 GetPositionOverDistance(float distance)
        {
            var d = 0f;
            var previewPos = transform.position;
            var isFirstIteration = true;

            foreach (var point in Points())
            {
                if (isFirstIteration)
                {
                    d += Vector3.Distance(previewPos, point);
                    
                    isFirstIteration = false;
                }
                

                d += _distanceBetweenPoints;

                if (d > distance)
                {
                    var direction = previewPos - point;
                    direction.Normalize();
                    var addDistance = d - distance;
                    
                    return previewPos + direction * addDistance;
                }
                
                previewPos = point;
            }

            return previewPos;
        }

        private IEnumerable<Vector3> Points()
        {
            if(_points == null || _points.Length <= 0)
                yield break;
            
            for (int i = _lastPointIndex; i >= 0; i--)
            {
                yield return _points[i];
            }

            if (_isReachLimit)
            {
                for (int i = _points.Length - 1; i >= _lastPointIndex; i--)
                {
                    yield return _points[i];
                }
            }
        }

        /// <summary>
        /// O(1)
        /// </summary>
        private void CalculatePositions()
        {
            var distance = Vector3.Distance(transform.position, _points[_lastPointIndex]);
            
            if (distance > _distanceBetweenPoints)
            {
                var dir = transform.position - _points[_lastPointIndex];
                dir.Normalize();
                
                while ((distance -= _distanceBetweenPoints) > 0)
                {
                    int nextPointIndex;

                    if (_lastPointIndex + 1 >= _points.Length)
                    {
                        _isReachLimit = true;
                        nextPointIndex = 0;
                    }
                    else
                    {
                        nextPointIndex = _lastPointIndex + 1;
                    }
                    
                    _points[nextPointIndex] = _points[_lastPointIndex] + dir * _distanceBetweenPoints;
                    _lastPointIndex = nextPointIndex;
                }
            }
        }

        private void OnDrawGizmos()
        {
            if(_isDrawDebug == false)
                return;
            
            Gizmos.color = Color.red;
            
            foreach (var point in Points())
            {
                Gizmos.DrawSphere(point, .1f);
            }
            
        }
    }
}