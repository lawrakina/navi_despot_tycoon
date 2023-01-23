using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Project19.Weapons.LightingAirStrike
{
    public class LightingAirStrikeWeapon : WeaponBase
    {
        [SerializeField] private LightingAirStrikeProjectile _projectile;
        
        private LightingAirStrikeData _data => AttachedSpell.Data as LightingAirStrikeData;
        
        private Coroutine _coroutine;
        private int _remainAmmo;

        public override StatBase BaseStats => _data.Stats;
        protected override Func<int> SetStartBulletCount => () => 1;

        protected override void OnStartShot()
        {
            print("StartShot");
            _coroutine = StartCoroutine(ShotDelay());
        }

        protected  override void OnEndShot()
        {
            if(_coroutine != null)
                StopCoroutine(_coroutine);
        }

        protected override void OnStartUse_Internal()
        {
            print("startUse");
            
            OnBulletCountChange += BullectCountChange;
            _remainAmmo = _data.ShotsCount;
        }

        private void BullectCountChange(int value)
        {
            if (value != 0)
            {
                _remainAmmo = _data.ShotsCount;
            }
        }

        public override void OnEndUse()
        {
            
        }


        private IEnumerator ShotDelay()
        {
            while (_remainAmmo > 0)
            {
                yield return new WaitForSeconds(_data.Stats.ShotDelay);

                CreateStrike();

                _remainAmmo--;
            }

            if (_remainAmmo <= 0)
                BulletCount = 0;
        }

        private void CreateStrike()
        {
            var rnd = Random.insideUnitCircle * _data.Stats.Distance;
            var pos = new Vector3(rnd.x, 0, rnd.y) + transform.position;

            var instance = Instantiate(_projectile);
            instance.transform.position = pos;
            
            instance.Damage = _data.Stats.Damage;
            instance.Lifetime = _data.ShotLifetime;
            instance.Team = CurrentTeam;
        }
    }
}