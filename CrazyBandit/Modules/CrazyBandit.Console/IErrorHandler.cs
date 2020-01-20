using System;

namespace CrazyBandit.Console
{
    /// <summary>
    /// Interfejs obsługi błędów.
    /// </summary>
    public interface IErrorHandler
    {
        /// <summary>
        /// Ma obsłużyć błąd w dowolny sposób
        /// </summary>
        /// <param name="ex">Exception do obsłużenia.</param>
        void HandleError(Exception ex);
    }
}
