using System;

namespace Caliburn.Dynamic
{
    public interface IObservableDataErrorInfo
    {
        IObservable<DataErrorChanged> ErrorsChanged { get; }
    }
}