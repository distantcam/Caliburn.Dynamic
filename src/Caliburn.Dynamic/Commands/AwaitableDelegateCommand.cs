﻿using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Caliburn.Dynamic.Commands
{
    internal class AwaitableDelegateCommand : AwaitableDelegateCommand<object>, IAsyncCommand
    {
        public AwaitableDelegateCommand(Func<object, Task> executeMethod, Func<object, bool> canExecuteMethod = null) : base(executeMethod, canExecuteMethod)
        {
        }
    }

    internal class AwaitableDelegateCommand<T> : BaseCommand<T>, IAsyncCommand<T>
    {
        private readonly Func<T, Task> executeMethod;

        public AwaitableDelegateCommand(Func<T, Task> executeMethod, Func<T, bool> canExecuteMethod = null) : base(canExecuteMethod)
        {
            if (executeMethod == null)
                throw new ArgumentNullException(nameof(executeMethod), $"{nameof(executeMethod)} is null.");

            this.executeMethod = executeMethod;
        }

        bool ICommand.CanExecute(object parameter) => CanExecute((T)parameter);

        async void ICommand.Execute(object parameter) => await ExecuteAsync((T)parameter);

        public async Task ExecuteAsync(T parameter)
        {
            using (StartExecuting())
            {
                await executeMethod(parameter);
            }
        }
    }
}