using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace CrazyBandit.Engine.UnitTests
{
    /// <summary>
    /// Test na weryfikację poprawności budowania klasy <see cref="PayLine"/>
    /// </summary>
    [TestClass]
    public class TestPayLine
    {
        /// <summary>
        /// Sprawdza czy linia dostaje odpowiedni index
        /// </summary>
        [TestMethod]
        public void PayLine_Index_Set()
        {
            const int lineIndex = 3;
            PayLine payLine = new PayLine(lineIndex, new int[] { 1, 2, 3 });
            Assert.AreEqual(lineIndex, payLine.Index, "Invalid index set");
        }
        /// <summary>
        /// Sprawdza czy jak zapotamy pustą tablicę to czy poleci oczekiwany wyjątek
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PayLine_Line_Set_Null()
        {
            new PayLine(0, null);
        }

        /// <summary>
        /// Sprawdza czy linia o takich samych symbolach uznawana jest za linię wygrywającą
        /// </summary>
        [TestMethod]
        public void PayLine_Line_IsWinningLine_Symbols_All_Same()
        {
            PayLine payLine = new PayLine(0, new int[] { 1, 1, 1, 1, 1 });
            Assert.IsTrue(payLine.IsWinningLine, "Expected the line is marked as the winning one.");
        }

        /// <summary>
        /// Sprawdza czy linia nie zostanie oznaczona jako wygrywająca jeśli pierwszy symbol jest inny niż reszta
        /// </summary>
        public void PayLine_Line_IsWinningLine_Symbols_Different_First()
        {
            PayLine payLine = new PayLine(1, new int[] { 2, 1, 1, 1, 1 });
            Assert.IsFalse(payLine.IsWinningLine, "It is no winning line - some of the symbols are different");
        }

        /// <summary>
        /// Sprawdza czy jeśli środkowy symbol jest inny niż reszta to linia nie zostanie oznaczona jako wygrywająca
        /// </summary>
        public void PayLine_Line_IsWinningLine_Symbols_Different_Middle()
        {
            PayLine payLine = new PayLine(0, new int[] { 5, 5, 4, 5, 5 });
            Assert.IsFalse(payLine.IsWinningLine, "It is no winning line - some of the symbols are different");
        }

        /// <summary>
        /// Sprawdza czy linia, dla której ostatni symbol jest inny niż poprzednie nie zostanie oznaczona jako wygrywająca
        /// </summary>
        [TestMethod]
        public void PayLine_Line_IsWinningLine_Symbols_Different_Last()
        {
            PayLine payLine = new PayLine(0, new int[] { 1, 1, 1, 1, 5 });
            Assert.IsFalse(payLine.IsWinningLine, "It is no winning line - some of the symbols are different");

        }
    }
}
