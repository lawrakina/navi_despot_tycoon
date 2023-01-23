using Core.Damagables;
using Extensions;
using Misc.Damagables;
using NavySpade.Modules.Extensions.UnityTypes;
using UnityEngine;

namespace Project19.Enemies
{
    public class LookAtNearestEnemy : MonoBehaviour
    {
        [SerializeField] private Damageable _damagable;

        private IterationFilter<Damageable> _iterationFilter;

        private void Awake()
        {
            _iterationFilter = new IterationFilter<Damageable>().Where(d => d.IsAlive);
        }

        private void Update()
        {
            var target = Damageable.GetAllEnemysOfTeam(_damagable.CurrentTeam).FindClosed(transform.position, _iterationFilter);

            if (target == null)
                return;

            var dir = target.transform.position - transform.position;

            if (dir != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(dir);

            _dir = dir;
        }

        private Vector3 _dir;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, _dir);
        }
    }
}