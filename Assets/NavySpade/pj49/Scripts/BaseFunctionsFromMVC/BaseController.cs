using UniRx;


namespace NavySpade.pj49.Scripts.BaseFunctionsFromMVC{
    public abstract class BaseController{
        protected CompositeDisposable _subscriptions = new CompositeDisposable();

        ~BaseController(){
            _subscriptions?.Dispose();
        }
    }
}