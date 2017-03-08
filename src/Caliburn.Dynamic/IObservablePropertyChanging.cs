using System;

namespace Caliburn.Dynamic
{
    public interface IObservablePropertyChanging
    {
        IObservable<PropertyChangingData> Changing { get; }
    }
}