using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive.Concurrency;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using System.Windows.Input;
[assembly: System.Runtime.Versioning.TargetFrameworkAttribute(".NETFramework,Version=v4.5", FrameworkDisplayName=".NET Framework 4.5")]
namespace Caliburn.Dynamic
{
    public class BindableObject : System.Dynamic.DynamicObject, IObservableDataErrorInfo, IObservablePropertyChanged, IObservablePropertyChanging, INotifyDataErrorInfo, INotifyPropertyChanged, INotifyPropertyChanging, IDisposable
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
        public static ICommand<T> Create<T>(Action<T> executeMethod) { }
        public static ICommand<T> Create<T>(Action<T> executeMethod, Func<T, Boolean> canExecuteMethod) { }
        public static IAsyncCommand CreateAsync(Func<Task> executeMethod) { }
        public static IAsyncCommand CreateAsync(Func<Task> executeMethod, Func<Boolean> canExecuteMethod) { }
        public static IAsyncCommand<T> CreateAsync<T>(Func<T, Task> executeMethod) { }
        public static IAsyncCommand<T> CreateAsync<T>(Func<T, Task> executeMethod, Func<T, Boolean> canExecuteMethod) { }
    }
    public class DataErrorChanged
    {
        public DataErrorChanged(String propertyName, String error) { }
        public String Error { get; }
        public String PropertyName { get; }
    }
    public class DynamicConductor<T> : DynamicConductorBaseWithActiveItem<T>
        where T :  class
    {
        public DynamicConductor() { }
        public override void ActivateItem(T item) { }
        public override void DeactivateItem(T item, Boolean close) { }
        public override IEnumerable<T> GetChildren() { }
        public class Collection<T>
            where T :  class
        {
            public Collection() { }
            public class AllActive<T> : DynamicConductorBase<T>
                where T :  class
            {
                public AllActive(Boolean openPublicItems) { }
                public AllActive() { }
                public IObservableCollection<T> Items { get; }
                public override void ActivateItem(T item) { }
                public override void DeactivateItem(T item, Boolean close) { }
                protected override T EnsureItem(T newItem) { }
                public override IEnumerable<T> GetChildren() { }
            }
            public class OneActive<T> : DynamicConductorBaseWithActiveItem<T>
                where T :  class
            {
                public OneActive() { }
                public IObservableCollection<T> Items { get; }
                public override void ActivateItem(T item) { }
                public override void DeactivateItem(T item, Boolean close) { }
                protected virtual T DetermineNextItemToActivate(IList<T> list, Int32 lastIndex) { }
                protected override T EnsureItem(T newItem) { }
                public override IEnumerable<T> GetChildren() { }
            }
        }
    }
    public abstract class DynamicConductorBase<T> : DynamicScreen, IConductor, INotifyPropertyChangedEx, IParent, IParent<T>, INotifyPropertyChanged
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
    public abstract class DynamicConductorBaseWithActiveItem<T> : DynamicConductorBase<T>, IConductActiveItem, IConductor, IHaveActiveItem, INotifyPropertyChangedEx, IParent, INotifyPropertyChanged
        where T :  class
    {
        protected DynamicConductorBaseWithActiveItem() { }
        public T ActiveItem { get; set; }
        protected virtual void ChangeActiveItem(T newItem, Boolean closePrevious) { }
    }
    public class DynamicScreen : DynamicViewAware, IActivate, IChild, IClose, IDeactivate, IGuardClose, IHaveDisplayName, INotifyPropertyChangedEx, IScreen, INotifyPropertyChanged
    {
        public DynamicScreen() { }
        public IObservable<ActivationEventArgs> Activated { get; }
        public IObservable<DeactivationEventArgs> AttemptingDeactivation { get; }
        public Func<Task<Boolean>> CloseGuard { get; set; }
        public IObservable<DeactivationEventArgs> DeactivatedObservable { get; }
        public String DisplayName { get; set; }
        public Boolean IsActive { get; }
        public Boolean IsInitialized { get; }
        [ObsoleteAttribute("Use ChangeNotificationEnabled and SuppressNotifications instead.", true)]
        public Boolean IsNotifying { get; set; }
        public Object Parent { get; set; }
        public event EventHandler<ActivationEventArgs> Caliburn.Micro.IActivate.Activated;
        public event EventHandler<DeactivationEventArgs> Caliburn.Micro.IDeactivate.AttemptingDeactivation;
        public event EventHandler<DeactivationEventArgs> Caliburn.Micro.IDeactivate.Deactivated;
        [ObsoleteAttribute("Use OnPropertyChanging and OnPropertyChanged instead.")]
        public void NotifyOfPropertyChange(String propertyName) { }
        [ObsoleteAttribute("Do not use.")]
        public void Refresh() { }
        public void TryClose(Nullable<Boolean> dialogResult = null) { }
    }
    public class DynamicViewAware : BindableObject, IViewAware
    {
        public static readonly Object DefaultContext;
        public DynamicViewAware() { }
        public IObservable<ViewAttachedEventArgs> ViewAttached { get; }
        public IObservable<Object> ViewLoaded { get; }
        public IObservable<Object> ViewReady { get; }
        protected IDictionary<Object, Object> Views { get; }
        public event EventHandler<ViewAttachedEventArgs> Caliburn.Micro.IViewAware.ViewAttached;
    }
    public interface IAsyncCommand : IAsyncCommand<Object>, IRaiseCanExecuteChanged, ICommand { }
    public interface IAsyncCommand<in T> : IRaiseCanExecuteChanged, ICommand
    {
        Boolean CanExecute(T obj);
        Task ExecuteAsync(T obj);
    }
    public interface ICommand<in T> : IRaiseCanExecuteChanged, ICommand
    {
        Boolean CanExecute(T obj); void Execute(T obj);
    }
    public interface IObservableCommand : IDisposable, ICommand { }
    public interface IObservableDataErrorInfo
    {
        IObservable<DataErrorChanged> ErrorsChanged { get; }
    }
    public interface IObservablePropertyChanged
    {
        IObservable<PropertyChangedData> Changed { get; }
    }
    public interface IObservablePropertyChanging
    {
        IObservable<PropertyChangingData> Changing { get; }
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
        public static IObservable<PropertyChangedData<TProperty>> CastPropertyType<TProperty>(this IObservable<PropertyChangedData> observable) { }
        public static IObservable<PropertyChangingData<TProperty>> CastPropertyType<TProperty>(this IObservable<PropertyChangingData> observable) { }
        public static IDisposable Execute<T>(this IObservable<T> observable, ICommand command) { }
        public static IDisposable Execute<T>(this IObservable<T> observable, ICommand<T> command) { }
        public static IDisposable ExecuteAsync<T>(this IObservable<T> observable, IAsyncCommand<T> command) { }
        public static void RaiseCanExecuteChanged(this ICommand command) { }
        public static IObservableCommand ToCommand(this IObservable<Boolean> canExecuteObservable, Action<Object> action) { }
        public static IObservableCommand ToCommand(this IObservable<Boolean> canExecuteObservable, Action action) { }
        public static IObservableCommand ToCommand(this IObservable<PropertyChangedData<Boolean>> canExecuteObservable, Action<Object> action) { }
        public static IObservableCommand ToCommand(this IObservable<PropertyChangedData<Boolean>> canExecuteObservable, Action action) { }
        public static IObservableCommand ToCommandAsync(this IObservable<Boolean> canExecuteObservable, Func<Object, Task> action) { }
        public static IObservableCommand ToCommandAsync(this IObservable<Boolean> canExecuteObservable, Func<Task> action) { }
        public static IObservableCommand ToCommandAsync(this IObservable<PropertyChangedData<Boolean>> canExecuteObservable, Func<Object, Task> action) { }
        public static IObservableCommand ToCommandAsync(this IObservable<PropertyChangedData<Boolean>> canExecuteObservable, Func<Task> action) { }
        public static IObservable<PropertyChangedData> WhenPropertiesChanged(this IObservablePropertyChanged changed, [ParamArrayAttribute()] String[] propertyNames) { }
        public static IObservable<PropertyChangedData<TProperty>> WhenPropertiesChanged<TProperty>(this IObservablePropertyChanged changed, [ParamArrayAttribute()] String[] propertyNames) { }
        public static IObservable<PropertyChangingData> WhenPropertiesChanging(this IObservablePropertyChanging changing, [ParamArrayAttribute()] String[] propertyNames) { }
        public static IObservable<PropertyChangingData<TProperty>> WhenPropertiesChanging<TProperty>(this IObservablePropertyChanging changing, [ParamArrayAttribute()] String[] propertyNames) { }
        public static IObservable<PropertyChangedData> WhenPropertyChanged(this IObservablePropertyChanged changed, String propertyName) { }
        public static IObservable<PropertyChangedData<TProperty>> WhenPropertyChanged<TProperty>(this IObservablePropertyChanged changed, String propertyName) { }
        public static IObservable<PropertyChangingData> WhenPropertyChanging(this IObservablePropertyChanging changing, String propertyName) { }
        public static IObservable<PropertyChangingData<TProperty>> WhenPropertyChanging<TProperty>(this IObservablePropertyChanging changing, String propertyName) { }
    }
    public static class Schedulers
    {
        public static IScheduler BackgroundScheduler { get; set; }
        public static IScheduler MainScheduler { get; set; }
    }
}