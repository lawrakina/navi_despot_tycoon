using NaughtyAttributes;
using UnityEngine;

namespace NavySpade.pj49.Scripts
{
    public class RandomLocalEulerAngels : MonoBehaviour
    {
        [MinMaxSlider(0, 360)] public Vector2 Angle;

        public Axis EulerAxis;
        
        public enum Axis
        {
            X = 0,
            Y = 1,
            Z = 2
        }

        private void OnEnable()
        {
            var angle = transform.localEulerAngles;

            angle[(int) EulerAxis] = Random.Range(Angle.x, Angle.y);

            transform.localEulerAngles = angle;
        }
    }
}