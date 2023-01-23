using Cinemachine;
using NavySpade.Core.Runtime.Player.Logic;
using UnityEngine;

namespace NavySpade.pj49.Scripts.Shelter
{
    public class PlayerLairEnterEffect : MonoBehaviour
    {
        public float _multiplyCameraOffset;
        
        private CinemachineVirtualCamera _cm;
        private float _cmNormalDistance;
        private SinglePlayer _collidedPlayer;

        private void Awake()
        {
            _cm = Camera.main.GetComponent<CinemachineVirtualCamera>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if(_collidedPlayer != null)
                return;
            
            if (other.TryGetComponent<SinglePlayer>(out var player) == false)
                return;

            _collidedPlayer = player;
            var framingTransposer = _cm.GetCinemachineComponent<CinemachineFramingTransposer>();

            _cmNormalDistance = framingTransposer.m_CameraDistance;
            framingTransposer.m_CameraDistance = _cmNormalDistance * _multiplyCameraOffset;
        }

        private void OnTriggerExit(Collider other)
        {
            if(_collidedPlayer == null)
                return;
            
            if (other.TryGetComponent<SinglePlayer>(out var player) == false)
                return;

            _collidedPlayer = null;
            var framingTransposer = _cm.GetCinemachineComponent<CinemachineFramingTransposer>();

            framingTransposer.m_CameraDistance = _cmNormalDistance;
        }
    }
}