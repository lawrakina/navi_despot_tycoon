using ThirdParty.Joystick_Pack.Scripts.Integration;
using UnityEngine;


namespace NavySpade.pj49.Scripts.Productions.Arena{
    public class JoystickSettings : MonoBehaviour
    {
        public bool IsShow;
        public bool IsWork;
        public bool IsResetJoystickPos;

        private void OnEnable()
        {
            JoystickInputProvider.Instance.GetComponent<CanvasGroup>().alpha = IsShow ? 1 : 0;
            JoystickInputProvider.Instance.enabled = IsWork;
            
            if(IsResetJoystickPos)
                JoystickInputProvider.Instance.ResetPos();
        }
        
        private void OnDisable()
        {
            JoystickInputProvider.Instance.GetComponent<CanvasGroup>().alpha = 1;
            JoystickInputProvider.Instance.enabled = true;
            
            if(IsResetJoystickPos)
                JoystickInputProvider.Instance.ResetPos();
        }
    }
}