using NaughtyAttributes;
using UnityEngine;

namespace NavySpade.pj49.Scripts.UnitsQueues.Positions
{
    public abstract class PointsHolder : MonoBehaviour
    {
        [SerializeField]
        [Foldout("Debug")]
        private bool _isDebug;
        
        [ReadOnly]
        public Vector3[] _positions;
        
        public abstract Vector2 RectSize { get; }
        
        [Button("Set")]
        private void Awake()
        {
            _positions = GetPositions();
        }

        public Vector3 GetPosition(int index)
        {
            // Debug.Log($"Counts:{_positions.Length}, Requested Index{index}");
            return _positions[index];
        }

        protected abstract Vector3[] GetPositions();

        private void OnDrawGizmos()
        {
            if(_isDebug == false)
                return;
        
            for (int i = 0; i < _positions.Length; i++)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(_positions[i], 0.3f);
            
                Gizmos.color = Color.blue;
                if (i < _positions.Length - 1)
                {
                    Gizmos.DrawLine(_positions[i], _positions[i+1]);
                }
            }
        }
        
        public abstract void ChangeSize(int totalCount);
    }
}