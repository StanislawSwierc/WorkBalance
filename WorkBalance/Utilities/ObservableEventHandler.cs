using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reactive.Disposables;
using System.Reactive;

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

}
