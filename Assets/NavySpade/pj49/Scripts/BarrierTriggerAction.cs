using System;
using UnityEngine;
using UnityEngine.Events;

namespace NavySpade.pj49.Scripts
{
    public class BarrierTriggerAction : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        private readonly int _isOpenHash = Animator.StringToHash("IsOpen");
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag.Equals("Player"))
            {
                _animator.SetBool(_isOpenHash, true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag.Equals("Player"))
            {
                _animator.SetBool(_isOpenHash, false);
            }
        }
    }
}