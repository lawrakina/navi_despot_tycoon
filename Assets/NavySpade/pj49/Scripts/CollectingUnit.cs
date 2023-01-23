using System;
using System.Linq;
using NavySpade.pj49.Scripts.Items;
using NavySpade.pj49.Scripts.Items.Inventory;
using NavySpade.pj49.Scripts.Navigation;
using NavySpade.pj49.Scripts.UnitsQueues;
using UnityEngine;
using UnityEngine.Events;

namespace NavySpade.pj49.Scripts
{
    public class CollectingUnit : MonoBehaviour
    {
        [Serializable]
        class Events
        {
            public UnityEvent OnPickuped;
        }
        
        [field: SerializeField] public ResourceAsset UnitResource { get; private set; }
        
        public MoveToPlayer MovementToPlayer;
        public AINavPointMovement MovementToPath;
        public MoveToPoint MovementToPoint;

        [SerializeField] private Events _callbacks;

        private bool _blockCollect;

        public bool BlockCollect
        {
            get => _blockCollect;
            set
            {
                _blockCollect = value;
                GetComponent<UnitChain>().enabled = _blockCollect == false;
                GetComponent<Rigidbody>().isKinematic = _blockCollect;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if(BlockCollect)
                return;
            
            var queues = other.GetComponents<IUnitMovementQueue>();
            
            if(queues == null || queues.Length <= 0)
                return;
            
            var queue = queues.FirstOrDefault(c => c.IsEnable);
            
            if(queue == null)
                return;
            
            var squad = other.GetComponent<UnitSquad>();
            TryConnectToSquad(squad, queue, true);
        }

        public void StartMoveToPlayer()
        {
            MovementToPoint.enabled = false;
            MovementToPlayer.enabled = true;
            MovementToPath.enabled = false;
        }
        
        public void TryConnectToSquad(UnitSquad squad, IUnitMovementQueue queue, bool addInInventory)
        {
            if (squad.TryAddUnit(this, addInInventory))
            {
                BlockCollect = false;
                StartMoveToPlayer();
                MovementToPlayer.Init(queue, squad.Units.Count);
                MovementToPath.enabled = false;
                _callbacks.OnPickuped.Invoke();
            }
        }
        
        public void StartMoveToPoint(Vector3 point, Action reachedCallback)
        {
            MovementToPoint.enabled = true;
            MovementToPoint.StartMoveToPoint(point, 5, reachedCallback);
            MovementToPath.enabled = false;
            MovementToPlayer.enabled = false;
        }

        public void StartMoveToPath(AIPoint point, Action reachedCallback)
        {
            MovementToPoint.enabled = false;
            MovementToPlayer.enabled = false;
            MovementToPath.enabled = true;
            MovementToPath.MoveToAiPoint(point, reachedCallback);
        }

        public void DestroySelf(){
            if(gameObject)
                Destroy(gameObject);
        }
    }
}