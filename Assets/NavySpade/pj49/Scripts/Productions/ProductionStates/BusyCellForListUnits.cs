using NavySpade.pj49.Scripts.Navigation;
using UnityEngine;


namespace NavySpade.pj49.Scripts.Productions.ProductionStates{
    internal class BusyCellForListUnits{
        public AIPoint studyPoint{ get; set; }
        public CollectingUnit Unit{ get; set; }
        public Transform BarbellPoint{ get; set; }
    }
}