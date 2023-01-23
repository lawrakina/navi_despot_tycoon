using System;
using System.Collections;
using NavySpade.pj49.Scripts.Items.Inventory;
using UnityEngine;


namespace NavySpade.pj49.Scripts.Productions.ProductionStates{
    public class UnitMineBehaviour : MonoBehaviour{
        [SerializeField]
        private CollectingUnit _unit;
        [SerializeField]
        private Animator _animator;
        [SerializeField]
        private UnitAnimator _unitAnimator;
        [SerializeField]
        private AnimationEventsHandler _animationEvents;
        [SerializeField]
        private float _jumpSpeed;
        [SerializeField]
        private AnimationCurve _jumpCurve;
        [SerializeField]
        private GameObject _pickAxe;
        [SerializeField]
        private ParticleSystem _debries;
        
        private MineController _root;
        private FactoryStates _currentState;

        private readonly int _startWork = Animator.StringToHash("WorkMine");

        public FactoryStates State => _currentState;

        public ResourceAsset ResourceAsset => _unit.UnitResource;

        public event Action<UnitMineBehaviour> StateChanged;

        public void Init(MineController controller){
            _root = controller;
        }

        public void SetState(FactoryStates state){
            _currentState = state;

            switch (state){
                case FactoryStates.MoveToProductionPoint:
                    var positions = _root.GetPositions(_unit);
                    _unit.StartMoveToPoint(positions, () => { SetState(FactoryStates.Producing); });
                    StateChanged?.Invoke(this);
                    break;
                case FactoryStates.Producing:
                    StartCoroutine(StartWork(_unit));

                    StateChanged?.Invoke(this);
                    //StartCoroutine(InvokeDestroy());
                    break;
                case FactoryStates.Destroying:
                    StateChanged?.Invoke(this);
                    // Destroy(gameObject);
                    break;
                case FactoryStates.Finish:
                    StateChanged?.Invoke(this);
                    break;
            }
        }

        private IEnumerator StartWork(CollectingUnit unit){
            _pickAxe.SetActive(true);
            StartCoroutine(StartDebries());
            _animator.SetTrigger(_startWork);
            yield return new WaitForSeconds(_root.ProductionTime);
            var trsh = _root.TryGetMeat();
            if (trsh){
                SetState(FactoryStates.Producing);
                _root.WorkCompleted();
                yield break;
            }

            SetState(FactoryStates.Destroying);
            _root.WorkCompleted();
        }

        private IEnumerator StartDebries(){
            while (true){
                yield return new WaitForSeconds(1);
                _debries.Play();
            }
        }
    }
}