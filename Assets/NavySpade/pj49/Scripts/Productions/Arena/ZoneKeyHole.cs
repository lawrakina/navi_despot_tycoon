using NavySpade.Core.Runtime.Player.Logic;
using UnityEngine;


namespace NavySpade.pj49.Scripts.Productions.Arena{
    [RequireComponent(typeof(BoxCollider))]
    public class ZoneKeyHole : MonoBehaviour{
        [SerializeField]
        private Transform _keyhole;

        private void OnTriggerEnter(Collider other){
            if (other.TryGetComponent(out SinglePlayer _)){
                FindObjectOfType<KeyController>().SetTarget(_keyhole, Vector3.zero, KeyStage.Keyhole,transform.rotation.eulerAngles.y);
            }
        }
    }
}