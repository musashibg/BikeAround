using System;
using System.Windows.Input;

namespace BikeAround.App
{
    public sealed class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;

            CommandManager.RequerySuggested += CommandManager_RequerySuggested;
        }

        #region ICommand Members

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute();
        }

        public void Execute(object parameter)
        {
            if (_canExecute == null || _canExecute())
            {
                _execute();
            }
        }

        #endregion

        private void CommandManager_RequerySuggested(object sender, EventArgs e)
        {
            if (_canExecute != null)
            {
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public sealed class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Func<T, bool> _canExecute;

        public RelayCommand(Action<T> execute, Func<T, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;

            CommandManager.RequerySuggested += CommandManager_RequerySuggested;
        }

        #region ICommand Members

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute((T)parameter);
        }

        public void Execute(object parameter)
        {
            var realParameter = (T)parameter;
            if (_canExecute == null || _canExecute(realParameter))
            {
                _execute(realParameter);
            }
        }

        #endregion

        private void CommandManager_RequerySuggested(object sender, EventArgs e)
        {
            if (_canExecute != null)
            {
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}