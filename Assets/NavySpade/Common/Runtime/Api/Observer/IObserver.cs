using JetBrains.Annotations;

namespace NavySpade.Common.Runtime.Api.Observer
{
    public interface IObserver
    {
        [PublicAPI]
        void Register();
    
        [PublicAPI]
        void Unregister();

        [PublicAPI]
        void Notify();
    }
}
