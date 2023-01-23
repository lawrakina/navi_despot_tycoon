using Core.Damagables;
using JetBrains.Annotations;
using Misc.Damagables;
using UnityEngine;

namespace Project19.Weapons
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class ProjectileBase : MonoBehaviour
    {
        public Damageable.Team Team;

        private Rigidbody _rb;
        private StatBase _stat;
        
        public Rigidbody Rb => _rb;

        [CanBeNull]
        public Transform Target { get; private set; }

        public SpellDataBase SpellData { get; private set; }
        public StatBase Stat => _stat;

        public float CoveredDistance => _coveredDistance;
        public float CoverDistanceNormal => _coveredDistance / Stat.CircularDistance;

        private float _coveredDistance;
        private Vector3 _startPos;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            _startPos = transform.position;
        }

        private void FixedUpdate()
        {
            _coveredDistance += MoveTick(Time.fixedDeltaTime, _stat.ShotSpeed);

            var bound = new Bounds(_startPos, new Vector3(Stat.Distance.x, 100, _stat.Distance.y));

            if(bound.Contains(transform.position) == false)
                Destroy(gameObject);
        }

        public void InitBase(Damageable.Team team, StatBase stat, SpellDataBase spell, Transform target)
        {
            SpellData = spell;
            Team = team;
            _stat = stat;
            Target = target;
            OnInitBase();
        }

        protected virtual void OnInitBase()
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="speed"></param>
        /// <returns>пройденная дистанция</returns>
        protected abstract float MoveTick(float dt, float speed);
        protected abstract void OnEnterToTarget(Damageable damageable);

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.TryGetComponent(out Damageable damagable))
                return;
            
            if(Team == Damageable.Team.Neitral)
                OnEnterToTarget(damagable);
            else if(damagable.CurrentTeam != Team)
            {
                OnEnterToTarget(damagable);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            
        }
    }
}