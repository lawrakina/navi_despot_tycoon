using NavySpade.pj49.Scripts.UnitsQueues;
using UnityEngine;

namespace NavySpade.pj49.Scripts
{
    [RequireComponent(typeof(Rigidbody))]
    public class MoveToPlayer : MonoBehaviour
    {
        [field: SerializeField] public float MovementSpeed { get; private set; }

        [field: SerializeField] public float LookAtSpeed { get; private set; }

        public IUnitMovementQueue Queue { get; private set; }
        
        public int IndexInQueue { get; private set; }

        private Vector3 _previewFramePos;
        private Rigidbody _rigidbody;
        private bool _isReachedTarget;
        private UnitSquad _unitSquad;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            _previewFramePos = transform.position;
        }

        public void Init(IUnitMovementQueue queue, int index)
        {
            Queue = queue;
            IndexInQueue = index;
            _unitSquad = UnitSquad.Instance;
        }

        public void UpdateQueuePosition(int index)
        {
            IndexInQueue = index;
        }

        private void LateUpdate()
        {
            if (Queue == null)
                return;

            var currentPos = transform.position;
            var targetPos = Queue.GetPosition(IndexInQueue);
            
            var newPos = Vector3.MoveTowards(
                currentPos, 
                targetPos, 
                MovementSpeed * Time.deltaTime);

            transform.position = newPos;

            var dir = newPos - currentPos;
            dir.Normalize();

            _dir = dir;
            if (Vector3.Distance(targetPos, currentPos) < .1 && _isReachedTarget == false)
            {
                _unitSquad.InvokeSetsInSquad();
                _isReachedTarget = true;
            }

            _rigidbody.angularVelocity = Vector3.up * (Vector3.Dot(transform.right, dir) * LookAtSpeed);
        }

        private Vector3 _dir;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, _dir);
        }
    }
}