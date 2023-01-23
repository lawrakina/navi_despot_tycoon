using System;
using UnityEngine;


namespace NavySpade.pj49.Scripts.Productions.Arena{
    public class FlyingBullet : MonoBehaviour{
        private bool _go = false;
        private Vector3 _targetPos;
        private float _startTime;
        private float _endLength;
        public CollectingUnit Owner{ get; set; }
        public int AttackValue => !Owner ? 1 : Owner.UnitResource.Multiplier;
        public event Action<FlyingBullet> OnCollision = (x) => { };

        public void ThrowTo(Vector3 endPos){
            _go = true;
            _targetPos = endPos;
            _startTime = Time.time;
            _endLength = Vector3.Distance(transform.position, endPos);
        }

        private void Update(){
            if (!_go) return;
            if (Vector3.Distance(transform.position, _targetPos) > 0.1f){
                float time = (Time.time - _startTime) * 1;
                float deltaWay = time / _endLength;
                transform.position = Vector3.Lerp(transform.position, _targetPos, deltaWay);
            } else{
                OnCollision?.Invoke(this);
                Destroy(gameObject);
            }
        }
    }
}