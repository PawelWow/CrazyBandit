using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CrazyBandit.Engine.UnitTests
{
    /// <summary>
    /// Klasa testuj¹ca kalkulator wygranych
    /// </summary>
    [TestClass]
    public class TestWinningCalculator
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WinningsCalculator_create_null()
        {
            new WinningsCalculator(null);
        }

        /// <summary>
        /// Weryfikuje poprawnoœæ liczenia wygranych. Spodziewamy siê, ¿e bêdzie dzia³aæ wed³ug wzoru:
        /// stawka x wartoœæ wygranej dla danego symbolu
        /// </summary>
        [TestMethod]
        public void WinningsCalculator_Calculate()
        {
            Dictionary<int, double> winnings = new Dictionary<int, double>
            {
                { 0, 1 },
                { 1, 12 },
                { 2, 3 },
                { 3, 0.8 },
                { 4, 0.4 },
                { 5, 10 },
                { 6, 99 },
                { 7, 1 }
            };

            IEnumerable<int> symbolsAll = winnings.Keys;

            WinningsCalculator calculator = new WinningsCalculator(winnings);            

            // Weryfikacja dla stawki = 1
            int bet = 1;            
            Dictionary<int, double> actualResults = this.CalculateWinnings(bet, calculator, symbolsAll);                      
            
            foreach(int symbol in symbolsAll)
            {
                double expectedResult = winnings[symbol];
                Assert.AreEqual(expectedResult, actualResults[symbol], $"Invalid result for {symbol}.");
            }

            // Weryifkacja dla stawki = 9
            bet = 9;            
            actualResults = this.CalculateWinnings(bet, calculator, symbolsAll);

            foreach (int symbol in symbolsAll)
            {
                double expectedResult = winnings[symbol] * bet;
                Assert.AreEqual(expectedResult, actualResults[symbol], $"Invalid result for {symbol}.");
            }
        }

        /// <summary>
        /// Helper tworz¹cy s³ownik z wynikami dla podanych symboli.
        /// </summary>
        /// <param name="bet">Stawka dla ka¿dego symbolu</param>
        /// <param name="calculator">Kalkulator licz¹cy wynik</param>
        /// <param name="symbolsAll">Symbole, dla których budujemy wyniki.</param>
        /// <returns>S³ownik z wynikami.</returns>
        private Dictionary<int, double> CalculateWinnings(int bet, WinningsCalculator calculator, IEnumerable<int> symbolsAll)
        {            
            Dictionary<int, double> actualResults = new Dictionary<int, double>();
            foreach (int symbol in symbolsAll)
            {
                actualResults.Add(symbol, calculator.Calculate(bet, symbol));
            }

            return actualResults;
        }
    }
}
