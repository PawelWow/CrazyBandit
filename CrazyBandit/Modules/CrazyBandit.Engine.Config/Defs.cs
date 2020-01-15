namespace CrazyBandit.Engine.Config
{
    /// <summary>
    /// Globalne definicje dla projektu
    /// </summary>
    public class Defs
    {
        /// <summary>
        /// Liczba linii wygrywających. Jest to jednocześnie minimum kombinacji obsługiwanych przez program.
        /// </summary>
        internal const int PayLines = 5;

        /// <summary>
        /// Maksymalna wartość defaultowego Rno. Potrzebna do wyliczenia zakresu
        /// </summary>
        public const int MaxRnoDefaultValue = 500;

        /// <summary>
        /// Minimalna wartość defaultowego Rno. Potrzebna do wyliczenia zakresu
        /// </summary>
        public const int MinRnoDefaultValue = 0;
    }
}
