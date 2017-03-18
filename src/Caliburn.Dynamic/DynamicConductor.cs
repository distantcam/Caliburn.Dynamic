using System.Collections.Generic;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace Caliburn.Dynamic
{
    /// <summary>
    /// An implementation of <see cref="IConductor"/> that holds on to and activates only one item at a time.
    /// </summary>
    public partial class DynamicConductor<T> : DynamicConductorBaseWithActiveItem<T> where T : class
    {
        public DynamicConductor()
        {
            CloseGuard = () =>
            {
                var tcs = new TaskCompletionSource<bool>();
                CloseStrategy.Execute(new[] { ActiveItem }, (canClose, items) => tcs.SetResult(canClose));
                return tcs.Task;
            };
        }

        /// <summary>
        /// Activates the specified item.
        /// </summary>
        /// <param name="item">The item to activate.</param>
        public override void ActivateItem(T item)
        {
            if (item != null && item.Equals(ActiveItem))
            {
                if (IsActive)
                {
                    ScreenExtensions.TryActivate(item);
                    OnActivationProcessed(item, true);
                }
                return;
            }

            CloseStrategy.Execute(new[] { ActiveItem }, (canClose, items) =>
            {
                if (canClose)
                    ChangeActiveItem(item, true);
                else OnActivationProcessed(item, false);
            });
        }

        /// <summary>
        /// Deactivates the specified item.
        /// </summary>
        /// <param name="item">The item to close.</param>
        /// <param name="close">Indicates whether or not to close the item after deactivating it.</param>
        public override void DeactivateItem(T item, bool close)
        {
            if (item == null || !item.Equals(ActiveItem))
            {
                return;
            }

            CloseStrategy.Execute(new[] { ActiveItem }, (canClose, items) =>
            {
                if (canClose)
                    ChangeActiveItem(default(T), close);
            });
        }

        internal override void OnActivate()
        {
            ScreenExtensions.TryActivate(ActiveItem);
        }

        internal override void OnDeactivate(bool close)
        {
            ScreenExtensions.TryDeactivate(ActiveItem, close);
        }

        /// <summary>
        /// Gets the children.
        /// </summary>
        /// <returns>The collection of children.</returns>
        public override IEnumerable<T> GetChildren()
        {
            return new[] { ActiveItem };
        }
    }
}