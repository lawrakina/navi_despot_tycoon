using System;
using EventSystem.Runtime.Core.Managers;
using NavySpade.Core.Runtime.Player.Logic;
using NavySpade.pj49.Scripts.Productions.Gym;
using UnityEngine;

namespace NavySpade.pj49.Scripts.Productions.Factory
{
    public class FactoryUpgradeZone : MonoBehaviour
    {
        [SerializeField] private Factory _factory;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag.Equals("Player"))
            {
                EventManager.Invoke(PopupsEnum.OpenFactoryUpgradePopup, new FactoryUpgradeData{Factory = _factory} );
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag.Equals("Player"))
            {
                EventManager.Invoke(PopupsEnum.CloseUpgradePopup);
            }
        }
    }
}