using Core.Damagables;
using Misc.Damagables;
using Misc.Damagables.Effects;
using Project19.Weapons.ProjectileEmitWeapon;

namespace Project19.Weapons.Water
{
    public class WaterProjectile : SimpleProjectile
    {
        protected WaterStat WaterStat { get; private set; }

        public void InitWater(WaterStat data)
        {
            WaterStat = data;
        }

        protected override void DealDamage(Damageable damageable)
        {
            base.DealDamage(damageable);
            Repulsion(damageable);
        }

        protected void Repulsion(Damageable damageable)
        {
            var from = damageable.transform.position;
            var to = transform.position;
            from.y = 0;
            to.y = 0;
            
            var dir = from - to;
            dir.Normalize();

            damageable.DealDamage(0, Damageable.Team.Neitral, new RepulsionParameter(dir, WaterStat.PunchForce));
        }
    }
}