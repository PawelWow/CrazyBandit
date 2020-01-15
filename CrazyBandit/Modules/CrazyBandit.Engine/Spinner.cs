using CrazyBandit.Common;
using System.Collections.Generic;

namespace CrazyBandit.Engine
{
    /// <summary>
    /// Klasa pozwalająca zakręcić 
    /// </summary>
    public class Spinner : ISpinner
    {
        /// <summary>
        /// Co ile dany walec ma się zakręcić. Index ma być komatybilny z numerem walca.
        /// </summary>
        private readonly int[] _spin;

        /// <summary>
        /// Do wyszukiwania ułożónej linii w całej kolekcji.
        /// </summary>
        private readonly LinesSearcher _linesSearcher;

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public int[][] Lines
        {
            get;
            private set;
        }

        /// <summary>
        /// Linie, które mogą okazać się wygrywające
        /// </summary>
        public IEnumerable<PayLine> PayLines { get; private set; }

        /// <summary>
        /// Numer zakręcenia - jednocześnie jest to pozycja pierwszej linii wygrywającej.
        /// </summary>
        public int Rno { get; private set; }

        /// <summary>
        /// Konstruktor spinnera
        /// </summary>
        /// <param name="reels">Walce do kręcenia</param>
        public Spinner(SpinnerConfig config)
        {
            Ensure.ParamNotNull(config, nameof(config));
            
            _spin = config.Spin;
            _linesSearcher = new LinesSearcher(config.Lines);

            this.Lines = config.Lines;     
            this.Rno = config.Rno;                                 
            this.PayLines = this.CreateInitialWinningLine(config);            
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Spin()
        {
            List<PayLine> winningLines = new List<PayLine>();
            foreach (var previousWinningLine in this.PayLines)
            {
                int[] line = new int[previousWinningLine.Line.Length];

                for (int reelIndex = 0; reelIndex < previousWinningLine.Line.Length; reelIndex++)
                {
                    // index po zakręceniu bębnem
                    int reelSpinIndex = previousWinningLine.Index + _spin[reelIndex];
                    if (reelSpinIndex > this.Lines.Length)
                    {
                        // Skoro przekręciliśmy licznik - to zaczynamy od nowa
                        reelSpinIndex -= this.Lines.Length;
                    }

                    int reelSymbol = this.Lines[reelSpinIndex][reelIndex];

                    line[reelIndex] = reelSymbol;
                }

                // Mamy całą linię, teraz musimy odszukać ją w tablicy, bo walce mogą kręcić się o różne wartości
                // ważne, żeby zacząć szukać od poprzedniego miejsca

                winningLines.Add(_linesSearcher.Find(line, previousWinningLine.Index));
            }

            this.PayLines = winningLines;
            this.Rno++;
        }

         /// <summary>
        /// Helper tworzący początkowe ustawienia walców. Początkowa pozycja brana jest z <see cref="SpinnerConfig.Rno"/>
        /// </summary>
        /// <param name="config">Konfiguracja spinnera</param>        
        /// <returns>Linie ustalane na początku gry.</returns>
        private IEnumerable<PayLine> CreateInitialWinningLine(SpinnerConfig config)
        {
            List<PayLine> winningLines = new List<PayLine>();

            for (int linesIndex = config.Rno, done = 0; done < config.PayLines; linesIndex++, done++)
            {
                if (linesIndex > this.Lines.Length - 1)
                {
                    //Przekręcamy licznik
                    linesIndex = 0;
                }

                winningLines.Add(new PayLine(linesIndex, config.Lines[linesIndex]));
            }

            return winningLines;
        }
    }
}
