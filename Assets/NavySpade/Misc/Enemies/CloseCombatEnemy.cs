using System;
using System.Collections;
using Core.Damagables;
using Core.Player;
using JetBrains.Annotations;
using NavySpade.Core.Runtime.Player.Logic;
using NavySpade.Modules.Extensions.UnityTypes;
using UnityEngine;

namespace Misc.Enemies
{
    [RequireComponent(typeof(Enemy))]
    public class CloseCombatEnemy : MonoBehaviour
    {
        public enum TargetType
        {
            Player,
            NearestEnemy
        }

        [SerializeField] private TargetType _target;
        [SerializeField] private int _damage;
        [SerializeField] private float _delayBetweenAttack = .5f;
        [SerializeField] private float _rangeToAttack;
        
        private Damageable _damagable;
        private Enemy _enemy;

        private bool _isSubscribe;
        
        private void Awake()
        {
            _enemy = GetComponent<Enemy>();
        }

        private void OnEnable()
        {
            StartAttack();
        }

        private void StartAttack()
        {
            StartCoroutine(Attack());
        }

        private IEnumerator Attack()
        {
            while (true)
            {
                yield return new WaitForSeconds(_delayBetweenAttack);

                var target = GetTarget();

                if (target == null)
                {
                    continue;
                }

                if (Vector3.Distance(target.transform.position, transform.position) < _rangeToAttack && _enemy.Damagable.IsAlive)
                {
                    target.DealDamage(_damage, _damagable.CurrentTeam);
                    _enemy.OnAttack?.Invoke();
                    continue;
                }
            }
        }
        
        [CanBeNull]
        private Damageable GetTarget()
        {
            switch (_target)
            {
                case TargetType.Player:
                    var p = SinglePlayer.Instance;
                    
                    if(p == null)
                        return null;

                    return p.Damageable;
                    break;
                case TargetType.NearestEnemy:
                    var dt = TransformExtensions.FindClosed(Damageable.GetAllEnemysOfTeam(_damagable.CurrentTeam), transform.position);
                    
                    if(dt == null)
                        return null;
                    
                    return dt;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(1f, 0f, 0f, 0.2f);
            Gizmos.DrawSphere(transform.position, _rangeToAttack);
        }
    }
}