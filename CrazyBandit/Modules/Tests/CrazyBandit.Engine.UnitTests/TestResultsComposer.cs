using CrazyBandit.Engine.Config;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CrazyBandit.Engine.UnitTests
{
    /// <summary>
    /// Klasa reprezentująca testy kompozytora wyników (lini ułożenia walców)
    /// </summary>
    [TestClass]
    public class TestResultsComposer
    {
        /// <summary>
        /// Test sprawdza czy kombinacje dobrze są liczone dla walców o różnej długości, jeśli pierwszy walce ma najwięcej elementów
        /// </summary>
        [TestMethod]
        public void ResultsComposer_Reels_Symbols_Descending()
        {
            Reel[] reels = new Reel[]
            {
                new Reel(new int[]{ 0, 1, 3}, 1),
                new Reel(new int[]{ 6, 1}, 1),
                new Reel(new int[]{ 5 }, 1),               
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
                new Reel(new int[]{ 5 }, 1),
                new Reel(new int[]{ 6, 1}, 1),
                new Reel(new int[]{ 0, 1, 3}, 1),
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
            int[] reel1Symbols = new int[] { 0, 1, 2, 3, 4, 4, 4, 5, 6, 7, 7, 0, 0, 2, 2, 3 };
            int[] reel2Symbols = new int[] { 7, 7, 7, 6, 6, 6, 5, 5, 5, 4, 3, 2, 2, 2, 1, 1, 1, 0, 0, 0, 2, 3, 4, 7 };
            int[] reel3Symbols = new int[] { 0, 1, 0, 2, 0, 3, 4, 5, 6, 6, 6, 6, 6, 5, 5, 5, 1, 0, 1, 2, 3, 4, 1, 1, 1, 0, 0, 7, 7, 7, 5 };

            Reel[] reels = new Reel[]
            {            
                new Reel(reel1Symbols, 1),
                new Reel(reel2Symbols, 1),
                new Reel(reel3Symbols, 1),
            };

            ResultsComposer composer = new ResultsComposer(reels);
                       
            // 16 x 24 x 31
            Assert.AreEqual(11904, composer.Lines.Count());
        }
        
        /// <summary>
        /// Sprawdza czy w wynikach udaje się ułożyć linie o takich samych wartościach
        /// </summary>
        [TestMethod]
        public void ResultsComposer_Do_Winning_Lines_Exist()
        {
            Reel[] reels = new Reel[]
            {
                new Reel(new int[]{ 0, 1, 5}, 1),
                new Reel(new int[]{ 5, 1}, 1),
                new Reel(new int[]{ 5, 2, 1}, 1),
                new Reel(new int[]{ 7, 5, 1, 6}, 1),

            };

            ResultsComposer composer = new ResultsComposer(reels);
            Assert.AreEqual(2, composer.Lines.Count(this.IsWinningLine), "No expected winning line.");
        }

        /// <summary>
        /// Sprawdza czy walce prawidłowo transformowane są do linii 
        /// czyli miałem: 1,2,3
        /// to teraz mam:
        /// 1
        /// 2
        /// 3
        /// </summary>
        [TestMethod]
        public void ResultsComposer_IsAppriopriate_Order()
        {
            int[] first = new int[] { 1, 2, 3, 4 };
            int[] second = new int[] { 4, 6, 5, 7 };
            int[] third = new int[] { 8, 9, 10, 11 };

            Reel[] reels = new Reel[]
            {
                new Reel(first, 1),
                new Reel(second, 1),
                new Reel(third, 1),
            };

            ResultsComposer composer = new ResultsComposer(reels);

            for (int i = 0; i < composer.Lines.Length; i++)
            {
                Assert.AreEqual(reels.Length, composer.Lines[i].Length, $"Invalid row {i} size.");
                Assert.IsTrue(first.Any(symbol => symbol == composer.Lines[i][0]), $"Invalid value of 1st reel: '{composer.Lines[i][0]}', row: {i}");
                Assert.IsTrue(second.Any(symbol => symbol == composer.Lines[i][1]), $"Invalid value of 2st reel: '{composer.Lines[i][1]}', row: {i}");
                Assert.IsTrue(third.Any(symbol => symbol == composer.Lines[i][2]), $"Invalid value of 3rd reel: '{composer.Lines[i][2]}', row: {i}");               
            }
        }

        /// <summary>
        /// Czy to jest linia wygrywająca, czyli taka, która ma wszystkie elementy (symbole) takie same?
        /// </summary>
        /// <param name="line">Linia, którą sprawdzamy</param>
        /// <returns>True, jeśli jest to linia zwycięska.</returns>
        private bool IsWinningLine(int[] line)
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

        /// <summary>
        /// Helper sprawdzający czy linie są unikalne. Tylko dla testów, w których spodziewamy się unikalnośći każdego z wyników
        /// (!) Mają być unikalne, jeśli każdy walec będzie miał unikalną listę symboli.
        /// </summary>
        /// <param name="linesVerified">Zestaw linii do weryfikacji</param>
        private void AssertLinesUnique(IEnumerable<int[]> linesVerified)
        {
            foreach (int[] line in linesVerified)
            {            
                int linesEqual = linesVerified.Count(l => Enumerable.SequenceEqual(line, l));
                string lineValues = string.Join(", ", line);
                Assert.AreEqual(1, linesEqual, $"The line is not unique in collection: '{lineValues}'");
            }
        }
    }
}
