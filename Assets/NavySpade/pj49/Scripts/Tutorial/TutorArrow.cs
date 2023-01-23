using UnityEngine;

namespace NavySpade.pj49.Scripts.Tutorial
{
    public class TutorArrow : MonoBehaviour
    {
        public Transform Target { get; set; }

        private void Update()
        {
            if(Target == null)
                return;

            transform.rotation = Quaternion.LookRotation(Target.position - transform.position);
        }
    }
}