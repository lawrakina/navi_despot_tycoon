using UnityEngine;


namespace NavySpade.pj49.Scripts.Productions.EnergyFactory{
    [RequireComponent(typeof(Animation))] 
    public class BatteryView : MonoBehaviour{
        [SerializeField]
        private string _animationClipName = "BatterySpawn";
        private Animation _animation;
        public float DurationClip => _animation.GetClip("BatterySpawn").length;

        private void Awake(){
            _animation = GetComponent<Animation>();
        }
    }
}