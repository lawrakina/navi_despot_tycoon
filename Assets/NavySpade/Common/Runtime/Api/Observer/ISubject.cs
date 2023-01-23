using JetBrains.Annotations;

namespace NavySpade.Common.Runtime.Api.Observer
{
    public interface ISubject
    {
        [PublicAPI]
        void Register(IObserver observer);

        [PublicAPI]
        void Unregister(IObserver observer);

        [PublicAPI]
        void NotifyObserver(IObserver observer);
    }
}