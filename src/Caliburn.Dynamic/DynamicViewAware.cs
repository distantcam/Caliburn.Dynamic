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

        protected void InitializeObservableEvent<TArg>(ref Subject<TArg> subject, ref IObservable<TArg> observable, Action<TArg> subscription)
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
            OnViewAttached(nonGeneratedView, context);

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

        /// <summary>
        /// Called when a view is attached.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="context">The context in which the view appears.</param>
        protected virtual void OnViewAttached(object view, object context)
        {
            viewAttachedSubject.OnNext(new ViewAttachedEventArgs { View = view, Context = context });
        }

        /// <summary>
        /// Called when an attached view's Loaded event fires.
        /// </summary>
        /// <param name = "view"></param>
        protected virtual void OnViewLoaded(object view)
        {
        }

        /// <summary>
        /// Called the first time the page's LayoutUpdated event fires after it is navigated to.
        /// </summary>
        /// <param name = "view"></param>
        protected virtual void OnViewReady(object view)
        {
        }

        /// <summary>
        /// Gets a view previously attached to this instance.
        /// </summary>
        /// <param name = "context">The context denoting which view to retrieve.</param>
        /// <returns>The view.</returns>
        public virtual object GetView(object context = null)
        {
            object view;
            Views.TryGetValue(context ?? DefaultContext, out view);
            return view;
        }
    }
}