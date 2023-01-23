using Core.Damagables;
using Misc.Damagables;
using UnityEngine;

namespace Misc.Entities
{
    public class Explosion : MonoBehaviour
    {
        [SerializeField] private float _radius;
        [SerializeField] private int _damage = 5;

        private static RaycastHit[] _hitsCache;

        public Damageable.Team AttackTeam;

        private void Awake()
        {
            if (_hitsCache == null)
                _hitsCache = new RaycastHit[32];
        }

        public void Boom()
        {
            var resultsCount = Physics.SphereCastNonAlloc(transform.position, _radius, Vector3.down, _hitsCache);

            for (int i = 0; i < resultsCount; i++)
            {
                if (_hitsCache[i].transform == transform)
                    continue;

                if (_hitsCache[i].transform.TryGetComponent<Damageable>(out var health) == false)
                    continue;

                health.DealDamage(_damage, AttackTeam);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(1f, 0f, 0f, 0.22f);
            Gizmos.DrawSphere(transform.position, _radius);
        }
    }
}