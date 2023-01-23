
namespace Project19.Weapons.ProjectileEmitWeapon
{
    public class SimpleProjectileWeapon : ProjectileEmitWeapon<SimpleProjectile, StatBase>
    {
        protected override StatBase Stat => (AttachedSpell.Data as SimpleSpellData).Stats;
        
        protected override void OnEmitBullet(SimpleProjectile projectile)
        {
            projectile.Init(Stat.Damage, true);
        }
    }
}