using CrazyBandit.Common;
using CrazyBandit.Console.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CrazyBandit.Console
{
    /// <summary>
    /// Implementacja komendy asynchronicznej.
    /// </summary>
    internal class AsyncCommand : IAsyncCommand
    {
        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Czy komenda jest wykonywana właśnie?
        /// </summary>
        private bool _isExecuting;

        /// <summary>
        /// Co właściwie ma się wykonać
        /// </summary>
        private readonly Func<Task> _execute;

        /// <summary>
        /// Czy można wykonać akcję?
        /// </summary>
        private readonly Func<bool> _canExecute;

        /// <summary>
        /// Handler potrzebny do wywołań asynchronciznych.
        /// </summary>
        private readonly IErrorHandler _errorHandler;

        /// <summary>
        /// C-tor umożłiwiający wykonanie komendy
        /// </summary>
        /// <param name="execute"><see cref="_execute"/></param>
        /// <param name="canExecute"><see cref="_canExecute"/></param>
        /// <param name="errorHandler"><see cref="_errorHandler"/></param>
        public AsyncCommand(Func<Task> execute, Func<bool> canExecute = null, IErrorHandler errorHandler = null)
        {
            Ensure.ParamNotNull(execute, nameof(execute));

            _execute = execute;
            _canExecute = canExecute;
            _errorHandler = errorHandler;
        }
        
        /// <inheritdoc />        
        public bool CanExecute()
        {
            return !_isExecuting && (_canExecute?.Invoke() ?? true);
        }

        /// <inheritdoc />    
        public async Task ExecuteAsync()
        {
            if (this.CanExecute() == false)
            {
                return;
            }

            try
            {
                _isExecuting = true;
                await _execute();
            }
            finally
            {
                _isExecuting = false;
            }
        }

        // TODO !!  
        public void OnCanExecuteChanged()
        {
            this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        #region ICommand
        /// <inheritdoc /> 
        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute();
        }
        /// <inheritdoc /> 
        void ICommand.Execute(object parameter)
        {
            this.ExecuteAsync().FireAndForgetSafeAsync(_errorHandler);                       
        }

        #endregion

    }
}
