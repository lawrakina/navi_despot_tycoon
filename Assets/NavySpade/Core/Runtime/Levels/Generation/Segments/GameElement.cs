using UnityEngine;

namespace NavySpade.Core.Runtime.Levels.Generation.Segments
{
    public class GameElement : LevelSegmentBase
    {
        [SerializeField] private Transform _point;
        [SerializeField] private Vector3 _size = new Vector3(10, 20, 10);
        [SerializeField] private Color _gizmosColor;

        public override Transform Origin => _point;
        public override Vector3 Size => _size;

        private void OnDrawGizmos()
        {
            Gizmos.color = _gizmosColor;
            Gizmos.DrawCube(transform.position, _size);
        }
    }
}