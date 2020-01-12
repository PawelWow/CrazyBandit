using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CrazyBandit.Engine.UnitTests
{
    /// <summary>
    /// Klasa reprezentująca testy kompozytora wyników (lini ułożenia walców)
    /// </summary>
    [TestClass]
    public class TestResultsComposer
    {
        [TestMethod]
        public void ResultsComposer_SimpleArray()
        {
            Reel[] reels = new Reel[]
            {
                new Reel(new Symbol[]{ Symbol.B, Symbol.Dollar, Symbol.Cherry}, 1),
                new Reel(new Symbol[]{ Symbol.Joker, Symbol.Dollar}, 1),
                new Reel(new Symbol[]{ Symbol.MoneyBag, Symbol.Paragraph, Symbol.Dollar}, 1),
                new Reel(new Symbol[]{ Symbol.Seven, Symbol.MoneyBag, Symbol.Dollar, Symbol.Joker}, 1),

            };

            ResultsComposer composer = new ResultsComposer(reels);

            // Mnożymy wszystkie kombinacje walców
            Assert.AreEqual(72, composer.Lines.Count());
            this.AssertLinesUnique(composer.Lines);
        }

        /// <summary>
        /// Test sprawdza czy kombinacje dobrze są liczone dla walców o różnej długości, jeśli pierwszy walce ma najwięcej elementów
        /// </summary>
        [TestMethod]
        public void ResultsComposer_Reels_Symbols_Descending()
        {
            Reel[] reels = new Reel[]
            {
                new Reel(new Symbol[]{ Symbol.B, Symbol.Dollar, Symbol.Cherry}, 1),
                new Reel(new Symbol[]{ Symbol.Joker, Symbol.Dollar}, 1),
                new Reel(new Symbol[]{ Symbol.MoneyBag }, 1),               
            };

            ResultsComposer composer = new ResultsComposer(reels);

            // Mnożymy wszystkie kombinacje walców
            Assert.AreEqual(6, composer.Lines.Count());
            this.AssertLinesUnique(composer.Lines);
        }

        /// <summary>
        /// Sprawdza czy jeśli mamy rosnącą liczbę symboli na poszczególnych walcach to czy wyniki liczone są zgodnie  z oczekiwaniami
        /// </summary>
        [TestMethod]
        public void ResultsComposer_Reels_Symbols_Ascending()
        {
            Reel[] reels = new Reel[]
            {
                new Reel(new Symbol[]{ Symbol.MoneyBag }, 1),
                new Reel(new Symbol[]{ Symbol.Joker, Symbol.Dollar}, 1),
                new Reel(new Symbol[]{ Symbol.B, Symbol.Dollar, Symbol.Cherry}, 1),            
            };

            ResultsComposer composer = new ResultsComposer(reels);

            // Mnożymy wszystkie kombinacje walców
            Assert.AreEqual(6, composer.Lines.Count());
            this.AssertLinesUnique(composer.Lines);
        }

        /// <summary>
        /// Sprawdza czy zostanie skomponowana prawidłowa liczba kombinacji walców defaultowych programu
        /// </summary>
        [TestMethod]
        public void ResultsComposer_Reels_Default()
        {
            int[] reel1Values = new int[] { 0, 1, 2, 3, 4, 4, 4, 5, 6, 7, 7, 0, 0, 2, 2, 3 };
            int[] reel2Values = new int[] { 7, 7, 7, 6, 6, 6, 5, 5, 5, 4, 3, 2, 2, 2, 1, 1, 1, 0, 0, 0, 2, 3, 4, 7 };
            int[] reel3Values = new int[] { 0, 1, 0, 2, 0, 3, 4, 5, 6, 6, 6, 6, 6, 5, 5, 5, 1, 0, 1, 2, 3, 4, 1, 1, 1, 0, 0, 7, 7, 7, 5 };

            Reel[] reels = new Reel[]
            {            
                new Reel(this.ConvertToSymbols(reel1Values), 1),
                new Reel(this.ConvertToSymbols(reel2Values), 1),
                new Reel(this.ConvertToSymbols(reel3Values), 1),
            };

            ResultsComposer composer = new ResultsComposer(reels);
            // 16 x 24 x 31
            Assert.AreEqual(11904, composer.Lines.Count());
        }

        /// <summary>
        /// Zamienia tablicę intów na odpowiadającą jej tablicę symboli
        /// </summary>
        /// <param name="numbers">numery symboli</param>
        /// <returns>Tablica symboli</returns>
        private Symbol[] ConvertToSymbols(int[] numbers)
        {
            return Array.ConvertAll(numbers, i => (Symbol)i);
        }

        /// <summary>
        /// Helper sprawdzający czy linie są unikalne. Mają być unikalne, jeśli każdy walec będzie miał unikalną listę symboli.
        /// </summary>
        /// <param name="linesVerified">Zestaw linii do weryfikacji</param>
        private void AssertLinesUnique(IEnumerable<Symbol[]> linesVerified)
        {
            foreach (Symbol[] line in linesVerified)
            {
                int linesEqual = linesVerified.Count(l => Enumerable.SequenceEqual(line, l));
                string lineValues = string.Join(", ", line);
                Assert.AreEqual(1, linesEqual, $"The line is not unique in collection: '{lineValues}'");
            }
        }
    }
}
