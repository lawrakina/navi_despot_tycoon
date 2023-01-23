using NavySpade.Core.Runtime.Player.Logic;
using ThirdParty.Joystick_Pack.Scripts.Integration;
using UnityEngine;

namespace NavySpade.pj49.Scripts
{
    public class PlayerMovement : MonoBehaviour
    {
        public Vector2 InputDirection
        {
            get
            {
                if (JoystickInputProvider.Instance == null)
                    return Vector2.zero;

                return JoystickInputProvider.Instance.Value;
            }
        }

        public float Speed;
        public Rigidbody Body;
        private Vector3 _previewFrameMovementDirection;

        private void FixedUpdate()
        {
            if (JoystickInputProvider.Instance == null || SinglePlayer.Instance.Damageable.IsAlive == false)
                return;

            var axis = JoystickInputProvider.Instance.Value;
            var movementDirection = new Vector3(axis.x, 0, axis.y);

            Body.velocity = movementDirection * Speed;

            if (movementDirection != Vector3.zero)
            {
                _previewFrameMovementDirection = movementDirection;
            }
            
            if(_previewFrameMovementDirection != Vector3.zero)
                Body.rotation = Quaternion.LookRotation(_previewFrameMovementDirection);
        }
    }
}