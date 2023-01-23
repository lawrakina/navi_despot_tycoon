using Project19.Weapons.ProjectileEmitWeapon;

namespace Project19.Weapons.CirculImpuls
{
    public class CirculImpulsWeapon : SimpleProjectileWeapon
    {
        protected override StatBase Stat => (AttachedSpell.Data as CirculImpulsData).Stats;
    }
}