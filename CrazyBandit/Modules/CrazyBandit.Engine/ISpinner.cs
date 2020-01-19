using System.Collections.Generic;

namespace CrazyBandit.Engine
{
    /// <summary>
    /// Interfejs reprezentujący spinner
    /// </summary>
    public interface ISpinner
    {
        /// <summary>
        /// Wszystkie dostępne linie
        /// </summary>
        int[][] Lines { get; }

        /// <summary>
        /// Linie ułożone po zakręceniu - mogą być zwycięskie lub nie
        /// </summary>
        PayLine[] PayLines { get; }

        /// <summary>
        /// Aktualny numer zakręcenia bębna
        /// </summary>
        int Rno { get; }

        /// <summary>
        /// Funkcja uruchamiająca maszynę
        /// </summary>
        void Spin();
    }
}
