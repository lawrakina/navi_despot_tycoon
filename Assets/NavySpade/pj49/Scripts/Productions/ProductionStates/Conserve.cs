using UnityEngine;

namespace NavySpade.pj49.Scripts.Productions.ProductionStates
{
    public class Conserve : MonoBehaviour
    {
        [SerializeField] private MoveToDestroy _move;

        public MoveToDestroy MoveHandler => _move;
        
        // public void Init()
        // {
        //     _moveToDestroy.transform.position = _conveyorStart.position;
        //     _moveToDestroy.MoveTo(_conveyorEnd.position);
        // }
    }
}