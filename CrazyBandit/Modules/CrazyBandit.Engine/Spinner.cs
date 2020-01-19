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
        /// Walce
        /// </summary>
        private readonly Reel[] _reels;

        /// <summary>
        /// Do wyszukiwania ułożónej linii w całej kolekcji.
        /// </summary>
        private readonly LinesSearcher _linesSearcher;

        private readonly int _payLinesQuantity;

        /// <summary>
        /// Wszystkie możliwe linie
        /// </summary>
        public int[][] Lines
        {
            get;
            private set;
        }

        /// <summary>
        /// Linie, które mogą okazać się wygrywające
        /// </summary>
        public PayLine[] PayLines { get; private set; }

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

            
            _reels = config.Reels;            
            _payLinesQuantity = config.PayLines;            

            this.PayLines = this.CreateInitialWinningLine(config);          
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Spin()
        {

            // TODO ReelsSymbolsOccurance

            Dictionary<int, Dictionary<int, int>> reelSymbolsOccurance = this.CreateSymbolsOccurancesTable();

            PayLine[] winningLines = new PayLine[_payLinesQuantity];

            int[] previousLine = null;

            int lineAttempt = 0;
            // Jeśli danej linii nie udało się zrobić to podbijamy index            
            for (int lineIndex = 0, maxAttemptesAfterFail = this.Lines.Length; lineIndex < _payLinesQuantity; lineIndex++, maxAttemptesAfterFail--)
            {
                PayLine previousPayLine = this.PayLines[lineIndex];
                int[] lineChoosen = new int[previousPayLine.Line.Length];

                for (int reelIndex = 0; reelIndex < previousPayLine.Line.Length; reelIndex++)
                {
                    // index po zakręceniu bębnem (skorygowany o spin). Jeśli ostatnia próba nie powiodła się to korygujemy index o 1         
                    int reelSpinIndex = previousPayLine.Index + _spin[reelIndex] + lineAttempt;
                    if (reelSpinIndex >= this.Lines.Length)
                    {
                        // Skoro przekręciliśmy licznik - to zaczynamy od nowa
                        reelSpinIndex -= this.Lines.Length;
                    }

                    int reelSymbol = this.Lines[reelSpinIndex][reelIndex];

                    lineChoosen[reelIndex] = reelSymbol;


                }

                // Jeśli linia niewalidna i mamy jeszce próby to 
                if (this.IsLineValid(reelSymbolsOccurance, lineChoosen, previousLine) == false && lineAttempt < this.Lines.Length)
                {
                    // To będzie kolejna próba
                    lineAttempt++;

                    lineIndex--;

                    // TODO co z tymi próbami?

                    // Podejmiemy tyle prób, ile jest linii
                    maxAttemptesAfterFail--;

                    // Przechodzimy do następnej linii, bo tej wziąć nie możemy
                    continue;
                }

                // Mamy całą linię, teraz musimy odszukać ją w tablicy, bo walce mogą kręcić się o różne wartości
                // ważne, żeby zacząć szukać od poprzedniego miejsca

                winningLines[lineIndex] = _linesSearcher.Find(lineChoosen, previousPayLine.Index);
                lineAttempt = 0;

                // Podbijamy occurance dla każdego symbolu
                for (int reel = 0; reel < lineChoosen.Length; reel++)
                {
                    int reelSymbol = lineChoosen[reel];
                    reelSymbolsOccurance[reel][reelSymbol]++;
                }

                previousLine = lineChoosen;

            }

            // TODO - kiedy nie będziemy w stanie ułożyć tych linii? Może ilość prób jest tutaj zbędne? Może trzeba założyć mniejsze zło
            // i jeśli jakiś config nie pozwoli nam na wygenerowanie takich liczb to ustawimy przypadkowe wyniki?
            // TODO przeanalizować i opisać algorytm

            this.PayLines = winningLines;
            this.Rno++;
        }



        /// <summary>
        /// Helper tworzący początkowe ustawienia walców. Początkowa pozycja brana jest z <see cref="SpinnerConfig.Rno"/>
        /// </summary>
        /// <param name="config">Konfiguracja spinnera</param>        
        /// <returns>Linie ustalane na początku gry.</returns>
        private PayLine[] CreateInitialWinningLine(SpinnerConfig config)
        {

            PayLine[] winningLines = new PayLine[config.PayLines];

            Dictionary<int, Dictionary<int, int>> reelSymbolsOccurance = this.CreateSymbolsOccurancesTable();

            int[] previousLine = null;
            for (int lineIndex = config.Rno, done = 0, maxAttemptesAfterFail = config.Lines.Length;
                done < config.PayLines && maxAttemptesAfterFail > 0;
                lineIndex++, done++)
            {
                if (lineIndex > config.Lines.Length - 1)
                {
                    //Przekręcamy licznik
                    lineIndex = 0;
                }

                int[] line = config.Lines[lineIndex];
                if (this.IsLineValid(reelSymbolsOccurance, line, previousLine) == false)
                {
                    done--;

                    // Podejmiemy tyle prób, ile jest linii
                    maxAttemptesAfterFail--;

                    // Przechodzimy do następnej linii, bo tej wziąć nie możemy
                    continue;
                }

                // Dodajemy do wyników nową linię
                winningLines[done] = new PayLine(lineIndex, line);
                
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


        /// <summary>
        /// Tworzy wyzerowaną matrycę wystąpień symboli dla wszystkich walców
        /// </summary>
        /// <returns></returns>
        private Dictionary<int, Dictionary<int, int>> CreateSymbolsOccurancesTable()
        {
            Dictionary<int, Dictionary<int, int>> reelSymbolsOccurance = new Dictionary<int, Dictionary<int, int>>();
            for (int reel = 0; reel < _reels.Length; reel++)
            {
                Dictionary<int, int> symbolOccurance = new Dictionary<int, int>();
                foreach (int symbol in _reels[reel].Symbols)
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
        /// Decyduje czy linia może być dodana - jest to określane przez ilość symboli, które powtarzają się na walcu
        /// 
        /// </summary>
        /// <param name="currnetReelSymbolsOccurance"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        private bool IsLineValid(Dictionary<int, Dictionary<int, int>> currnetReelSymbolsOccurance, int[] line, int[] previousLine)
        {
            for (int reel = 0; reel < line.Length; reel++)
            {
                int symbol = line[reel];
                if (currnetReelSymbolsOccurance[reel][symbol] >= _reels[reel].SymbolsOccurance[symbol])
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
