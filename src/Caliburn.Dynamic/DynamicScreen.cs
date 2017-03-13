using System;
using System.ComponentModel;
using System.Reactive.Subjects;
using System.Threading;
using Caliburn.Micro;

namespace Caliburn.Dynamic
{
    /// <summary>
    /// A base implementation of <see cref = "IScreen" />.
    /// </summary>
    public class DynamicScreen : DynamicViewAware, IScreen, IChild, INotifyPropertyChangedEx
    {
        static readonly ILog Log = LogManager.GetLog(typeof(DynamicScreen));

        bool isActive;
        bool isInitialized;
        object parent;
        string displayName;

        EventHandler<ActivationEventArgs> activated;
        EventHandler<DeactivationEventArgs> attemptingDeactivation;
        EventHandler<DeactivationEventArgs> deactivated;

        Subject<ActivationEventArgs> activatedSubject;
        Subject<DeactivationEventArgs> attemptingDeactivationSubject;
        Subject<DeactivationEventArgs> deactivatedSubject;

        IObservable<ActivationEventArgs> activatedObservable;
        IObservable<DeactivationEventArgs> attemptingDeactivationObservable;
        IObservable<DeactivationEventArgs> deactivatedObservable;

        /// <summary>
        /// Creates an instance of the screen.
        /// </summary>
        public DynamicScreen()
        {
            displayName = GetType().FullName;

            InitializeObservableEvent(ref activatedSubject, ref activatedObservable, args => activated?.Invoke(this, args));
            InitializeObservableEvent(ref attemptingDeactivationSubject, ref attemptingDeactivationObservable, args => attemptingDeactivation?.Invoke(this, args));
            InitializeObservableEvent(ref deactivatedSubject, ref deactivatedObservable, args => deactivated?.Invoke(this, args));
        }

        public IObservable<ActivationEventArgs> Activated => activatedObservable;
        public IObservable<DeactivationEventArgs> AttemptingDeactivation => attemptingDeactivationObservable;
        public IObservable<DeactivationEventArgs> DeactivatedObservable => deactivatedObservable;

        /// <summary>
        /// Gets or Sets the Parent <see cref = "IConductor" />
        /// </summary>
        public virtual object Parent
        {
            get { return parent; }
            set
            {
                if (parent == value)
                    return;
                var before = parent;
                OnPropertyChanging(nameof(Parent), before);
                parent = value;
                OnPropertyChanged(nameof(Parent), before, value);
            }
        }

        /// <summary>
        /// Gets or Sets the Display Name
        /// </summary>
        public virtual string DisplayName
        {
            get { return displayName; }
            set
            {
                if (displayName == value)
                    return;
                var before = displayName;
                OnPropertyChanging(nameof(DisplayName), before);
                displayName = value;
                OnPropertyChanged(nameof(DisplayName), before, value);
            }
        }

        /// <summary>
        /// Indicates whether or not this instance is currently active.
        /// Virtualized in order to help with document oriented view models.
        /// </summary>
        public virtual bool IsActive
        {
            get { return isActive; }
            private set
            {
                if (isActive == value)
                    return;
                var before = isActive;
                OnPropertyChanging(nameof(IsActive), before);
                isActive = value;
                OnPropertyChanged(nameof(IsActive), before, value);
            }
        }

        /// <summary>
        /// Indicates whether or not this instance is currently initialized.
        /// Virtualized in order to help with document oriented view models.
        /// </summary>
        public virtual bool IsInitialized
        {
            get { return isInitialized; }
            private set
            {
                if (isInitialized == value)
                    return;
                var before = isInitialized;
                OnPropertyChanging(nameof(IsInitialized), before);
                isInitialized = value;
                OnPropertyChanged(nameof(IsInitialized), before, value);
            }
        }

        /// <summary>
        /// Raised after activation occurs.
        /// </summary>
        event EventHandler<ActivationEventArgs> IActivate.Activated
        {
            add
            {
                EventHandler<ActivationEventArgs> handler2;
                var newEvent = activated;
                do
                {
                    handler2 = newEvent;
                    var handler3 = (EventHandler<ActivationEventArgs>)Delegate.Combine(handler2, value);
                    Interlocked.CompareExchange(ref activated, handler3, handler2);
                } while (newEvent != handler2);
            }
            remove
            {
                EventHandler<ActivationEventArgs> handler2;
                var newEvent = activated;
                do
                {
                    handler2 = newEvent;
                    var handler3 = (EventHandler<ActivationEventArgs>)Delegate.Remove(handler2, value);
                    Interlocked.CompareExchange(ref activated, handler3, handler2);
                } while (newEvent != handler2);
            }
        }

        /// <summary>
        /// Raised before deactivation.
        /// </summary>
        event EventHandler<DeactivationEventArgs> IDeactivate.AttemptingDeactivation
        {
            add
            {
                EventHandler<DeactivationEventArgs> handler2;
                var newEvent = attemptingDeactivation;
                do
                {
                    handler2 = newEvent;
                    var handler3 = (EventHandler<DeactivationEventArgs>)Delegate.Combine(handler2, value);
                    Interlocked.CompareExchange(ref attemptingDeactivation, handler3, handler2);
                } while (newEvent != handler2);
            }
            remove
            {
                EventHandler<DeactivationEventArgs> handler2;
                var newEvent = attemptingDeactivation;
                do
                {
                    handler2 = newEvent;
                    var handler3 = (EventHandler<DeactivationEventArgs>)Delegate.Remove(handler2, value);
                    Interlocked.CompareExchange(ref attemptingDeactivation, handler3, handler2);
                } while (newEvent != handler2);
            }
        }

        /// <summary>
        /// Raised after deactivation.
        /// </summary>
        event EventHandler<DeactivationEventArgs> IDeactivate.Deactivated
        {
            add
            {
                EventHandler<DeactivationEventArgs> handler2;
                var newEvent = deactivated;
                do
                {
                    handler2 = newEvent;
                    var handler3 = (EventHandler<DeactivationEventArgs>)Delegate.Combine(handler2, value);
                    Interlocked.CompareExchange(ref deactivated, handler3, handler2);
                } while (newEvent != handler2);
            }
            remove
            {
                EventHandler<DeactivationEventArgs> handler2;
                var newEvent = deactivated;
                do
                {
                    handler2 = newEvent;
                    var handler3 = (EventHandler<DeactivationEventArgs>)Delegate.Remove(handler2, value);
                    Interlocked.CompareExchange(ref deactivated, handler3, handler2);
                } while (newEvent != handler2);
            }
        }

        void IActivate.Activate()
        {
            if (IsActive)
            {
                return;
            }

            var initialized = false;

            if (!IsInitialized)
            {
                IsInitialized = initialized = true;
                OnInitialize();
            }

            IsActive = true;
            Log.Info("Activating {0}.", this);
            OnActivate();

            activatedSubject.OnNext(new ActivationEventArgs
            {
                WasInitialized = initialized
            });
        }

        /// <summary>
        /// Called when initializing.
        /// </summary>
        protected virtual void OnInitialize() { }

        /// <summary>
        /// Called when activating.
        /// </summary>
        protected virtual void OnActivate() { }

        void IDeactivate.Deactivate(bool close)
        {
            if (IsActive || (IsInitialized && close))
            {
                attemptingDeactivationSubject.OnNext(new DeactivationEventArgs
                {
                    WasClosed = close
                });

                IsActive = false;
                Log.Info("Deactivating {0}.", this);
                OnDeactivate(close);

                deactivatedSubject.OnNext(new DeactivationEventArgs
                {
                    WasClosed = close
                });

                if (close)
                {
                    Views.Clear();
                    Log.Info("Closed {0}.", this);
                }
            }
        }

        /// <summary>
        /// Called when deactivating.
        /// </summary>
        /// <param name = "close">Inidicates whether this instance will be closed.</param>
        protected virtual void OnDeactivate(bool close) { }

        /// <summary>
        /// Called to check whether or not this instance can close.
        /// </summary>
        /// <param name = "callback">The implementor calls this action with the result of the close check.</param>
        public virtual void CanClose(Action<bool> callback)
        {
            callback(true);
        }

        /// <summary>
        /// Tries to close this instance by asking its Parent to initiate shutdown or by asking its corresponding view to close.
        /// Also provides an opportunity to pass a dialog result to it's corresponding view.
        /// </summary>
        /// <param name="dialogResult">The dialog result.</param>
        public virtual void TryClose(bool? dialogResult = null)
        {
            PlatformProvider.Current.GetViewCloseAction(this, Views.Values, dialogResult).OnUIThread();
        }

        /// <summary>
        /// Enables/Disables property change notification.
        /// Virtualized in order to help with document oriented view models.
        /// </summary>
        /// <remarks>Obsolete. Should use ChangeNotificationEnabled and SuppressNotifications instead.</remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Use ChangeNotificationEnabled and SuppressNotifications instead.", true)]
        public bool IsNotifying
        {
            get { return ChangeNotificationEnabled; }
            set { throw new NotSupportedException(); }
        }

        /// <summary>
        /// Notifies subscribers of the property change.
        /// </summary>
        /// <param name = "propertyName">Name of the property.</param>
        /// <remarks>Obsolete. Should use OnPropertyChanging and OnPropertyChanged instead.</remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Use OnPropertyChanging and OnPropertyChanged instead.")]
        public void NotifyOfPropertyChange(string propertyName)
        {
            propertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Raises a change notification indicating that all bindings should be refreshed.
        /// </summary>
        /// <remarks>Obsolete. Do not use.</remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Do not use.")]
        public void Refresh()
        {
            propertyChanged?.Invoke(this, new PropertyChangedEventArgs(string.Empty));
        }
    }
}