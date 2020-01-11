using Microsoft.VisualStudio.TestTools.UnitTesting;
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

            // TODO DOK - więcej testów
            Assert.AreEqual(72, composer.Lines.Count());

        }
    }
}
