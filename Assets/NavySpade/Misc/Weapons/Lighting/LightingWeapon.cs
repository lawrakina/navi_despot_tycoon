using Project19.Weapons.ProjectileEmitWeapon;

namespace Project19.Weapons.Lighting
{
    public class LightingWeapon : SimpleProjectileWeapon
    {
        protected override StatBase Stat => (AttachedSpell.Data as LightingData).Stats;
        
        protected LightingStat LightingStat => Stat as LightingStat;

        protected override void OnEmitBullet(SimpleProjectile projectile)
        {
            base.OnEmitBullet(projectile);
            var lightingProjectile = projectile as LightingProjectile;
            
            lightingProjectile.InitLighting(LightingStat.RicochetCount);
        }
    }
}