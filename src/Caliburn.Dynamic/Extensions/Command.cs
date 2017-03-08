﻿using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Caliburn.Dynamic.Commands;

namespace Caliburn.Dynamic
{
    public static partial class PublicExtensions
    {
        public static void RaiseCanExecuteChanged(this System.Windows.Input.ICommand command)
        {
            var canExecuteChanged = command as IRaiseCanExecuteChanged;

            if (canExecuteChanged != null)
                canExecuteChanged.RaiseCanExecuteChanged();
        }

        public static IObservableCommand ToCommand(this IObservable<bool> canExecuteObservable, Func<object, Task> action) =>
            new ObservableCommand(canExecuteObservable, action);

        public static IObservableCommand ToCommand(this IObservable<bool> canExecuteObservable, Func<Task> action) =>
            new ObservableCommand(canExecuteObservable, _ => action());

        public static IObservableCommand ToCommand(this IObservable<PropertyChangedData<bool>> canExecuteObservable, Func<object, Task> action) =>
            new ObservableCommand(canExecuteObservable.Select(pc => pc.After), action);

        public static IObservableCommand ToCommand(this IObservable<PropertyChangedData<bool>> canExecuteObservable, Func<Task> action) =>
            new ObservableCommand(canExecuteObservable.Select(pc => pc.After), _ => action());

        public static IObservableCommand ToCommand(this IObservable<bool> canExecuteObservable, Action<object> action) =>
            new ObservableCommand(canExecuteObservable, p => { action(p); return Task.FromResult(0); });

        public static IObservableCommand ToCommand(this IObservable<bool> canExecuteObservable, Action action) =>
            new ObservableCommand(canExecuteObservable, _ => { action(); return Task.FromResult(0); });

        public static IObservableCommand ToCommand(this IObservable<PropertyChangedData<bool>> canExecuteObservable, Action<object> action) =>
            new ObservableCommand(canExecuteObservable.Select(pc => pc.After), p => { action(p); return Task.FromResult(0); });

        public static IObservableCommand ToCommand(this IObservable<PropertyChangedData<bool>> canExecuteObservable, Action action) =>
            new ObservableCommand(canExecuteObservable.Select(pc => pc.After), _ => { action(); return Task.FromResult(0); });

        public static IDisposable Execute<T>(this IObservable<T> observable, System.Windows.Input.ICommand command) =>
            observable.Do(t => { if (command.CanExecute(t)) command.Execute(t); }).Subscribe();

        public static IDisposable Execute<T>(this IObservable<T> observable, ICommand<T> command) =>
            observable.Do(t => { if (command.CanExecute(t)) command.Execute(t); }).Subscribe();

        public static IDisposable ExecuteAsync<T>(this IObservable<T> observable, IAsyncCommand<T> command) =>
            observable.SelectMany(async t => { if (command.CanExecute(t)) await command.ExecuteAsync(t); return Unit.Default; }).Subscribe();
    }
}