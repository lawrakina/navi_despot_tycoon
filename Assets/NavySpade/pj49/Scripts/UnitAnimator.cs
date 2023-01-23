using System;
using Core.Damagables;
using JetBrains.Annotations;
using UnityEngine;

namespace NavySpade.pj49.Scripts
{
    public class UnitAnimator : MonoBehaviour
    {
        public Animator Animator;
        [CanBeNull] public Damageable Damageable;

        public float SpeedForMove;
        public float SpeedForRun;

        private Vector3 _previewFramePosition;
        private float _unitsPerSecond;
        private float _currentTime;
        private float _checkTime = 0.05f;
        
        private static readonly int IsMove = Animator.StringToHash("Running State");

        private void Start()
        {
            _previewFramePosition = transform.position;
            
            if(Damageable != null)
                Damageable.OnDeath += DamageableOnOnDeath;
        }

        private void DamageableOnOnDeath()
        {
            Animator.Play("Death");
        }

        private void Update()
        {
            _currentTime += Time.deltaTime;
            if (_currentTime > _checkTime)
            {
                var deltaPos = transform.position - _previewFramePosition;
                var deltaNormalize = deltaPos / Time.deltaTime;
                _unitsPerSecond = deltaNormalize.magnitude;
            
                var state = 0;

                if (_unitsPerSecond > SpeedForMove)
                    state = 1;
                if (_unitsPerSecond > SpeedForRun)
                    state = 2;
                
                Animator.SetInteger(IsMove, state);
                _previewFramePosition = transform.position;
                _currentTime = 0;
            }
        }
    }
}