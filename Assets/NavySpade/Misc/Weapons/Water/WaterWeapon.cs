using Project19.Weapons.ProjectileEmitWeapon;

namespace Project19.Weapons.Water
{
    public class WaterWeapon : SimpleProjectileWeapon
    {
        protected override StatBase Stat => (AttachedSpell.Data as WaterData).Stats;
        protected WaterStat SteamStat => Stat as WaterStat;


        protected override void OnEmitBullet(SimpleProjectile projectile)
        {
            base.OnEmitBullet(projectile);
            var steamProjectile = projectile as WaterProjectile;
            
            steamProjectile.InitWater(SteamStat);
        }

        public override void AddSynergySpell(PickupType type)
        {
            
        }
    }
}