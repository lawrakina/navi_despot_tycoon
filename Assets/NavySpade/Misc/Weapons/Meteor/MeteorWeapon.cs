using Project19.Weapons.ProjectileEmitWeapon;

namespace Project19.Weapons.Meteor
{
    public class MeteorWeapon : SimpleProjectileWeapon
    {
        protected override StatBase Stat => (AttachedSpell.Data as MeteorData).Stats;

        protected override void OnEmitBullet(SimpleProjectile projectile)
        {
            base.OnEmitBullet(projectile);

            var castedProjectile = projectile as MeteorProjectile;
            var data = AttachedSpell.Data as MeteorData;
            var MeteorStat = Stat as MeteorStats;

            castedProjectile.InitMeteor(MeteorStat.Angle);
        }
    }
}