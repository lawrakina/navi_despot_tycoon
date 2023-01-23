using EventSystem.Runtime.Core.Managers;
using NavySpade.pj49.Scripts.Productions.Factory;
using UnityEngine;


namespace NavySpade.pj49.Scripts.Productions.Gym{
    public class GymUpgradeZone : MonoBehaviour{
        [SerializeField]
        private Factory.Factory _factory;

        [SerializeField]
        private bool _isUseCertainUpdates;
        [SerializeField]
        private FactoryUpgradeParameter[] _unitsUpgradeParameters;

        private void OnTriggerEnter(Collider other){
            if (other.tag.Equals("Player")){
                FactoryUpgradeData data = new FactoryUpgradeData(){
                    Factory = _factory,
                    UnitsUpgradeParameters = _unitsUpgradeParameters,
                    IsUseCertainUpdates = _isUseCertainUpdates
                };

                EventManager.Invoke(PopupsEnum.OpenFactoryUpgradePopup, data);
            }
        }

        private void OnTriggerExit(Collider other){
            if (other.tag.Equals("Player")){
                EventManager.Invoke(PopupsEnum.CloseUpgradePopup);
            }
        }
    }

    public struct FactoryUpgradeData{
        public Factory.Factory Factory;
        public bool IsUseCertainUpdates;
        public FactoryUpgradeParameter[] UnitsUpgradeParameters;
    }
}