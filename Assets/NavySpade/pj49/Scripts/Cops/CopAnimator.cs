using UnityEngine;

namespace NavySpade.pj49.Scripts.Cops
{
    public class CopAnimator : UnitAnimator
    {
        public Cop Cop;
        private static readonly int IsAttack = Animator.StringToHash("IsAttack");

        private void OnEnable()
        {
            Cop.AttackStateChanged += CopOnAttackStateChanged;
        }

        private void OnDisable()
        {
            Cop.AttackStateChanged -= CopOnAttackStateChanged;
        }
        
        private void CopOnAttackStateChanged(bool isAttack)
        {
            Animator.SetBool(IsAttack, isAttack);
        }

        //invoke from animator
        public void A_AttackPerformed()
        {
            Cop.OnAttackPerformed();
        }
    }
}