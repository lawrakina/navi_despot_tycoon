using Cinemachine;
using UnityEngine;

namespace NavySpade.Misc.Cinemachine
{
    /// <summary>
    /// An add-on module for <see cref="CinemachineVirtualCameraBase"/> that locks the camera's Z co-ordinate
    /// </summary>
    [ExecuteInEditMode]
    [SaveDuringPlay]
    [AddComponentMenu("")] // Hide in menu
    public class LockCameraY : CinemachineExtension
    {
        [Tooltip("Lock the camera's Z position to this value")] [SerializeField]
        private float _yPosition = 10;

        protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam,
            CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
        {
            if (enabled && stage == CinemachineCore.Stage.Body)
            {
                var pos = state.RawPosition;
                pos.y = _yPosition;
                state.RawPosition = pos;
            }
        }
    }
}