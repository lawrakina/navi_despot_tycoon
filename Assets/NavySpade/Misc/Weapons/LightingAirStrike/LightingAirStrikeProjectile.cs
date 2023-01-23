using System.Collections;
using Core.Damagables;
using Misc.Damagables;
using UnityEngine;

namespace Project19.Weapons.LightingAirStrike
{
    public class LightingAirStrikeProjectile : MonoBehaviour
    {
        public float Lifetime;
        public int Damage;
        public Damageable.Team Team;

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(Lifetime);
            Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out Damageable damagable) == false)
                return;

            print("d");

            damagable.DealDamage(Damage, Team);
        }
    }
}