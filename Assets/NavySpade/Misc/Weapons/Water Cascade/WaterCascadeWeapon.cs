using Project19.Weapons.ProjectileEmitWeapon;
using UnityEngine;

namespace Project19.Weapons.Water_Cascade
{
    public class WaterCascadeWeapon : SimpleProjectileWeapon
    {
        [SerializeField] private SimpleProjectileWeapon[] weapons;

        private SpellData _waterSpell;

        protected override StatBase Stat => WaterCascadeData.Stats;

        protected WaterCascadeData WaterCascadeData => AttachedSpell.Data as WaterCascadeData;


        protected override void OnStartUse_Internal()
        {
            base.OnStartUse_Internal();
            foreach (var weapon in weapons)
            {
                weapon.Init(AttachedDamageable, AttachedSpell);

                weapon.IsAutoShot = false;
                weapon.UseCustomTearLevel = true;
                weapon.CustomTearLevel = 0;
            }
        }

        public override void Shot(Vector2 direction, Transform target)
        {
            var deltaAngle = 0f;

            foreach (var weapon in weapons)
            {
                var euler = Quaternion.Euler(0, deltaAngle, 0);
                var dir = euler * Vector3.forward;
                var dir2d = new Vector2(dir.x, dir.z);

                weapon.Shot(dir2d, target);

                deltaAngle += 360f / weapons.Length;
            }

            BulletCount -= 1;
        }
    }
}