using UniRx;
using UnityEngine;


namespace NavySpade.pj49.Scripts.BaseFunctionsFromMVC{
    public abstract class BaseMonoController: MonoBehaviour{
        protected CompositeDisposable _subscriptions = new CompositeDisposable();

        ~BaseMonoController(){
            _subscriptions?.Dispose();
        }
    }
}