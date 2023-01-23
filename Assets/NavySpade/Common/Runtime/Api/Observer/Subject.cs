using System;
using System.Collections.Generic;

namespace NavySpade.Common.Runtime.Api.Observer
{
    public class Subject<T> : ISubject
    {
        public event Action<IObserver> Attached;
        public event Action<IObserver> Detached;
        public event Action<IObserver> Notified;

        private readonly List<IObserver> _observers;
        
        public void Register(IObserver observer)
        {
            if (_observers.Contains(observer) == false)
            {
                _observers.Add(observer);
                Attached?.Invoke(observer);
            }
        }

        public void Unregister(IObserver observer)
        {
            if (_observers.Contains(observer))
            {
                _observers.Remove(observer);
                Detached?.Invoke(observer);
            }
        }

        public void NotifyObserver(IObserver observer)
        {
            observer.Notify();
            Notified?.Invoke(observer);
        }

        public Subject()
        {
            _observers = new List<IObserver>();
        }
    }
}