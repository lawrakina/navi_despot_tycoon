using Core.Damagables;
using Misc.Damagables;
using Project19.Weapons.ProjectileEmitWeapon;
using UnityEngine;

namespace Project19.Weapons.CirculImpuls
{
    public class CirculImpulsProjectile : SimpleProjectile
    {
        protected CirculImpulsData CirculImpulsData => SpellData as CirculImpulsData;

        private Vector3 _normalScale;
        
        protected override void OnInitBase()
        {
            _normalScale = transform.localScale;
            transform.localScale = Vector3.zero;
        }

        protected override float MoveTick(float dt, float speed)
        {
            transform.localScale = _normalScale * CirculImpulsData.SizeMultiplyOverLifetime.Evaluate(CoverDistanceNormal);
            Rb.velocity = Vector3.zero;
            
            return dt * speed;
        }

        protected override void OnEnterToTarget(Damageable damageable)
        {
            DealDamage(damageable);
            PlayHitEffect();
        }
    }
}