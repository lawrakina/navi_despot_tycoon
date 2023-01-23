using EventSystem.Runtime.Core.Managers;
using Mono.CSharp;
using UnityEngine;

namespace NavySpade.pj49.Scripts.Zone
{
    public class WinZone : MonoBehaviour
    {
        private bool _isWon;
        
        private void OnTriggerEnter(Collider other)
        {
            if (_isWon == false)
            {
                EventManager.Invoke(GameStatesEM.OnWin);
                _isWon = true;
            }
            
           
        }
    }
}