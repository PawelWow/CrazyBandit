using CrazyBandit.Common;
using System;
using System.Collections.Generic;

namespace CrazyBandit.Engine.Config
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
        /// Ile razy dany symbol występuje na walcu (klucz: nr symbolu, value: ilość wystąpień)
        /// </summary>
        public Dictionary<int, int> SymbolsOccurance { get; private set; }

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

            if (symbols.Length < spin)
            {
                throw new ArgumentException("Not enough symbols for that large spin.");
            }

            this.Symbols = symbols;
            this.SymbolsOccurance = this.CountSymbolsOccurance(symbols);
            this.Spin = spin;
        }

        /// <summary>
        /// Buduje statystyki wystąpienia danych symboli
        /// </summary>
        /// <param name="symbolsAll"></param>
        /// <returns></returns>
        private Dictionary<int, int> CountSymbolsOccurance(int[] symbolsAll)
        {
            Dictionary<int, int> occurance = new Dictionary<int, int>();
            foreach (int symbol in symbolsAll)
            {
                if (occurance.ContainsKey(symbol))
                {
                    occurance[symbol]++;
                }
                else
                {
                    occurance.Add(symbol, 1);
                }
            }
            return occurance;
        }
    }
}
