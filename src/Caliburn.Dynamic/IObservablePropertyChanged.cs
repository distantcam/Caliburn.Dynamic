using System;

namespace Caliburn.Dynamic
{
    public interface IObservablePropertyChanged
    {
        IObservable<PropertyChangedData> Changed { get; }
    }
}