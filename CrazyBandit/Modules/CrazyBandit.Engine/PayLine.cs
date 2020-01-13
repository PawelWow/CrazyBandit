using CrazyBandit.Common;

namespace CrazyBandit.Engine
{
    /// <summary>
    /// Deskryptor linii wygrywającej
    /// </summary>
    public class PayLine
    {
        /// <summary>
        /// Index, po którym znajdziemy tę linię w bazie
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        /// Linia
        /// </summary>
        public int[] Line { get; private set; }

        /// <summary>
        /// Czy ta linia oznacza wygraną?
        /// </summary>
        public bool IsWinningLine { get; protected set; }

        /// <summary>
        /// C-tor ustawiający wartości. 
        /// </summary>
        /// <param name="index"><inheritdoc cref="Index"/></param>
        /// <param name="line"><inheritdoc cref="Line"/></param>
        public PayLine(int index, int[] line)
        {
            Ensure.ParamNotNull(line, nameof(line));

            this.Index = index;
            this.Line = line;
            this.IsWinningLine = this.AreSymbolsTheSame(line);
        }

        /// <summary>
        /// Czy wszystkie symbole (elementy) na linii są takie same?
        /// </summary>
        /// <param name="line">Linia, którą sprawdzamy</param>
        /// <returns>True, jeśli wszystkie elementy mają tę samą wartość.</returns>
        private bool AreSymbolsTheSame(int[] line)
        {
            for (int i = 0; i < line.Length; i++)
            {
                if (i == 0)
                {
                    continue;
                }

                if (line[i] != line[i - 1])
                {
                    return false;
                }
            }

            return true;
        }
    }

}
