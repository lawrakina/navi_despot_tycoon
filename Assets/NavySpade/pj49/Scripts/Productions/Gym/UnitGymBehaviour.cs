using System;
using System.Collections;
using NavySpade.pj49.Scripts.Productions.ProductionStates;
using UnityEngine;


namespace NavySpade.pj49.Scripts.Productions.Gym{
    internal class UnitGymBehaviour : MonoBehaviour {
        [SerializeField] private CollectingUnit _unit;
        [SerializeField] private Animator _animator;
        [SerializeField] private UnitAnimator _unitAnimator;
        [SerializeField] private AnimationEventsHandler _animationEvents;
        [SerializeField] private float _jumpSpeed;

        private GymLogic _root;
        private FactoryStates _currentState;

        private readonly int _jumpTrigger = Animator.StringToHash("Jump");
        private readonly int IsMove = Animator.StringToHash("Running State");
        private readonly int _jumpProgress = Animator.StringToHash("JumpProgress");
        private readonly int _staffPickUp = Animator.StringToHash("StaffPickUp");
        private readonly int _swing = Animator.StringToHash("Swing");

        public FactoryStates State => _currentState;
        
        public event Action<UnitGymBehaviour> StateChanged; 
        
        public void Init(GymLogic stateMachine)
        {
            _root = stateMachine;
        }
        
        public void SetState(FactoryStates state)
        {
            _currentState = state;
            
            switch (state)
            {
                case FactoryStates.MoveToProductionPoint:
                    var equipment = _root.GetPositionOnBarbells(_unit);
                    _unit.StartMoveToPoint(equipment.studyPoint.position,() =>
                    {
                        SetState(FactoryStates.Producing);
                        StartCoroutine(StartTraining(_unit,equipment));
                        // StartCoroutine(PlaceUnitInBarbells());
                    });
                    // _unit.StartMoveToPath(_root.GetAiPoint(), () =>
                    // {
                    // StartCoroutine(PlaceUnitInBarbells());
                    // });
                    StateChanged?.Invoke(this);
                    break;
                case FactoryStates.Producing:
                    StartCoroutine(StopTraining(_unit));
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

        private IEnumerator StopTraining(CollectingUnit unit){
            _animator.SetBool(_swing, false);
            yield return null;
        }

        private IEnumerator StartTraining(CollectingUnit unit, (Transform studyPoint, Transform barbells) equipment){
            transform.LookAt(equipment.barbells);
            _animator.SetTrigger(_staffPickUp);
            float progress = 0;
            while (progress < 1){
                progress += Time.deltaTime;
                yield return null;
                _animator.SetBool(_swing, true);
                transform.LookAt(equipment.studyPoint);
            }
        }

        private IEnumerator InvokeDestroy()
        {
            yield return new WaitForSeconds(_root.ResourceLifeTime * _unit.UnitResource.Multiplier);
            SetState(FactoryStates.Destroying);
        }

        // private IEnumerator PlaceUnitInBarbells()
        // {
        //     Transform targetPoint = _root.GetPositionOnBarbells();
        //     _unitAnimator.enabled = false;
        //     _animator.SetTrigger(_jumpTrigger);
        //     _animator.SetInteger(IsMove, 3);
        //     float progress = 0;
        //     while (progress < 1)
        //     {
        //         progress += Time.deltaTime * _jumpSpeed;
        //         transform.position = Vector3.Slerp(transform.position, targetPoint.position, progress);
        //         transform.rotation = Quaternion.RotateTowards(
        //             transform.rotation, 
        //             targetPoint.rotation,
        //             Time.deltaTime * 100);
        //         
        //         _animator.SetFloat(_jumpProgress, progress);
        //         yield return null;
        //     }
        //     
        //     transform.rotation = Quaternion.LookRotation(targetPoint.forward);
        //     _animator.SetInteger(IsMove, 2);
        //     SetState(FactoryStates.Producing);
        // }
        public bool Compare(CollectingUnit unit){
            return ReferenceEquals(unit, _unit);
        }
    }
}