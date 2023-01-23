using System;
using Core.Damagables;
using NaughtyAttributes;
using NavySpade.Core.Runtime.Player.Logic;
using NavySpade.pj49.Scripts.Navigation;
using UnityEngine;
using UnityEngine.Events;

namespace NavySpade.pj49.Scripts.Cops
{
    public class Cop : MonoBehaviour
    {
        [Serializable]
        class Callbacks
        {
            public UnityEvent OnSpoted;
        }
        
        public float SpotRadius;
        public float FollowRadius = 10;
        public float DistanceToAttack;
        public float DistanceToStop;
        [SerializeField] private int _damagePerTick;
        
        public float MoveToPlayerSpeed;
        public float RotationSpeed;
        public Rigidbody Rigidbody;
        
        [MinMaxSlider(-180, 180)] public Vector2 SpotAngle;

        public AINavPointMovement MovementToPath;
        
        [SerializeField] private Callbacks _events;

        [Foldout("Debug")] public bool DrawDebug;
        [Foldout("Debug")] public Color DebugColor;

        public event Action<bool> AttackStateChanged;

        private bool _isSpoted;

        public bool IsSpoted
        {
            get => _isSpoted;
            set
            {
                if (value)
                {
                    _events.OnSpoted.Invoke();
                    MovementToPath.enabled = false;
                }
                else
                {
                    MovementToPath.enabled = true;
                }
                
                _isSpoted = value;
            }
        }

        private void FixedUpdate()
        {
            if (IsSpoted == false)
            {
                if (CheckPlayerContainSpotPos())
                {
                    IsSpoted = true;
                }
                
                return;
            }

            var playerPosition = SinglePlayer.Instance.transform.position;

            if (Vector3.Distance(transform.position, playerPosition) > FollowRadius)
            {
                IsSpoted = false;
                return;
            }

            var distance = Vector3.Distance(transform.position, playerPosition);
            
            AttackStateChanged?.Invoke(distance < DistanceToAttack);
            
            if(distance < DistanceToStop)
                return;

            MoveToPlayer();
        }

        public void OnAttackPerformed()
        {
            SinglePlayer.Instance.Damageable.DealDamage(_damagePerTick, Damageable.Team.Enemy);
        }

        private void MoveToPlayer()
        {
            Rigidbody.position = Vector3.MoveTowards(Rigidbody.position, SinglePlayer.Instance.transform.position, MoveToPlayerSpeed * Time.fixedDeltaTime);
            Rigidbody.rotation = Quaternion.RotateTowards(
                Rigidbody.rotation, 
                Quaternion.LookRotation((SinglePlayer.Instance.transform.position - transform.position).normalized), RotationSpeed * Time.fixedDeltaTime);
        }

        private bool CheckPlayerContainSpotPos()
        {
            var v1 = transform.position;
            var v2 = transform.position + Quaternion.Euler(0, SpotAngle[0], 0) * transform.forward * SpotRadius;
            var v3 = transform.position + Quaternion.Euler(0, SpotAngle[1], 0) * transform.forward * SpotRadius;
            var playerPos3D = SinglePlayer.Instance.transform.position;

            return IsPointInTriangle(
                ToTopDown(playerPos3D),
                ToTopDown(v1),
                ToTopDown(v2),
                ToTopDown(v3));

            Vector2 ToTopDown(Vector3 v) => new Vector2(v.x, v.z);
        }

        bool IsPointInTriangle(Vector2 pt, Vector2 v1, Vector2 v2, Vector2 v3)
        {
            float d1, d2, d3;
            bool has_neg, has_pos;

            d1 = sign(pt, v1, v2);
            d2 = sign(pt, v2, v3);
            d3 = sign(pt, v3, v1);

            has_neg = (d1 < 0) || (d2 < 0) || (d3 < 0);
            has_pos = (d1 > 0) || (d2 > 0) || (d3 > 0);

            return !(has_neg && has_pos);

            float sign(Vector2 p1, Vector2 p2, Vector2 p3)
            {
                return (p1.x - p3.x) * (p2.y - p3.y) - (p2.x - p3.x) * (p1.y - p3.y);
            }
        }

        private void OnDrawGizmos()
        {
            if (DrawDebug == false)
                return;

            Gizmos.color = DebugColor;

            var min = Quaternion.Euler(0, SpotAngle[0], 0) * transform.forward;
            var max = Quaternion.Euler(0, SpotAngle[1], 0) * transform.forward;

            Gizmos.DrawLine(transform.position, transform.position + min * SpotRadius);
            Gizmos.DrawLine(transform.position, transform.position + max * SpotRadius);
            Gizmos.DrawLine(transform.position + min * SpotRadius, transform.position + max * SpotRadius);
        }
    }
}