using System;
using System.Collections;
using Core.Damagables;
using Core.Player;
using JetBrains.Annotations;
using NavySpade.Core.Runtime.Player.Logic;
using NavySpade.Modules.Extensions.UnityTypes;
using UnityEngine;
using UnityEngine.AI;

namespace Misc.Enemies
{
    [RequireComponent(typeof(Enemy))]
    [RequireComponent(typeof(Rigidbody))]
    public class AIFollowPlayer : MonoBehaviour
    {
        public enum TargetType
        {
            Player,
            NearestEnemy
        }

        [SerializeField] private TargetType _target;
        [SerializeField] private Damageable.Team _team = Damageable.Team.Enemy;
        [SerializeField] private float _movementSpeed;

        [Header("Nav mesh settings")] [SerializeField]
        private float _recalulatePathTime;

        [SerializeField] private float _nextPathToleraste = .1f;

        private Rigidbody _rb;
        private NavMeshAgent _agent;

        private NavMeshPath _path;
        private int _indexInPath;

        private Coroutine _recalculateRoutine;

        private void Awake()
        {
            _path = new NavMeshPath();

            _agent = GetComponent<NavMeshAgent>();
            _rb = GetComponent<Rigidbody>();

            _agent.enabled = false;
            _agent.updatePosition = false;
            _agent.updateRotation = false;
            _agent.updateUpAxis = false;
        }

        private void OnEnable()
        {
            StartFollowing();
        }

        private void OnDisable()
        {
            StopFollowing();
        }

        public void StartFollowing()
        {
            if (_agent == null)
                Awake();

            _agent.enabled = true;
            Calculate();
            _recalculateRoutine = StartCoroutine(CalculatePath());
        }

        public void StopFollowing()
        {
            if (_recalculateRoutine != null)
            {
                StopCoroutine(_recalculateRoutine);
                _recalculateRoutine = null;
            }
        }

        private IEnumerator CalculatePath()
        {
            while (true)
            {
                yield return new WaitForSeconds(_recalulatePathTime);
                Calculate();
            }
        }

        private void Calculate()
        {
            var target = GetTarget();

            if (target == null)
                return;

            _agent.nextPosition = _rb.position;
            _agent.CalculatePath(target.position, _path);
            _indexInPath = 0;
        }

        private void FixedUpdate()
        {
            //TODO: Game.IsPlay? how todo it??
            //if (GameLogic.IsPlay == false)
            //    return;

            LookAtPlayer();

            if (_recalculateRoutine == null)
                return;

            if (_indexInPath >= _path.corners.Length)
                return;

            var point = _path.corners[_indexInPath];
            point.y = transform.position.y;

            var dir = point - transform.position;
            dir.Normalize();

            _rb.position += dir * (_movementSpeed * Time.fixedDeltaTime);

            if (Vector3.Distance(point, transform.position) < _nextPathToleraste)
            {
                _indexInPath++;
            }
        }

        private void LookAtPlayer()
        {
            var target = GetTarget();

            if (target == null)
                return;

            var from = target.position;
            var to = transform.position;
            to.y = 0;
            from.y = 0;

            var dir = from - to;

            if (dir != Vector3.zero)
                _rb.rotation = Quaternion.LookRotation(dir);
        }

        [CanBeNull]
        private Transform GetTarget()
        {
            switch (_target)
            {
                case TargetType.Player:
                {
                    var p = SinglePlayer.Instance;
                    return p == null ? null : p.transform;
                }
                case TargetType.NearestEnemy:
                {
                    var dt = TransformExtensions.FindClosed(Damageable.GetAllEnemysOfTeam(_team), transform.position);
                    return dt == null ? null : dt.transform;
                }
                default:
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}