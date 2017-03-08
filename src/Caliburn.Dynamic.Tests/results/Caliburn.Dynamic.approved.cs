using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive.Concurrency;
using System.Reactive.Subjects;
using System.Windows.Input;
[assembly: System.Runtime.Versioning.TargetFrameworkAttribute(".NETFramework,Version=v4.6.1", FrameworkDisplayName=".NET Framework 4.6.1")]
namespace Caliburn.Dynamic
{
    public class BindableObject : System.Dynamic.DynamicObject, Caliburn.Dynamic.IObservableDataErrorInfo, Caliburn.Dynamic.IObservablePropertyChanged, Caliburn.Dynamic.IObservablePropertyChanging, INotifyDataErrorInfo, INotifyPropertyChanged, INotifyPropertyChanging, IDisposable
    {
        public BindableObject() { }
        public Boolean ChangeNotificationEnabled { get; }
        public event EventHandler<DataErrorsChangedEventArgs> System.ComponentModel.INotifyDataErrorInfo.ErrorsChanged;
        public event PropertyChangedEventHandler System.ComponentModel.INotifyPropertyChanged.PropertyChanged;
        public event PropertyChangingEventHandler System.ComponentModel.INotifyPropertyChanging.PropertyChanging;
        public virtual void Dispose() { }
        protected void OnPropertyChanged(String propertyName, Object before, Object after) { }
        protected void OnPropertyChanging(String propertyName, Object before) { }
        public void ResetDataError(String propertyName) { }
        public void SetDataError(String propertyName, String error) { }
        public IDisposable SuppressNotifications() { }
        public override Boolean TryGetMember(System.Dynamic.GetMemberBinder binder, out Object result) { }
        public override Boolean TrySetMember(System.Dynamic.SetMemberBinder binder, Object value) { }
    }
    public static class Command
    {
        public static ICommand Create(Action executeMethod) { }
        public static ICommand Create(Action executeMethod, Func<Boolean> canExecuteMethod) { }
        public static Caliburn.Dynamic.ICommand<T> Create<T>(Action<T> executeMethod) { }
        public static Caliburn.Dynamic.ICommand<T> Create<T>(Action<T> executeMethod, Func<T, Boolean> canExecuteMethod) { }
        public static Caliburn.Dynamic.IAsyncCommand CreateAsync(Func<System.Threading.Tasks.Task> executeMethod) { }
        public static Caliburn.Dynamic.IAsyncCommand CreateAsync(Func<System.Threading.Tasks.Task> executeMethod, Func<Boolean> canExecuteMethod) { }
        public static Caliburn.Dynamic.IAsyncCommand<T> CreateAsync<T>(Func<T, System.Threading.Tasks.Task> executeMethod) { }
        public static Caliburn.Dynamic.IAsyncCommand<T> CreateAsync<T>(Func<T, System.Threading.Tasks.Task> executeMethod, Func<T, Boolean> canExecuteMethod) { }
    }
    public class DataErrorChanged
    {
        public DataErrorChanged(String propertyName, String error) { }
        public String Error { get; }
        public String PropertyName { get; }
    }
    public class DynamicConductor<T> : Caliburn.Dynamic.DynamicConductorBaseWithActiveItem<T>
        where T :  class
    {
        public DynamicConductor() { }
        public override void ActivateItem(T item) { }
        public override void CanClose(Action<Boolean> callback) { }
        public override void DeactivateItem(T item, Boolean close) { }
        public override IEnumerable<T> GetChildren() { }
        protected override void OnActivate() { }
        protected override void OnDeactivate(Boolean close) { }
        public class Collection<T>
            where T :  class
        {
            public Collection() { }
            public class AllActive<T> : Caliburn.Dynamic.DynamicConductorBase<T>
                where T :  class
            {
                public AllActive(Boolean openPublicItems) { }
                public AllActive() { }
                public IObservableCollection<T> Items { get; }
                public override void ActivateItem(T item) { }
                public override void CanClose(Action<Boolean> callback) { }
                public override void DeactivateItem(T item, Boolean close) { }
                protected override T EnsureItem(T newItem) { }
                public override IEnumerable<T> GetChildren() { }
                protected override void OnActivate() { }
                protected override void OnDeactivate(Boolean close) { }
                protected override void OnInitialize() { }
            }
            public class OneActive<T> : Caliburn.Dynamic.DynamicConductorBaseWithActiveItem<T>
                where T :  class
            {
                public OneActive() { }
                public IObservableCollection<T> Items { get; }
                public override void ActivateItem(T item) { }
                public override void CanClose(Action<Boolean> callback) { }
                public override void DeactivateItem(T item, Boolean close) { }
                protected virtual T DetermineNextItemToActivate(IList<T> list, Int32 lastIndex) { }
                protected override T EnsureItem(T newItem) { }
                public override IEnumerable<T> GetChildren() { }
                protected override void OnActivate() { }
                protected override void OnDeactivate(Boolean close) { }
            }
        }
    }
    public abstract class DynamicConductorBase<T> : Caliburn.Dynamic.DynamicScreen, IConductor, INotifyPropertyChangedEx, IParent, IParent<T>, INotifyPropertyChanged
        where T :  class
    {
        public DynamicConductorBase() { }
        public IObservable<ActivationProcessedEventArgs> ActivationProcessed { get; }
        public ICloseStrategy<T> CloseStrategy { get; set; }
        public event EventHandler<ActivationProcessedEventArgs> Caliburn.Micro.IConductor.ActivationProcessed;
        public abstract void ActivateItem(T item);
        public abstract void DeactivateItem(T item, Boolean close);
        protected virtual T EnsureItem(T newItem) { }
        public abstract IEnumerable<T> GetChildren();
        protected virtual void OnActivationProcessed(T item, Boolean success) { }
    }
    public abstract class DynamicConductorBaseWithActiveItem<T> : Caliburn.Dynamic.DynamicConductorBase<T>, IConductActiveItem, IConductor, IHaveActiveItem, INotifyPropertyChangedEx, IParent, INotifyPropertyChanged
        where T :  class
    {
        protected DynamicConductorBaseWithActiveItem() { }
        public T ActiveItem { get; set; }
        protected virtual void ChangeActiveItem(T newItem, Boolean closePrevious) { }
    }
    public class DynamicScreen : Caliburn.Dynamic.DynamicViewAware, IActivate, IChild, IClose, IDeactivate, IGuardClose, IHaveDisplayName, INotifyPropertyChangedEx, IScreen, INotifyPropertyChanged
    {
        public DynamicScreen() { }
        public IObservable<ActivationEventArgs> Activated { get; }
        public IObservable<DeactivationEventArgs> AttemptingDeactivation { get; }
        public IObservable<DeactivationEventArgs> DeactivatedObservable { get; }
        public virtual String DisplayName { get; set; }
        public Boolean IsActive { get; }
        public Boolean IsInitialized { get; }
        public virtual Object Parent { get; set; }
        public event EventHandler<ActivationEventArgs> Caliburn.Micro.IActivate.Activated;
        public event EventHandler<DeactivationEventArgs> Caliburn.Micro.IDeactivate.AttemptingDeactivation;
        public event EventHandler<DeactivationEventArgs> Caliburn.Micro.IDeactivate.Deactivated;
        public virtual void CanClose(Action<Boolean> callback) { }
        protected virtual void OnActivate() { }
        protected virtual void OnDeactivate(Boolean close) { }
        protected virtual void OnInitialize() { }
        public virtual void TryClose(Nullable<Boolean> dialogResult = null) { }
    }
    public class DynamicViewAware : Caliburn.Dynamic.BindableObject, IViewAware
    {
        public static readonly Object DefaultContext;
        public DynamicViewAware() { }
        public IObservable<ViewAttachedEventArgs> ViewAttached { get; }
        public IObservable<Object> ViewLoaded { get; }
        public IObservable<Object> ViewReady { get; }
        protected IDictionary<Object, Object> Views { get; }
        public event EventHandler<ViewAttachedEventArgs> Caliburn.Micro.IViewAware.ViewAttached;
        public virtual Object GetView(Object context = null) { }
        protected void InitializeObservableEvent<TArg>(ref Subject<> subject, ref IObservable<> observable, Action<TArg> subscription) { }
        protected virtual void OnViewAttached(Object view, Object context) { }
        protected virtual void OnViewLoaded(Object view) { }
        protected virtual void OnViewReady(Object view) { }
    }
    public interface IAsyncCommand : Caliburn.Dynamic.IAsyncCommand<Object>, Caliburn.Dynamic.IRaiseCanExecuteChanged, ICommand { }
    public interface IAsyncCommand<in T> : Caliburn.Dynamic.IRaiseCanExecuteChanged, ICommand
    {
        Boolean CanExecute(T obj);
        System.Threading.Tasks.Task ExecuteAsync(T obj);
    }
    public interface ICommand<in T> : Caliburn.Dynamic.IRaiseCanExecuteChanged, ICommand
    {
        Boolean CanExecute(T obj); void Execute(T obj);
    }
    public interface IObservableCommand : IDisposable, ICommand { }
    public interface IObservableDataErrorInfo
    {
        IObservable<Caliburn.Dynamic.DataErrorChanged> ErrorsChanged { get; }
    }
    public interface IObservablePropertyChanged
    {
        IObservable<Caliburn.Dynamic.PropertyChangedData> Changed { get; }
    }
    public interface IObservablePropertyChanging
    {
        IObservable<Caliburn.Dynamic.PropertyChangingData> Changing { get; }
    }
    public interface IRaiseCanExecuteChanged
    { void RaiseCanExecuteChanged();
    }
    public class PropertyChangedData
    {
        public PropertyChangedData(String propertyName, Object before, Object after) { }
        public Object After { get; }
        public Object Before { get; }
        public String PropertyName { get; }
    }
    public class PropertyChangedData<TProperty>
    {
        public PropertyChangedData(String propertyName, TProperty before, TProperty after) { }
        public TProperty After { get; }
        public TProperty Before { get; }
        public String PropertyName { get; }
    }
    public class PropertyChangingData
    {
        public PropertyChangingData(String propertyName, Object before) { }
        public Object Before { get; }
        public String PropertyName { get; }
    }
    public class PropertyChangingData<TProperty>
    {
        public PropertyChangingData(String propertyName, TProperty before) { }
        public TProperty Before { get; }
        public String PropertyName { get; }
    }
    public static class PublicExtensions
    {
        public static IObservable<Caliburn.Dynamic.PropertyChangedData<TProperty>> CastPropertyType<TProperty>(this IObservable<Caliburn.Dynamic.PropertyChangedData> observable) { }
        public static IObservable<Caliburn.Dynamic.PropertyChangingData<TProperty>> CastPropertyType<TProperty>(this IObservable<Caliburn.Dynamic.PropertyChangingData> observable) { }
        public static IDisposable Execute<T>(this IObservable<T> observable, ICommand command) { }
        public static IDisposable Execute<T>(this IObservable<T> observable, Caliburn.Dynamic.ICommand<T> command) { }
        public static IDisposable ExecuteAsync<T>(this IObservable<T> observable, Caliburn.Dynamic.IAsyncCommand<T> command) { }
        public static void RaiseCanExecuteChanged(this ICommand command) { }
        public static Caliburn.Dynamic.IObservableCommand ToCommand(this IObservable<Boolean> canExecuteObservable, Func<Object, System.Threading.Tasks.Task> action) { }
        public static Caliburn.Dynamic.IObservableCommand ToCommand(this IObservable<Caliburn.Dynamic.PropertyChangedData<Boolean>> canExecuteObservable, Func<Object, System.Threading.Tasks.Task> action) { }
        public static Caliburn.Dynamic.IObservableCommand ToCommand(this IObservable<Boolean> canExecuteObservable, Action<Object> action) { }
        public static Caliburn.Dynamic.IObservableCommand ToCommand(this IObservable<Caliburn.Dynamic.PropertyChangedData<Boolean>> canExecuteObservable, Action<Object> action) { }
        public static IObservable<Caliburn.Dynamic.PropertyChangedData> WhenPropertiesChanged(this Caliburn.Dynamic.IObservablePropertyChanged changed, [ParamArrayAttribute()] String[] propertyNames) { }
        public static IObservable<Caliburn.Dynamic.PropertyChangedData<TProperty>> WhenPropertiesChanged<TProperty>(this Caliburn.Dynamic.IObservablePropertyChanged changed, [ParamArrayAttribute()] String[] propertyNames) { }
        public static IObservable<Caliburn.Dynamic.PropertyChangingData> WhenPropertiesChanging(this Caliburn.Dynamic.IObservablePropertyChanging changing, [ParamArrayAttribute()] String[] propertyNames) { }
        public static IObservable<Caliburn.Dynamic.PropertyChangingData<TProperty>> WhenPropertiesChanging<TProperty>(this Caliburn.Dynamic.IObservablePropertyChanging changing, [ParamArrayAttribute()] String[] propertyNames) { }
        public static IObservable<Caliburn.Dynamic.PropertyChangedData> WhenPropertyChanged(this Caliburn.Dynamic.IObservablePropertyChanged changed, String propertyName) { }
        public static IObservable<Caliburn.Dynamic.PropertyChangedData<TProperty>> WhenPropertyChanged<TProperty>(this Caliburn.Dynamic.IObservablePropertyChanged changed, String propertyName) { }
        public static IObservable<Caliburn.Dynamic.PropertyChangingData> WhenPropertyChanging(this Caliburn.Dynamic.IObservablePropertyChanging changing, String propertyName) { }
        public static IObservable<Caliburn.Dynamic.PropertyChangingData<TProperty>> WhenPropertyChanging<TProperty>(this Caliburn.Dynamic.IObservablePropertyChanging changing, String propertyName) { }
    }
    public static class Schedulers
    {
        public static IScheduler BackgroundScheduler { get; set; }
        public static IScheduler MainScheduler { get; set; }
    }
}