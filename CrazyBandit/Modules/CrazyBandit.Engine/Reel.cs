using CrazyBandit.Common;

namespace CrazyBandit.Engine
{
    /// <summary>
    /// Klasa walca
    /// </summary>
    public class Reel
    {
        /// <summary>
        /// Symbole tegoż walca
        /// </summary>
        public int[] Symbols { get; }

        /// <summary>
        /// Ile symboli przelatuje przy pojedynczym zakręceniu tym walcem?
        /// </summary>
        public int Spin { get; }

        /// <summary>
        /// Ctor pozwalający zdefiniować 
        /// </summary>
        /// <param name="symbols"><inheritdoc cref="Symbols"/></param>
        /// <param name="spin"><inheritdoc cref="Spin"/></param>
        public Reel(int[] symbols, int spin)
        {
            Ensure.ParamNotNull(symbols, nameof(symbols));

            this.Symbols = symbols;
            this.Spin = spin;
        }
    }
}
