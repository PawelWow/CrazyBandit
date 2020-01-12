using CrazyBandit.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace CrazyBandit.Engine
{
    /// <summary>
    /// Kalkuluje wygrane
    /// </summary>
    internal class WinningsCalculator : IWinningsCalculator
    {
        /// <summary>
        /// Wygrana za linie wygrywającą o danym symbolu
        /// </summary>
        private readonly Dictionary<int, float> m_winnings;

        /// <summary>
        /// Konfiguruje mnożnik wygranych za linie danego symbolu.
        /// </summary>
        /// <param name="winnings"></param>
        public WinningsCalculator(Dictionary<int, float> winnings)
        {
            Ensure.ParamNotNull(winnings, nameof(winnings));
            m_winnings = winnings;
        }

        /// <inheritdoc />
        public float Calculate(int bet, int symbol)
        {
            float winningValue = m_winnings[symbol];
            return bet * winningValue;            
        }
    }
}
