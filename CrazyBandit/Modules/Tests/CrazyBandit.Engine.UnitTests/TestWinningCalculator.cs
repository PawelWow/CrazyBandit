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
            Dictionary<Symbol, float> winnings = new Dictionary<Symbol, float>
            {
                { Symbol.B, 1 },
                { Symbol.Cherry, 12 },
                { Symbol.Dollar, 3 },
                { Symbol.Horseshoe, 0.8f },
                { Symbol.Joker, 0.4f },
                { Symbol.MoneyBag, 10 },
                { Symbol.Paragraph, 99 },
                { Symbol.Seven, 1 }
            };

            IEnumerable<Symbol> symbolsAll = Enum.GetValues(typeof(Symbol)).Cast<Symbol>();

            WinningsCalculator calculator = new WinningsCalculator(winnings);            

            // Weryfikacja dla stawki = 1
            int bet = 1;            
            Dictionary<Symbol, float> actualResults = this.CalculateWinnings(bet, calculator, symbolsAll);                      
            
            foreach(Symbol symbol in symbolsAll)
            {
                float expectedResult = winnings[symbol];
                Assert.AreEqual(expectedResult, actualResults[symbol], $"Invalid result for {symbol}.");
            }

            // Weryifkacja dla stawki = 9
            bet = 9;            
            actualResults = this.CalculateWinnings(bet, calculator, symbolsAll);

            foreach (Symbol symbol in symbolsAll)
            {
                float expectedResult = winnings[symbol] * bet;
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
        private Dictionary<Symbol, float> CalculateWinnings(int bet, WinningsCalculator calculator, IEnumerable<Symbol> symbolsAll)
        {            
            Dictionary<Symbol, float> actualResults = new Dictionary<Symbol, float>();
            foreach (var item in symbolsAll)
            {
                actualResults.Add(item, calculator.Calculate(bet, item));
            }

            return actualResults;
        }
    }
}
