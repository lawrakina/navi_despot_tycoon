using Project19.Weapons.Water;

namespace Project19.Weapons.Steam
{
    public class SteamProjectileWeapon : WaterWeapon
    {
        protected override StatBase Stat => (AttachedSpell.Data as SteamData).Stats;
    }
}