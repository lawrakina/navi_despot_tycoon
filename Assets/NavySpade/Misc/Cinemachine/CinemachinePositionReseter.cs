using Cinemachine;
using UnityEngine;

namespace NavySpade.Misc.Cinemachine
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class CinemachinePositionReseter : MonoBehaviour
    {
        private Vector3 _startPos;
        private CinemachineVirtualCamera _camera;
        private CinemachineFramingTransposer _cinemachineFramingTransposer;

        private Quaternion _startRotation;

        private void Awake()
        {
            _camera = GetComponent<CinemachineVirtualCamera>();
            _cinemachineFramingTransposer = _camera.GetCinemachineComponent<CinemachineFramingTransposer>();
        }

        private void Start()
        {
            _startPos = transform.position;
            _startRotation = transform.rotation;

            //GameLogic.Instance.LevelLoaded += ResetPosition;
        }

        private void OnDisable()
        {
            //GameLogic.Instance.LevelLoaded -= ResetPosition;
        }

        public void ResetPosition()
        {
            var a = _startPos;
            var b = transform.position;
            //for correct top-down calculation
            a.y = 0;
            b.y = 0;

            var delta = a - b;
            transform.rotation = _startRotation;

            _cinemachineFramingTransposer.OnTargetObjectWarped(_camera.Follow, delta);
        }
    }
}