using System;
using UnityEngine;

namespace NavySpade.pj49.Scripts.Zone
{
    
    [ExecuteInEditMode]
    public class ZoneVisual : MonoBehaviour
    {
        [SerializeField] private Collider _boundsCollider;
        [SerializeField] private SpriteRenderer[] _renderers;
        [SerializeField] private Vector2 _padding;

        private void Update()
        {
            Vector3 colliderSize = _boundsCollider.bounds.size;
            ChangeAreaSize( new Vector3(colliderSize.x, colliderSize.z));
        }

        public void ChangeAreaSize(Vector2 size)
        {
            foreach (var rend in _renderers)
            {
                rend.size = new Vector2(size.x + _padding.x, size.y + _padding.y);
            }
        }
    }
}