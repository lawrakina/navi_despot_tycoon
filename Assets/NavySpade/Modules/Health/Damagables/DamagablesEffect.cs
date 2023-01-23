using UnityEngine;

namespace Core.Damagables
{
    public abstract class DamagablesEffect : MonoBehaviour
    {
        public abstract void TakeDamage(int count, Damageable.Team team, IDamageParameter[] damageParameters);
    }
}