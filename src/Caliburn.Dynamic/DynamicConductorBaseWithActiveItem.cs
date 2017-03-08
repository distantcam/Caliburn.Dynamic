using Caliburn.Micro;

namespace Caliburn.Dynamic
{
    /// <summary>
    /// A base class for various implementations of <see cref="IConductor"/> that maintain an active item.
    /// </summary>
    /// <typeparam name="T">The type that is being conducted.</typeparam>
    public abstract class DynamicConductorBaseWithActiveItem<T> : DynamicConductorBase<T>, IConductActiveItem where T : class
    {
        T activeItem;

        /// <summary>
        /// The currently active item.
        /// </summary>
        public T ActiveItem
        {
            get { return activeItem; }
            set
            {
                if (activeItem == value)
                    return;
                var before = activeItem;
                OnPropertyChanging(nameof(ActivateItem), before);
                ActivateItem(value);
                OnPropertyChanged(nameof(ActivateItem), before, value);
            }
        }

        /// <summary>
        /// The currently active item.
        /// </summary>
        /// <value></value>
        object IHaveActiveItem.ActiveItem
        {
            get { return ActiveItem; }
            set { ActiveItem = (T)value; }
        }

        /// <summary>
        /// Changes the active item.
        /// </summary>
        /// <param name="newItem">The new item to activate.</param>
        /// <param name="closePrevious">Indicates whether or not to close the previous active item.</param>
        protected virtual void ChangeActiveItem(T newItem, bool closePrevious)
        {
            if (activeItem == newItem)
                return;

            ScreenExtensions.TryDeactivate(activeItem, closePrevious);

            newItem = EnsureItem(newItem);

            if (IsActive)
                ScreenExtensions.TryActivate(newItem);

            var before = activeItem;
            OnPropertyChanging(nameof(ActiveItem), before);
            activeItem = newItem;
            OnPropertyChanged(nameof(ActiveItem), before, newItem);
            OnActivationProcessed(activeItem, true);
        }
    }
}