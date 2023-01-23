using Cinemachine;
using EventSystem.Runtime.Core.Dispose;
using EventSystem.Runtime.Core.Managers;
using NavySpade.Core.Runtime.Player.Logic;
using UnityEngine;

namespace NavySpade.Misc.Cinemachine
{
    [RequireComponent(typeof(CinemachineVirtualCameraBase))]
    public class CinemachinePlayerTargetSetter : MonoBehaviour
    {
        private CinemachineVirtualCameraBase _camera;
        private EventDisposal _disposal;
        
        private void Awake()
        {
            _disposal = new EventDisposal();
            _camera = GetComponent<CinemachineVirtualCameraBase>();
            
            EventManager.Add<SinglePlayer>(GenerateEnumEM.SetPlayer, SetPlayer).AddTo(_disposal);
        }

        private void SetPlayer(SinglePlayer player)
        {
            if (player)
            {
                _camera.Follow = player.transform;
            }
        }

        private void OnDestroy()
        {
            _disposal.Dispose();
        }
    }
}