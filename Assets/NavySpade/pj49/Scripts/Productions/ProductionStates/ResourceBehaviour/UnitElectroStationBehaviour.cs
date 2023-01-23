using System;
using System.Collections;
using NavySpade.pj49.Scripts.Productions.EnergyFactory;
using Unity.Mathematics;
using UnityEngine;

namespace NavySpade.pj49.Scripts.Productions.ProductionStates
{
    public class UnitElectroStationBehaviour : MonoBehaviour
    {
        [SerializeField] private CollectingUnit _unit;
        [SerializeField] private Animator _animator;
        [SerializeField] private UnitAnimator _unitAnimator;
        [SerializeField] private AnimationEventsHandler _animationEvents;
        [SerializeField] private float _jumpSpeed;

        private ElectroStationStateMachine _root;
        private FactoryStates _currentState;

        private readonly int _jumpTrigger = Animator.StringToHash("Jump");
        private readonly int IsMove = Animator.StringToHash("Running State");
        private readonly int _jumpProgress = Animator.StringToHash("JumpProgress");

        public FactoryStates State => _currentState;
        
        public event Action<UnitElectroStationBehaviour> StateChanged; 
        
        public void Init(ElectroStationStateMachine stateMachine)
        {
            _root = stateMachine;
        }
        
        public void SetState(FactoryStates state)
        {
            _currentState = state;
            
            switch (state)
            {
                case FactoryStates.MoveToProductionPoint:
                    _unit.StartMoveToPath(_root.EnterPoint, () =>
                    {
                        StartCoroutine(PlaceUnitInCircle());
                    });
                    StateChanged?.Invoke(this);
                    break;
                case FactoryStates.Producing:
                    StateChanged?.Invoke(this);
                    StartCoroutine(InvokeDestroy());
                    break;
                case FactoryStates.Destroying:
                    StateChanged?.Invoke(this);
                    Destroy(gameObject);
                    break;
                case FactoryStates.Finish:
                    StateChanged?.Invoke(this);
                    break;
            }
        }

        private IEnumerator InvokeDestroy()
        {
            yield return new WaitForSeconds(_root.ResourceLifeTime * _unit.UnitResource.Multiplier);
            SetState(FactoryStates.Destroying);
        }

        private IEnumerator PlaceUnitInCircle()
        {
            Transform targetPoint = _root.GetPositionInCircle(this);
            _unitAnimator.enabled = false;
            _animator.SetTrigger(_jumpTrigger);
            _animator.SetInteger(IsMove, 3);
            float progress = 0;
            while (progress < 1)
            {
                progress += Time.deltaTime * _jumpSpeed;
                transform.position = Vector3.Slerp(transform.position, targetPoint.position, progress);
                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation, 
                    targetPoint.rotation,
                    Time.deltaTime * 100);
                
                _animator.SetFloat(_jumpProgress, progress);
                yield return null;
            }
            
            transform.rotation = Quaternion.LookRotation(targetPoint.forward);
            _animator.SetInteger(IsMove, 2);
            SetState(FactoryStates.Producing);
        }
    }
}