using System.Collections;
using Core.Damagables;
using Misc.Damagables;
using Project19.Weapons.Water;
using UnityEngine;

namespace Project19.Weapons.Steam
{
    public class SteamProjectile : WaterProjectile
    {
        protected SteamData SteamData => SpellData as SteamData;
        protected SteamStat SteamStat => Stat as SteamStat;

        private Coroutine _isDisposing;

        protected override float MoveTick(float dt, float speed)
        {
            return dt * (1f / SteamStat.Lifetime) * Stat.CircularDistance;
        }


        protected override void OnEnterToTarget(Damageable damageable)
        {
            StartDispose();
            DealDamage(damageable);
        }

        public void StartDispose()
        {
            if(_isDisposing != null)
                return;

            _isDisposing = StartCoroutine(Dispose());
        }

        private IEnumerator Dispose()
        {
            yield return new WaitForSeconds(SteamData.DisposeTime);
            
            Destroy(gameObject);
            _isDisposing = null;
        }
    }
}