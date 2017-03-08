namespace Caliburn.Dynamic
{
    public interface ICommand : IRaiseCanExecuteChanged, System.Windows.Input.ICommand
    {
    }

    public interface ICommand<in T> : IRaiseCanExecuteChanged, ICommand
    {
        void Execute(T obj);

        bool CanExecute(T obj);
    }
}