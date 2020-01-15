using CrazyBandit.Engine.Config;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrazyBandit.Engine.UnitTests
{
    /// <summary>
    /// Testy weryfikujące poprawność działania klasy <see cref="Spinner"/>
    /// </summary>
    [TestClass]
    public class TestSpinner
    {
        /// <summary>
        /// Sprawdza czy nie uda się utworzyć obiektu z nullowym configiem
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Spinner_Config_Null()
        {
            new Spinner(null);
        }

        /// <summary>
        /// Sprawdza czy obiekt tworzy się prawidłowo
        /// </summary>
        [TestMethod]
        public void Spinner_Create()
        {
            const int initialRno = 3;
            const int initialPayLinesCount = 1;

            int[] reel1 = new int[] { 4, 5, 6 };
            int[] reel2 = new int[] { 7, 9 };
            int[] reel3 = new int[] { 10, 11, 12, 15 };

            Reel[] reels = new Reel[]
            {
                new Reel(reel1, 1),
                new Reel(reel2, 1),
                new Reel(reel3, 1)
            };

            SpinnerConfig config = new SpinnerConfig(reels, new RnoConfig(initialRno), initialPayLinesCount);

            Spinner spinner = new Spinner(config);

            Assert.AreEqual(initialRno, spinner.Rno, "Wrong number of RNO");
            Assert.AreEqual(initialPayLinesCount, spinner.PayLines.Count(), "Invalid number of paylines.");
            Assert.AreEqual(config.Lines.Length, spinner.Lines.Length, "Spinner should have the same amunt of lines as config.");
           

        }

        /// <summary>
        /// Weryfikuje czy po skonstruowaniu spinnera linie (kombinacje) ułożył się w oczekiwany sposób.
        /// TODO Ten test, jak i inne testy na tworzenie linni ma trafić do klasy testującej SpinerConfig tutaj co najwyżej czy mamy linie
        /// </summary>
        [TestMethod]
        public void Spinner_Create_Lines()
        {
            const int initialRno = 3;

            int[] reel1 = new int[] { 4, 5, 6 };
            int[] reel2 = new int[] { 7, 9 };
            int[] reel3 = new int[] { 10, 11, 12, 15 };

            Reel[] reels = new Reel[]
            {
                new Reel(reel1, 1),
                new Reel(reel2, 1),
                new Reel(reel3, 1)
            };

            // Takich linii się spodziewamy - dokładnie w tej kolejności
            int[][] linesExpected = new int[][]
            {
                new int[] { 4, 7, 10 },
                new int[] { 4, 7, 11 },
                new int[] { 4, 7, 12 },
                new int[] { 4, 7, 15 },
                new int[] { 4, 9, 10 },
                new int[] { 4, 9, 11 },
                new int[] { 4, 9, 12 },
                new int[] { 4, 9, 15 },
                new int[] { 5, 7, 10 },
                new int[] { 5, 7, 11 },
                new int[] { 5, 7, 12 },
                new int[] { 5, 7, 15 },
                new int[] { 5, 9, 10 },
                new int[] { 5, 9, 11 },
                new int[] { 5, 9, 12 },
                new int[] { 5, 9, 15 },
                new int[] { 6, 7, 10 },
                new int[] { 6, 7, 11 },
                new int[] { 6, 7, 12 },
                new int[] { 6, 7, 15 },
                new int[] { 6, 9, 10 },
                new int[] { 6, 9, 11 },
                new int[] { 6, 9, 12 },
                new int[] { 6, 9, 15 },
            };

            Spinner spinner = new Spinner(new SpinnerConfig(reels, new RnoConfig(initialRno), Defs.PayLines));
            Assert.AreEqual(initialRno, spinner.Rno, "Invalid rno.");

            for (int i = 0; i < linesExpected.Length; i++)
            {
                CollectionAssert.AreEqual(linesExpected[i], spinner.Lines[i], $"Invalid line {i}");
            }

        }

        /// <summary>
        /// Sprawdza czy spinner zakręca o 1 symbol
        /// </summary>
        [TestMethod]
        public void Spinner_Spin_One()
        {
            const int spinForAll = 1;
            int[] reel1 = new int[] { 1, 2, 3 };
            int[] reel2 = new int[] { 4, 5, 6 };
            int[] reel3 = new int[] { 7, 8, 9 };

            Reel[] reels = new Reel[]
            {
                new Reel(reel1, spinForAll),
                new Reel(reel2, spinForAll),
                new Reel(reel3, spinForAll),

            };

            const int initialRno = 0;

            // rno ustawiamy na 0 - pierwszy element
            Spinner spinner = new Spinner(new SpinnerConfig(reels, new RnoConfig(initialRno), Defs.PayLines));
            Assert.AreEqual(initialRno, spinner.Rno);

            Assert.AreEqual(9, spinner.Lines.Length, "Invalid number of lines provided to the spinner.");

            // TODO wynik danego spina mozna jakoś stestować + wyniki drugiego spina

        }

        #region Advanced

        /// <summary>
        /// Test sprawdza czy liczba kombinacji każdego z symboli jest zgodna z oczekiwaniami.
        /// Test automatycznie sam liczy, ale może być trudny w interpretacji. Jest jednak łatwiejszy do wykonania na bardziej 
        /// skomplikowanych danych.
        /// </summary>
        [TestMethod]
        public void Spinner_Create_Lines_AdvancedTest()
        {
            const int initialRno = 3;

            int[] reel1 = new int[] { 4, 5, 6 };
            int[] reel2 = new int[] { 7, 9 };
            int[] reel3 = new int[] { 10, 11, 12, 15 };

            Reel[] reels = new Reel[]
            {
                new Reel(reel1, 1),
                new Reel(reel2, 1),
                new Reel(reel3, 1)
            };


            Spinner spinner = new Spinner(new SpinnerConfig(reels, new RnoConfig(initialRno), Defs.PayLines));
            Assert.AreEqual(initialRno, spinner.Rno, "Invalid rno.");

            // ilośc kombinacji dla danego symbolu s1: liczba symboli walca 2 x ... x liczba symboli walca n
            // 

            int[][] reelsArray = this.ReelsToArray(reels);
            for (int currentReel = 0; currentReel < reelsArray.Length; currentReel++)
            {
                this.AssertSymbolsCombinationExceptCurrentOne(reelsArray, spinner.Lines, currentReel);
            }
        }

        /// <summary>
        /// Sprawdza czy liczba kombinacji każdego symbolu każdego walca 
        /// </summary>
        /// <param name="reelsInitial"></param>
        /// <param name="spinnerLines"></param>
        /// <param name="currentReel"></param>
        private void AssertSymbolsCombinationExceptCurrentOne(int[][] reelsInitial, int[][] spinnerLines, int currentReel)
        {
            for (int currentSymbol = 0; currentSymbol < reelsInitial[currentReel].Length; currentSymbol++)
            {
                int combinationsExpected = this.CountSymbolCombinations(reelsInitial, currentReel);
                int combinationsActual = spinnerLines.Count(s => s[currentReel] == reelsInitial[currentReel][currentSymbol]);
                Assert.AreEqual(combinationsExpected, combinationsActual, $"Invalid combination for reel: {currentReel}, symbol: {currentSymbol}");
            }
        }

        /// <summary>
        /// Konwertuje tablicę <see cref="Reel"/> do tablicy int[numer_walca][symbole_walca] 
        /// </summary>
        /// <param name="reels"></param>
        /// <returns></returns>
        private int[][] ReelsToArray(Reel[] reels)
        {
            int[][] reelsArray = new int[reels.Length][];
            for (int i = 0; i < reels.Length; i++)
            {
                reelsArray[i] = reels[i].Symbols;
            }

            return reelsArray;
        }

        /// <summary>
        /// Oblicza ilość kombinacji symboli z pominięciem danego walca.
        /// Ilość kombinacji osiąga się mnożąc ilość symboli każdego kolejnego walca (ilość symboli walca1 x ... x ilość symboli walcaN).
        /// </summary>
        /// <param name="reels">Wszystkie walce</param>
        /// <param name="excludedReel">Numer walca, który chcemy pominąć.</param>
        /// <returns>Liczba kombinacji</returns>
        private int CountSymbolCombinations(int[][] reels, int excludedReel)
        {
            // Dajemy liczbę kombinacji 1, bo liczymy kombinację 1 symboli, który wiemy, że występuje
            int combinations = 1;
            for (int i = 0; i < reels.Length; i++)
            {
                if (i == excludedReel)
                {
                    continue;
                }

                combinations *= reels[i].Length;
            }

            return combinations;
        }

        #endregion
    }
}
