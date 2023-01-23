using System;
using Core.Damagables;
using Misc.Damagables;
using Misc.Damagables.Effects;
using UnityEngine;

namespace Project19.Weapons.ProjectileEmitWeapon
{
    public class SimpleProjectile : ProjectileBase
    {
        [SerializeField] private GameObject _hitPrefab;

        private int _damage;
        private bool _isDamageOrHeal;

        protected GameObject HitPrefab => _hitPrefab;

        public void Init(int value, bool isDamageOrHeal)
        {
            _damage = value;
            _isDamageOrHeal = isDamageOrHeal;
        }

        protected override float MoveTick(float dt, float speed)
        {
            var distance = dt * speed;
            var dir = transform.forward * distance;

            Rb.position += dir;

            return distance;
        }

        protected override void OnEnterToTarget(Damageable damageable)
        {
            DealDamage(damageable);
            PlayHitEffect();
            Destroy(gameObject);
        }

        protected virtual void DealDamage(Damageable damageable)
        {
            if (_isDamageOrHeal)
            {
                var dir = damageable.transform.position - transform.position;
                dir.Normalize();

                damageable.DealDamage(_damage, Team, new RepulsionParameter(dir, Stat.RagdollVelocity));
            }
            else
            {
                damageable.Heal(_damage, Team);
            }
        }

        protected void OnDestroy()
        {
            PlayHitEffect();
        }

        protected void PlayHitEffect()
        {
            if (HitPrefab == null)
                return;

            Instantiate(HitPrefab, transform.position, transform.rotation);
        }
    }
}