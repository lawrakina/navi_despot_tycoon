using UnityEngine;


namespace NavySpade.pj49.Scripts.Productions.Arena{
    public class KeyView : MonoBehaviour{
        private bool isMoved;
        private Transform _target;

        [SerializeField]
        private Transform _key;

        public Transform Target{
            set{
                _target = value;
                isMoved = true;
            }
        }

        private void Update(){
            if (isMoved){
                var targetPosition = Vector3.Lerp(transform.position, _target.position, Time.deltaTime);
                transform.position = new Vector3(targetPosition.x, targetPosition.y + 1, targetPosition.z);
            }
        }

        public void DefaultRotate(float yRotation){
            _key.rotation = Quaternion.Euler(-90f, yRotation, 0);
        }
    }
}