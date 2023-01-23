using Misc.Enemies;
using UnityEngine;

namespace Project19.Enemies
{
    [RequireComponent(typeof(Enemy))]
    public class EnemyTowerAnimator : MonoBehaviour
    {
        private Enemy _enemy;
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _enemy = GetComponent<Enemy>();
        }

        private void Start()
        {
            _animator.enabled = false;
        }

        private void OnEnable()
        {
            _enemy.Dead += OnDead;
        }

        private void OnDead(Enemy.DeathState deathState)
        {
            _animator.enabled = true;
            _animator.Play("Lose");
        }
    }
}