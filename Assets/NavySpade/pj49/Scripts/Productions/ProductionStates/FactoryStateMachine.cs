using NavySpade.pj49.Scripts.Navigation;
using UnityEngine;

namespace NavySpade.pj49.Scripts.Productions.ProductionStates
{
    public class FactoryStateMachine : MonoBehaviour
    {
        [Header("Common")] 
        [SerializeField] protected Factory.Factory _factory;
        [SerializeField] private AIPoint _enterPoint;
        
        public AIPoint EnterPoint => _enterPoint;
    }
}