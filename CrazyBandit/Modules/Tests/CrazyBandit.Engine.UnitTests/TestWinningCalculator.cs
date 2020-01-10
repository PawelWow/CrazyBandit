using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CrazyBandit.Engine.UnitTests
{
    [TestClass]
    public class TestWinningCalculator
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WinningsCalculator_create_null()
        {
            new WinningsCalculator(null);
        }
    }
}
