using System.Collections.Generic;
using Core.Damagables;
using Extensions;
using NavySpade.Modules.Extensions.UnityTypes;
using Project19.Weapons.ProjectileEmitWeapon;
using UnityEngine;

namespace Project19.Weapons.Lighting
{
    public class LightingProjectile : SimpleProjectile
    {
        protected LightingData LightingData => SpellData as LightingData;
        
        private List<Damageable> _collidedEnemys = new List<Damageable>();
        private int _currentRicochetCount;

        private IterationFilter<Damageable> _filter;

        private Vector3 _dir;
        private Vector3 _pos;

        private IterationFilter<Damageable> Filter => _filter ??= new IterationFilter<Damageable>().Where(e => e.IsAlive == false);

        public void InitLighting(int ricochetCount)
        {
            _currentRicochetCount = ricochetCount;
        }

        protected override void OnEnterToTarget(Damageable damageable)
        {
            if (_collidedEnemys.Contains(damageable))
                return;
            
            DealDamage(damageable);
            PlayHitEffect();
                
            if(_currentRicochetCount <= 0)
                Destroy(gameObject);

            var array = Damageable.GetAllEnemysOfTeam(Team);
            var result = array.FindClosed(transform.position, Filter);

            if (result == null || Vector3.Distance(transform.position, result.transform.position) > LightingData.MAXDistanceToRicochet)
                return;

            var from = result.transform.position;
            var to = transform.position;
            from.y = 0;
            to.y = 0;
            
            var dir = from - to;
            dir.Normalize();

            _dir = dir;
            _pos = transform.position;
            transform.forward = dir;
            
            _currentRicochetCount--;
        }

        protected override void DealDamage(Damageable damageable)
        {
            base.DealDamage(damageable);
            _collidedEnemys.Add(damageable);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(_pos, _dir);
        }
    }
}