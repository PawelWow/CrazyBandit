using CrazyBandit.Common;
using CrazyBandit.Engine.Config;
using System.Collections.Generic;
using System.Linq;

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

        // TODO - dokończyć sprawę z builderem - ma być czy nie? Przy spinie jest za male zróżnicowanie znaków
        // TODO - trebaby zrobić 
        PayLinesBuilder _linesBuilder;

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

            _linesBuilder = new PayLinesBuilder(config.Lines, config.PayLines, config.Reels);

            this.PayLines = _linesBuilder.GetPayLines(config.Rno);          
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
                    // index po zakręceniu bębnem (skorygowany o spin)

                    //  TODO Większą różnorodność trzeba zrobić, albo coś nie tak jest z wybieraniem linii

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
                if (linesIndex > config.Lines.Length - 1)
                {
                    //Przekręcamy licznik
                    linesIndex = 0;
                }

                winningLines.Add(new PayLine(linesIndex, config.Lines[linesIndex]));
            }

            return winningLines;
        }



        // ile razy dany symbol moze wystąpić na danym walcu?

        /// <summary>
        /// Buduje linie wygrywające zgodnie z możliwościami walca
        /// </summary>
        private class PayLinesBuilder
        {
            private readonly Reel[] _reels;
            
 
            private readonly int[][] _lines;

            /// <summary>
            /// Dopuszczalna ilość linii
            /// </summary>
            private readonly int _linesQuantity;

            public PayLinesBuilder(int[][] lines, int linesQuantity, Reel[] reels)
            {
                
                _lines = lines;
                _linesQuantity = linesQuantity;
                _reels = reels;
            }

            /// <summary>
            /// Tworzy wyzerowaną matrycę wystąpień symboli dla wszystkich walców
            /// </summary>
            /// <returns></returns>
            private Dictionary<int, Dictionary<int, int>> CreateSymbolsOccurancesTable()
            {
                Dictionary<int, Dictionary<int, int>> reelSymbolsOccurance = new Dictionary<int, Dictionary<int, int>>();
                for (int reel = 0; reel < this._reels.Length; reel++)
                {
                    Dictionary<int, int> symbolOccurance = new Dictionary<int, int>();
                    foreach (int symbol in this._reels[reel].Symbols)
                    {
                        if (symbolOccurance.ContainsKey(symbol) == false)
                        {
                            symbolOccurance.Add(symbol, 0);
                        }
                    }

                    reelSymbolsOccurance.Add(reel, symbolOccurance);
                }

                return reelSymbolsOccurance;
            }

            /// <summary>
            /// Bierze pożądaną ilość linii począwszy od zadanego indexu
            /// </summary>
            /// <param name="startIndex"></param>
            /// <returns></returns>
            public IEnumerable<PayLine> GetPayLines(int startIndex)
            {
                List<PayLine> winningLines = new List<PayLine>();
                
                Dictionary<int, Dictionary<int, int>> reelSymbolsOccurance = this.CreateSymbolsOccurancesTable();

                int[] previousLine = null;
                for (int lineIndex = startIndex, done = 0, maxAttemptesAfterFail = _lines.Length; 
                    done < _linesQuantity && maxAttemptesAfterFail > 0; 
                    lineIndex++, done++)
                {
                    if (lineIndex > _lines.Length - 1)
                    {
                        //Przekręcamy licznik
                        lineIndex = 0;
                    }

                    int[] line = _lines[lineIndex];
                    if (this.IsLineValid(reelSymbolsOccurance, line, previousLine) == false)
                    {                        
                        done--;

                        // Podejmiemy tyle prób, ile jest linii
                        maxAttemptesAfterFail--;

                        // Przechodzimy do następnej linii, bo tej wziąć nie możemy
                        continue;
                    }

                    // Dodajemy do wyników
                    winningLines.Add(new PayLine(lineIndex, line));

                    // Podbijamy occurance dla każdego symbolu
                    for (int reel = 0; reel < line.Length; reel++)
                    {
                        int reelSymbol = line[reel];
                        reelSymbolsOccurance[reel][reelSymbol]++;
                    }

                    previousLine = line;
                }

                return winningLines;
            }

            // REV swoją drogą możliwe, że powinniśmy tutaj też brać pod uwagę kolejność symboli na walcach.

            /// <summary>
            /// Decyduje czy linia może być dodana - jest to określane przez ilość symboli, które powtarzają się na walcu
            /// 
            /// </summary>
            /// <param name="reelSymbolsOccurance"></param>
            /// <param name="line"></param>
            /// <returns></returns>
            private bool IsLineValid(Dictionary<int, Dictionary<int, int>> reelSymbolsOccurance, int[] line, int[] previousLine)
            {
                for (int reel = 0; reel < line.Length; reel++)
                {
                    int symbol = line[reel];
                    if (reelSymbolsOccurance[reel][symbol] >= _reels[reel].SymbolsOccurance[symbol])
                    {
                        return false;

                    }

                    if (previousLine != null && previousLine[reel] == symbol)
                    {
                        //  Nie chcemy powtarzać tych samych symboli
                        return false;
                    }
                }

                return true;
            }
        }
    }
}
