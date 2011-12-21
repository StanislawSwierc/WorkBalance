using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reactive.Disposables;
using System.Reactive;
using System.Reactive.Subjects;

namespace WorkBalance.Utilities
{
    public class ObservableEventHandler<TEventArgs> : IObservable<EventPattern<TEventArgs>> where TEventArgs : EventArgs
    {
        private Action<EventPattern<TEventArgs>> m_Subscriptions;

        public void Handler(object sender, TEventArgs e)
        {
            var handler = m_Subscriptions;
            if (handler != null) handler(new EventPattern<TEventArgs>(sender, e));
        }

        public IDisposable Subscribe(IObserver<EventPattern<TEventArgs>> observer)
        {
            m_Subscriptions += observer.OnNext;
            return Disposable.Create(() => m_Subscriptions -= observer.OnNext);
        }
    }

    public class ObservableEventHandler<TSender, TEventArgs> : IObservable<EventPattern<TSender, TEventArgs>> where TEventArgs : EventArgs
    {
        private Action<EventPattern<TSender, TEventArgs>> m_Subscriptions;

        public void Handler(object sender, TEventArgs e)
        {
            var handler = m_Subscriptions;
            if (handler != null) handler(new EventPattern<TSender, TEventArgs>((TSender)sender, e));
        }

        public IDisposable Subscribe(IObserver<EventPattern<TSender, TEventArgs>> observer)
        {
            m_Subscriptions += observer.OnNext;
            return Disposable.Create(() => m_Subscriptions -= observer.OnNext);
        }
    }

    public class EventHandlerSubject<TEventArgs> : 
        ISubject<EventPattern<TEventArgs>>,
        ISubject<EventPattern<TEventArgs>, EventPattern<TEventArgs>>, 
        IObserver<EventPattern<TEventArgs>>, 
        IObservable<EventPattern<TEventArgs>>, 
        IDisposable
        where TEventArgs : System.EventArgs
    {
        private Subject<EventPattern<TEventArgs>> _inner;

        public EventHandlerSubject()
        {
            _inner = new Subject<EventPattern<TEventArgs>>();
        }

        public void Handler(object sender, TEventArgs e)
        {
            _inner.OnNext(new EventPattern<TEventArgs>(sender, e));
        } 

        public void OnCompleted()
        {
            _inner.OnCompleted();
        }

        public void OnError(Exception error)
        {
            _inner.OnError(error);
        }

        public void OnNext(EventPattern<TEventArgs> value)
        {
            _inner.OnNext(value);
        }

        public IDisposable Subscribe(IObserver<EventPattern<TEventArgs>> observer)
        {
            return _inner.Subscribe(observer);
        }

        public void Dispose()
        {
            _inner.Dispose();
        }
    }

}
