using Misc.Enemies;
using Misc.Physic;
using Project19.Enemies;
using UnityEngine;

namespace Project19.Visual.Other
{
    [RequireComponent(typeof(Ragdoll))]
    [RequireComponent(typeof(Animator))]
    public class EnemyAnimator : MonoBehaviour
    {
        public enum AttackType
        {
            Mille = 0,
            Range = 1,
        }

        [SerializeField] private Enemy _enemy;

        public GameObject RangeOfAttack;

        [SerializeField] private AttackType TypeOfAttack = AttackType.Mille;

        private Animator _animator;
        private Ragdoll _ragdoll;


        private static readonly int Attack = Animator.StringToHash("Attack");

        private static readonly int Run = Animator.StringToHash("Run");
        private static readonly int AttackModeKey = Animator.StringToHash("Attack mode");

        private void Awake()
        {
            _ragdoll = GetComponent<Ragdoll>();
            _animator = GetComponent<Animator>();
            _animator.Play("Idle");
        }

        private void Start()
        {
            _enemy.OnAttack += OnAttack;
            _enemy.Dead += EnemyOnDead;

            if (RangeOfAttack != null)
                RangeOfAttack.SetActive(false);

            StartRun();
        }

        private void EnemyOnDead(Enemy.DeathState state)
        {
            if (RangeOfAttack != null)
            {
                RangeOfAttack.SetActive(false);
            }
        }

        private void OnDisable()
        {
            _enemy.OnAttack -= OnAttack;

        }

        public void A_BossStartAttack()
        {
            RangeOfAttack.SetActive(true);
        }

        //using in animator
        public void A_BossAttack()
        {
            
        }

        //using in animator
        public void A_BossEndAttackAnimation()
        {
            RangeOfAttack.SetActive(false);
            StartRun();
        }

        private void StartRun()
        {
            if (TypeOfAttack == AttackType.Range)
                return;

            _animator.SetTrigger(Run);
        }

        private void OnAttack()
        {
            _animator.SetInteger(AttackModeKey, (int) TypeOfAttack);
            _animator.SetTrigger(Attack);
        }
    }
}