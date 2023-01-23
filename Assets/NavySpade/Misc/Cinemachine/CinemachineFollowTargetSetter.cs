using Cinemachine;
using EventSystem.Runtime.Core.Dispose;
using EventSystem.Runtime.Core.Managers;
using UnityEngine;

namespace NavySpade.Misc.Cinemachine
{
    [RequireComponent(typeof(CinemachineVirtualCameraBase))]
    public class CinemachineFollowTargetSetter : MonoBehaviour
    {
        private CinemachineVirtualCameraBase _camera;
        private EventDisposal _disposal;

        private void Awake()
        {
            _disposal = new EventDisposal();
            _camera = GetComponent<CinemachineVirtualCameraBase>();

            EventManager.Add<Transform>(CinemachineEvents.SetCameraFollowTarget, SetTarget).AddTo(_disposal);
        }

        private void OnDestroy()
        {
            _disposal.Dispose();
        }

        private void SetTarget(Transform newTarget)
        {
            _camera.Follow = newTarget;
        }

        public void CopyTarget(CinemachineVirtualCameraBase camera)
        {
            camera.Follow = _camera.Follow;
        }
    }
}