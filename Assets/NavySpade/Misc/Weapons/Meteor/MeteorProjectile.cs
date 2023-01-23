using Core.Damagables;
using Misc.Damagables;
using Project19.Weapons.ProjectileEmitWeapon;
using UnityEngine;

namespace Project19.Weapons.Meteor
{
    public class MeteorProjectile : SimpleProjectile
    {
        protected MeteorStats MeteorStat => Stat as MeteorStats;

        private Vector3 _velocity;

        public void InitMeteor(float angle)
        {
            var targetPos = Target.position;
            var distanceToTarget = Vector3.Distance(transform.position, targetPos);

            if (distanceToTarget > Stat.CircularDistance)
                targetPos = transform.position + transform.forward * Stat.CircularDistance;

            _velocity = CalculateVelocity(transform.position, targetPos, angle);
        }

        protected override float MoveTick(float dt, float speed)
        {
            Rb.position += _velocity * (dt * speed);
            _velocity += Physics.gravity * (dt * speed);

            var meteorData = SpellData as MeteorData;

            if (Rb.position.y <= MeteorStat.DestroyYPos)
            {
                InvokeAOEAttack();
                HitEffect();
                Destroy(gameObject);
            }

            return dt * speed;
        }

        protected override void OnEnterToTarget(Damageable damageable)
        {
        }

        protected void InvokeAOEAttack()
        {
            var result = Physics.SphereCastAll(transform.position, MeteorStat.Aoe, Vector3.down);

            foreach (var hit in result)
            {
                if (!hit.transform.TryGetComponent<Damageable>(out var damagable)) 
                    continue;
                
                DealDamage(damagable);
            }
        }

        private void HitEffect()
        {
            var hit = Instantiate(HitPrefab, transform.position, Quaternion.identity);
            hit.transform.localScale = Vector3.one * 1.5f * MeteorStat.Aoe;
        }

        public Vector3 CalculateVelocity(Vector3 from, Vector3 to, float angle)
        {
            var dir = to - from; // get Target Direction
            var height = dir.y; // get height difference
            dir.y = 0; // retain only the horizontal difference
            var dist = dir.magnitude; // get horizontal direction
            var a = angle * Mathf.Deg2Rad; // Convert angle to radians
            dir.y = dist * Mathf.Tan(a); // set dir to the elevation angle.
            dist += height / Mathf.Tan(a); // Correction for small height differences

            // Calculate the velocity magnitude
            var velocity = Mathf.Sqrt(dist * Physics.gravity.magnitude / Mathf.Sin(2 * a));
            var result = velocity * dir.normalized;
            return result; // Return a normalized vector.
        }
    }
}