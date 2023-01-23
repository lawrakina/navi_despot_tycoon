using System;
using System.Collections;
using System.Collections.Generic;
using Core.Damagables;
using Project19.Enemies;
using UnityEngine;

namespace Misc.Enemies
{
    [RequireComponent(typeof(Damageable))]
    public class Enemy : MonoBehaviour
    {
        public enum DeathState
        {
            Die,
            Destroy
        }
        
        public Action OnAttack;
        public event Action<DeathState> Dead;

        public static List<Enemy> All { get; private set; }

        public bool IsDeadProgress => _isDeadProgress;

        public Damageable Damagable { get; private set; }
        private AIConfig _aiConfig;

        private Collider _collision;
        private Rigidbody _rb;

        private bool _isDeadProgress;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            Damagable = GetComponent<Damageable>();
            _collision = GetComponent<Collider>();

            _aiConfig = AIConfig.Instance;
        }

        private void OnEnable()
        {
            if (All == null)
                All = new List<Enemy>();

            All.Add(this);
            Damagable.OnHPChange += OnHPChange;
        }

        private void OnDisable()
        {
            if (All.Contains(this))
                All.Remove(this);
        }

        private void OnHPChange(int value)
        {
            if (value <= 0)
            {
                Death();
            }
        }

        private IEnumerator AutoDestroyTimer()
        {
            yield return new WaitForSeconds(_aiConfig.CorpseTime);
            Destroy();
        }

        public void Death(bool isAutoDestroy)
        {
            Death();
            
            if (isAutoDestroy)
            {
                StartCoroutine(AutoDestroyTimer());
            }
        }

        private void Death()
        {
            if (IsDeadProgress)
                return;

            DoDead();
        }

        private void DoDead()
        {
            _isDeadProgress = true;
            _collision.enabled = false;

            _rb.constraints = RigidbodyConstraints.FreezeAll;
            
            Dead?.Invoke(DeathState.Die);
            All.Remove(this);
        }

        public void Destroy()
        {
            Destroy(gameObject);
            
            Dead?.Invoke(DeathState.Destroy);

            _isDeadProgress = false;
        }
    }
}