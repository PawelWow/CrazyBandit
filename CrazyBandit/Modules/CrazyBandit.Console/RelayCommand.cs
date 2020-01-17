using CrazyBandit.Common;
using System;
using System.Windows.Input;

namespace CrazyBandit.Console
{
    /// <summary>
    /// Klasa upraszczająca wykonywanie komend.
    /// </summary>
    public class RelayCommand : ICommand
    {
        /// <summary>
        /// Komenda, która ma się wykonać
        /// </summary>
        private readonly Action<object> _execute;

        /// <summary>
        /// Do mówienia czy da się wykonać daną akcję. Jeśli null to zawsze można.
        /// </summary>
        private readonly Predicate<object> _canExecute;

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (_canExecute != null)
                {
                    CommandManager.RequerySuggested += value;
                }
            }
            remove
            {
                if (_canExecute != null)
                {
                    CommandManager.RequerySuggested -= value;
                }
            }
        }

        /// <summary>
        /// C-tor pozwalający ustawić komendę do wykonania i określić czy można ją wykonać.
        /// </summary>
        /// <param name="execute">Akcja wykonywana w komendzie.</param>
        /// <param name="canExecute"><inheritdoc cref="_canExecute"/></param>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            Ensure.ParamNotNull(execute, nameof(execute));

            _canExecute = canExecute;
            _execute = execute;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool CanExecute(object parameter)
        {
            if (_canExecute == null)
            {
                return true;
            }
            else
            {
                return _canExecute(parameter);
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}
