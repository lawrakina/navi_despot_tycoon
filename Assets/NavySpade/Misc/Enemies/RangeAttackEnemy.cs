using Core.Damagables;
using Misc.Enemies;
using Project19.Weapons;
using UnityEngine;

namespace Project19.Enemies
{
    [RequireComponent(typeof(Enemy))]
    [RequireComponent(typeof(Damageable))]
    public class RangeAttackEnemy : MonoBehaviour
    {
        [SerializeField] private Damageable.Team _team;
        
        private WeaponBase _weapon;
        private Damageable _damagable;
        private Enemy _enemy;
        private AIConfig _aiConfig;

        private void Awake()
        {
            _enemy = GetComponent<Enemy>();
            _damagable = GetComponent<Damageable>();
            _aiConfig = AIConfig.Instance;
        }

        private void Start()
        {
            _weapon = Instantiate(_aiConfig.RangeEnemySpell.Data.WeaponBasePrefab, transform);
            
            _weapon.Init(_damagable, _aiConfig.RangeEnemySpell);
            _enemy.Dead += b =>
            {
                if (b == Enemy.DeathState.Destroy)
                    return;

                _weapon.EndShot();
                _weapon.OnEndUse();
            };

            _weapon.OnShot += () => _enemy.OnAttack?.Invoke();
        }
    }
}