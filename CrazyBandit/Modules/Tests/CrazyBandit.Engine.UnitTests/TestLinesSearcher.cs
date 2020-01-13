using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrazyBandit.Engine.UnitTests
{
    /// <summary>
    /// Testy weryfikujące poprawność działania klasy <see cref="LinesSearcher"/>
    /// </summary>
    [TestClass]
    public class TestLinesSearcher
    {
        /// <summary>
        /// Sprawdza czy poleci wyjątek jeśli zapodamy nullowy zestaw linii
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LinesSearcher_Lines_Null_Fails()
        {
            new LinesSearcher(null);
        }

        /// <summary>
        /// Sprawdza czy poleci odpowiedni wyjątek jeśli tablica jest pusta
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void LinesSearcher_Lines_Empty_Fails()
        {
            new LinesSearcher(new int[][] { });
        }

        /// <summary>
        /// Sprawdza czy poleci odpowiedni wyjątek jeśli tablica jes
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void LinesSearcher_Lines_One_Lome_Null_Fails()
        {
            new LinesSearcher(new int[][] { new int[] { 1 }, null });
        }

        /// <summary>
        /// Sprawdza czy poleci odpowiedni wyjątek jeśli tablica jes
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void LinesSearcher_Lines_One_Lome_Empty_Fails()
        {
            new LinesSearcher(new int[][] { new int[] { 1 }, new int[] { } });
        }

        /// <summary>
        /// Sprawdza czy poleci wyjątek jeśli spróbujemy wyszukać linię, która nie istnieje w matrycy
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void LinesSearcher_Lines_Find_NotFound()
        {
            int[] line1 = new int[] { 1, 2, 3, 4, 5 };
            int[] line2 = new int[] { 1, 2, 3, 6, 7 };
            int[] line3 = new int[] { 6, 7, 8, 9, 10 };

            int[][] linesAll = new int[][]
            {
                line1,
                line2,
                line3
            };

            LinesSearcher searcher = new LinesSearcher(linesAll);
            searcher.Find(new int[] { 4, 4, 4, 4, 4});            
        }

        /// <summary>
        /// Sprawdza czy linia zostanie znaleziona w całej kolekcji linii
        /// </summary>
        [TestMethod]
        public void LinesSearcher_Lines_Find()
        {
            int[] line1 = new int[] { 1, 2, 3, 4, 5 };
            int[] line2 = new int[] { 1, 2, 3, 6, 7 };
            int[] line3 = new int[] { 6, 7, 8, 9, 10 };

            int[][] linesAll = new int[][]
            {
                line1,
                line2,
                line3
            };

            LinesSearcher searcher = new LinesSearcher(linesAll);
            PayLine line1Found = searcher.Find(line1);
            PayLine line2Found = searcher.Find(line2);
            PayLine line3Found = searcher.Find(line3);

            CollectionAssert.AreEqual(line1, line1Found.Line, "Invalid search result for line1");
            CollectionAssert.AreEqual(line2, line2Found.Line, "Invalid search result for line2");
            CollectionAssert.AreEqual(line3, line3Found.Line, "Invalid search result for line3");
        }

        /// <summary>
        /// Sprawdza czy znajdziemy linie w kolekcji unikalnych linii, jeśli wyszukiwanie rozpocznie się po zadanej linii (przekręcamy licznik)
        /// </summary>
        [TestMethod]
        public void LinesSearcher_Lines_Unique_Find_Starts_After_Line()
        {
            int[] line1 = new int[] { 1, 2, 3, 4, 5 };
            int[] line2 = new int[] { 1, 2, 3, 6, 7 };
            int[] line3 = new int[] { 6, 7, 8, 9, 10 };

            int[][] linesAll = new int[][]
            {
                line1,
                line2,
                line3,
                new int[] { 1, 2, 3, 9, 19 }
            };

            LinesSearcher searcher = new LinesSearcher(linesAll);
            PayLine line1Found = searcher.Find(line1, 1);
            PayLine line2Found = searcher.Find(line2, 2);
            PayLine line3Found = searcher.Find(line3, 3);

            CollectionAssert.AreEqual(line1, line1Found.Line, "Invalid search result for line1");
            CollectionAssert.AreEqual(line2, line2Found.Line, "Invalid search result for line2");
            CollectionAssert.AreEqual(line3, line3Found.Line, "Invalid search result for line3");
        }

        /// <summary>
        /// Sprawdza czy prawidłowo odnajdywane są numery (indexy) linii. W matrycy linie mogą się powtarzać, tutaj szukamy od początku, 
        /// więc oczekujemy znalezienia linii o pierwszym wystąpieniu
        /// </summary>
        [TestMethod]
        public void LinesSearcher_Lines_Find_Index()
        {
            int[] line1 = new int[] { 1, 2, 3, 4, 5 };
            int[] line2 = new int[] { 1, 2, 3, 6, 7 };
            int[] line3 = new int[] { 6, 7, 8, 9, 10 };

            int[][] linesAll = new int[][]
            {
                line1,
                line2,
                line3,
                line1,
                line2,
                line3
            };

            LinesSearcher searcher = new LinesSearcher(linesAll);
            PayLine line1Found = searcher.Find(line1);
            PayLine line2Found = searcher.Find(line2);
            PayLine line3Found = searcher.Find(line3);

            Assert.AreEqual(0, line1Found.Index, "Invalid index of first line");
            Assert.AreEqual(1, line2Found.Index, "Invalid index of second line");
            Assert.AreEqual(2, line3Found.Index, "Invalid index of third line");
        }

        /// <summary>
        /// Weryfikuje czy jeśli wyszukiwanie rozpocznie się po pierwszym wystąpieniu wyszukiwanej linii to czy zostanie odnaleziony index
        /// drugiego wystąpienia szukanej linii w tej kolekcji.
        /// </summary>
        public void LinesSearcher_Lines_Find_Index_Starts_After_first_Line()
        {
            int[] line1 = new int[] { 1, 2, 3, 4, 5 };
            int[] line2 = new int[] { 1, 2, 3, 6, 7 };
            int[] line3 = new int[] { 6, 7, 8, 9, 10 };

            int[][] linesAll = new int[][]
            {
                line1,
                line2,
                line3,
                line1,
                line2,
                line3,
                
            };

            LinesSearcher searcher = new LinesSearcher(linesAll);
            PayLine line1Found = searcher.Find(line1, 1);
            PayLine line2Found = searcher.Find(line2, 2);
            PayLine line3Found = searcher.Find(line3, 3);

            Assert.AreEqual(3, line1Found.Index, "Invalid index of first line");
            Assert.AreEqual(4, line2Found.Index, "Invalid index of second line");
            Assert.AreEqual(5, line3Found.Index, "Invalid index of third line");
        }

        /// <summary>
        /// Weryfikuje czy jeśli wyszukiwanie rozpocznie się po drugim wystąpieniu wyszukiwanej linii to czy zostanie odnalezione pierwsze
        /// wystąpienie linii na tej matrycy (licznik się przekręca, zaczynamy od nowa)
        /// </summary>
        [TestMethod]
        public void LinesSearcher_Lines_Find_Index_Starts_After_Second_Line()
        {
            int[] line1 = new int[] { 1, 2, 3, 4, 5 };
            int[] line2 = new int[] { 1, 2, 3, 6, 7 };
            int[] line3 = new int[] { 6, 7, 8, 9, 10 };

            int[][] linesAll = new int[][]
            {
                line1,
                line2,
                line3,
                line1,
                line2,
                line3,
                new int[] { 1, 2, 3, 9, 19 }
            };

            LinesSearcher searcher = new LinesSearcher(linesAll);
            PayLine line1Found = searcher.Find(line1, 4);
            PayLine line2Found = searcher.Find(line2, 5);
            PayLine line3Found = searcher.Find(line3, 6);

            Assert.AreEqual(0, line1Found.Index, "Invalid index of first line");
            Assert.AreEqual(1, line2Found.Index, "Invalid index of second line");
            Assert.AreEqual(2, line3Found.Index, "Invalid index of third line");
        }


    }
}
