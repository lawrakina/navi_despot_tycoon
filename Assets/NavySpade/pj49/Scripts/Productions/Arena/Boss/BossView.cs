using System;
using UnityEngine;


namespace NavySpade.pj49.Scripts.Productions.Arena.Boss{
    public class BossView : MonoBehaviour{
        private Animator _animator;

        private readonly int _attack = Animator.StringToHash("Attack");
        private readonly int _idle = Animator.StringToHash("Idle");
        private readonly int _die = Animator.StringToHash("Die");
        private readonly int _hit = Animator.StringToHash("Hit");
        private readonly int _hitParam = Animator.StringToHash("HitParam");
        public float HitParam{
            set => _animator.SetFloat(_hitParam, value);
        }

        private void Awake(){
            _animator = GetComponentInChildren<Animator>();
        }

        public void SetState(BossState state){
            switch (state){
                case BossState.Idle:
                    _animator.SetTrigger(_idle);
                    HitFlag(false);
                    break;
                case BossState.Dead:
                    _animator.SetTrigger(_die);
                    HitFlag(false);
                    break;
                case BossState.Hit:
                    HitFlag(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        public void HitFlag(bool state){
            _animator.SetBool(_hit, state);
        }
    }
}