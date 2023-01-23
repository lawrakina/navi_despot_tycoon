using Core.Damagables;
using UnityEngine;

namespace Misc.Damagables
{
    public class DamagableKiller : MonoBehaviour
    {
        [SerializeField] private Damageable[] _damagablesToKill;

        public void Kill()
        {
            foreach (var damagable in _damagablesToKill)
            {
                if(damagable != null)
                    damagable.DealDamage(int.MaxValue, Damageable.Team.Neitral);
            }
        }
    }
}