using System.Threading.Tasks;
using System.Windows.Input;

namespace CrazyBandit.Console
{
    /// <summary>
    /// Interfejs komend asynchronicznych.
    /// </summary>
    interface IAsyncCommand : ICommand
    {
        /// <summary>
        /// Wykonuje asynchroniczną komendę.
        /// </summary>
        /// <returns>Task, który jest awaitable</returns>
        Task ExecuteAsync();

        /// <summary>
        /// Czy można wykonać komendę?
        /// </summary>
        /// <returns></returns>
        bool CanExecute();
    }
}
