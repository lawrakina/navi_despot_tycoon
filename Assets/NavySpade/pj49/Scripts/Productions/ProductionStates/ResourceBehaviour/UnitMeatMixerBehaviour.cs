using System;
using NavySpade.pj49.Scripts.Items.Inventory;
using UnityEngine;
using UnityEngine.Timeline;

namespace NavySpade.pj49.Scripts.Productions.ProductionStates
{
    public class UnitMeatMixerBehaviour : MonoBehaviour
    {
        [SerializeField] private CollectingUnit _unit;
        [SerializeField] private Animator _animator;
        [SerializeField] private AnimationEventsHandler _animationEvents;

        private FactoryStateMachine _root;
        private FactoryStates _currentState;

        private int _jumpTrigger = Animator.StringToHash("JumpInMixer");

        public FactoryStates State => _currentState;
        
        public ResourceAsset ResourceAsset => _unit.UnitResource;

        public event Action<UnitMeatMixerBehaviour> StateChanged; 
        
        public void Init(FactoryStateMachine stateMachine)
        {
            _root = stateMachine;
        }
        
        public void SetState(FactoryStates state)
        {
            switch (state)
            {
                case FactoryStates.MoveToProductionPoint:
                    _unit.StartMoveToPath(_root.EnterPoint, () =>
                    {
                        SetState(FactoryStates.Producing);
                    });
                    break;
                case FactoryStates.Producing:
                    //_animator.Play(_jumpAnim);
                    _animator.SetTrigger(_jumpTrigger);
                    _animationEvents.JumpTrigger += JumpTrigger;
                    break;
                case FactoryStates.Destroying:
                    StateChanged?.Invoke(this);
                    Destroy(gameObject);
                    break;
                case FactoryStates.Finish:
                    break;
            }

            if (_currentState != state)
            {
                _currentState = state;
                StateChanged?.Invoke(this);
            }
        }

        private void JumpTrigger()
        {
            _animationEvents.JumpTrigger -= JumpTrigger;
            SetState(FactoryStates.Destroying);
        }
    }
}