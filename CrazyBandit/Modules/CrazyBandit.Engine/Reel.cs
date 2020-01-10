using System;

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
        public Symbol[] Symbols { get; }

        /// <summary>
        /// Ile symboli przelatuje przy pojedynczym zakręceniu tym walcem?
        /// </summary>
        public int Spin { get; }

        /// <summary>
        /// Ctor pozwalający zdefiniować 
        /// </summary>
        /// <param name="symbols"><inheritdoc cref="Symbols"/></param>
        /// <param name="spin"><inheritdoc cref="Spin"/></param>
        public Reel(Symbol[] symbols, int spin)
        {
            this.Symbols = symbols;
            this.Spin = spin;
        }
    }
}
