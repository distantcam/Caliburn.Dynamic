using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using Caliburn.Micro;

namespace Caliburn.Dynamic
{
    /// <summary>
    /// A base implementation of <see cref = "IViewAware" /> which is capable of caching views by context.
    /// </summary>
    public class DynamicViewAware : BindableObject, IViewAware
    {
        readonly IDictionary<object, object> views;

        EventHandler<ViewAttachedEventArgs> viewAttached;

        Subject<ViewAttachedEventArgs> viewAttachedSubject;
        Subject<object> viewLoadedSubject;
        Subject<object> viewReadySubject;

        IObservable<ViewAttachedEventArgs> viewAttachedObservable;
        IObservable<object> viewLoadedObservable;
        IObservable<object> viewReadyObservable;

        /// <summary>
        /// The default view context.
        /// </summary>
        public static readonly object DefaultContext = new object();

        /// <summary>
        /// The view chache for this instance.
        /// </summary>
        protected IDictionary<object, object> Views
        {
            get { return views; }
        }

        /// <summary>
        /// Creates an instance of <see cref="ViewAware"/>.
        /// </summary>
        public DynamicViewAware()
        {
            views = new WeakValueDictionary<object, object>();

            InitializeObservableEvent(ref viewAttachedSubject, ref viewAttachedObservable, args => viewAttached?.Invoke(this, args));
            InitializeObservableEvent(ref viewLoadedSubject, ref viewLoadedObservable, null);
            InitializeObservableEvent(ref viewReadySubject, ref viewReadyObservable, null);
        }

        public IObservable<ViewAttachedEventArgs> ViewAttached => viewAttachedObservable;
        public IObservable<object> ViewLoaded => viewLoadedObservable;
        public IObservable<object> ViewReady => viewReadyObservable;

        internal void InitializeObservableEvent<TArg>(ref Subject<TArg> subject, ref IObservable<TArg> observable, Action<TArg> subscription)
        {
            subject = new Subject<TArg>();
            observable = subject.AsObservable();
            if (subscription != null)
            {
                observable
                    .ObserveOn(Schedulers.MainScheduler)
                    .Subscribe(subscription);
            }
        }

        /// <summary>
        /// Raised when a view is attached.
        /// </summary>
        event EventHandler<ViewAttachedEventArgs> IViewAware.ViewAttached
        {
            add
            {
                EventHandler<ViewAttachedEventArgs> handler2;
                var newEvent = viewAttached;
                do
                {
                    handler2 = newEvent;
                    var handler3 = (EventHandler<ViewAttachedEventArgs>)Delegate.Combine(handler2, value);
                    Interlocked.CompareExchange(ref viewAttached, handler3, handler2);
                } while (newEvent != handler2);
            }
            remove
            {
                EventHandler<ViewAttachedEventArgs> handler2;
                var newEvent = viewAttached;
                do
                {
                    handler2 = newEvent;
                    var handler3 = (EventHandler<ViewAttachedEventArgs>)Delegate.Remove(handler2, value);
                    Interlocked.CompareExchange(ref viewAttached, handler3, handler2);
                } while (newEvent != handler2);
            }
        }

        void IViewAware.AttachView(object view, object context)
        {
            Views[context ?? DefaultContext] = view;

            var nonGeneratedView = PlatformProvider.Current.GetFirstNonGeneratedView(view);
            PlatformProvider.Current.ExecuteOnFirstLoad(nonGeneratedView, OnViewLoaded);
            viewAttachedSubject.OnNext(new ViewAttachedEventArgs { View = view, Context = context });

            var activatable = this as IActivate;
            if (activatable == null || activatable.IsActive)
            {
                PlatformProvider.Current.ExecuteOnLayoutUpdated(nonGeneratedView, OnViewReady);
            }
            else
            {
                AttachViewReadyOnActivated(activatable, nonGeneratedView);
            }
        }

        object IViewAware.GetView(object context)
        {
            object view;
            Views.TryGetValue(context ?? DefaultContext, out view);
            return view;
        }

        static void AttachViewReadyOnActivated(IActivate activatable, object nonGeneratedView)
        {
            var viewReference = new WeakReference(nonGeneratedView);
            EventHandler<ActivationEventArgs> handler = null;
            handler = (s, e) =>
            {
                ((IActivate)s).Activated -= handler;
                var view = viewReference.Target;
                if (view != null)
                {
                    PlatformProvider.Current.ExecuteOnLayoutUpdated(view, ((DynamicViewAware)s).OnViewReady);
                }
            };
            activatable.Activated += handler;
        }

        private void OnViewLoaded(object view)
        {
            viewLoadedSubject.OnNext(view);
        }

        private void OnViewReady(object view)
        {
            viewReadySubject.OnNext(view);
        }
    }
}