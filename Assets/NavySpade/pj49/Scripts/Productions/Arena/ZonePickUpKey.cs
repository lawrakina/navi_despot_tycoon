using System;
using NavySpade.Core.Runtime.Player.Logic;
using NavySpade.pj49.Scripts.BaseFunctionsFromMVC;
using UniRx;
using UnityEngine;


namespace NavySpade.pj49.Scripts.Productions.Arena{
    [RequireComponent(typeof(BoxCollider))]
    public class ZonePickUpKey : BaseMonoController{
        [NonSerialized]
        public ReactiveProperty<bool> EnterPlayer = new ReactiveProperty<bool>(false);

        private void OnTriggerEnter(Collider other){
            if (other.gameObject.TryGetComponent<SinglePlayer>(out _)){
                EnterPlayer.Value = true;
            }
        }
    }
}