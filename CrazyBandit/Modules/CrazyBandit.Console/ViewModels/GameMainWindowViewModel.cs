using CrazyBandit.Common;
using CrazyBandit.Engine;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CrazyBandit.Console.ViewModels
{
    /// <summary>
    /// View model gry (okna <see cref="Views.GameMainWindow"/>
    /// </summary>
    internal class GameMainWindowViewModel : Observed
    {       
        /// <summary>
        /// Model gry
        /// </summary>
        private readonly Game _gameModel;

        /// <summary>
        /// Ile linii widocznych na ekranie
        /// </summary>
        private readonly int _visibleLinesQuantity = 3;

        /// <summary>
        /// Mówi czy gra jest aktualnie w toku.
        /// </summary>
        private bool _isGameRunning;

        /// <summary>
        /// Symbole na walcu 1
        /// </summary>
        private ObservableCollection<Symbol> _reel1;

        /// <summary>
        /// Symbole na walcu 2
        /// </summary>
        private ObservableCollection<Symbol> _reel2;

        /// <summary>
        /// Symbole na walcu 3
        /// </summary>
        private ObservableCollection<Symbol> _reel3;

        /// <summary>
        /// Czy linia numer 1 jest zwycięska
        /// </summary>
        private bool _isPayLine1;

        /// <summary>
        /// Czy linia numer 2 jest zwycięska
        /// </summary>
        private bool _isPayLine2;

        /// <summary>
        /// Czy linia numer 3 jest zwycięska
        /// </summary>
        private bool _isPayLine3;

        /// <summary>
        /// Aktualny stan konta
        /// </summary>
        private double _balance;

        /// <summary>
        /// Aktualna wygrana
        /// </summary>
        private double _currentWin;

        /// <summary>
        /// Czy następuje pobieranie wypłaty?
        /// </summary>
        private bool _isCollecting;

        /// <summary>
        /// <see cref="_isGameRunning"/>
        /// </summary>
        public bool IsGameRunning
        {
            get
            {
                return _isGameRunning;
            }
            set
            {
                _isGameRunning = value;
                base.OnPropertyChange(nameof(this.IsGameRunning), nameof(this.IsStartPossible), nameof(this.IsPayoutAvailable));
            }
        }

        /// <summary>
        /// <inheritdoc cref="_balance"/>
        /// </summary>
        public double Balance
        {
            get
            {
                return _balance;
            }
            set
            {
                _balance = value;
                base.OnPropertyChange(nameof(this.Balance));
            }
        }

        /// <summary>
        /// <inheritdoc cref="_currentWin"/>
        /// </summary>
        public double CurrentWin
        {
            get
            {
                return _currentWin;
            }
            set
            {
                _currentWin = value;
                base.OnPropertyChange(nameof(this.CurrentWin));
            }
        }

        /// <summary>
        /// Czy pierwsza linia zwycięzyła
        /// </summary>
        public bool IsPayLine1
        {
            get
            {
                return _isPayLine1;                
            }
            set
            {
                _isPayLine1 = value;
                base.OnPropertyChange(nameof(IsPayLine1));
            }
        }

        /// <summary>
        /// Czy pierwsza linia zwycięzyła
        /// </summary>
        public bool IsPayLine2
        {
            get
            {
                return _isPayLine2;
            }
            set
            {
                _isPayLine2 = value;
                base.OnPropertyChange(nameof(IsPayLine2));
            }
        }

        /// <summary>
        /// Czy pierwsza linia zwycięzyła
        /// </summary>
        public bool IsPayLine3
        {
            get
            {
                return _isPayLine3;                
            }
            set
            {
                _isPayLine3 = value;
                base.OnPropertyChange(nameof(IsPayLine3));
            }
        }

        /// <summary>
        /// <inheritdoc cref="_reel1"/>
        /// </summary>
        public ObservableCollection<Symbol> Reel1
        {
            get
            {
                return _reel1;
            }
            set
            {
                _reel1 = value;
                base.OnPropertyChange(nameof(this.Reel1));
            }
        }

        /// <summary>
        /// <inheritdoc cref="_reel2"/>
        /// </summary>
        public ObservableCollection<Symbol> Reel2
        {
            get
            {
                return _reel2;
            }
            set
            {
                _reel2 = value;
                base.OnPropertyChange(nameof(this.Reel2));
            }
        }

        /// <summary>
        /// <inheritdoc cref="_reel3"/>
        /// </summary>
        public ObservableCollection<Symbol> Reel3
        {
            get
            {
                return _reel3;
            }
            set
            {
                _reel3 = value;
                base.OnPropertyChange(nameof(this.Reel3));
            }
        }

        /// <summary>
        /// Czy możemy wystartować grę?
        /// </summary>
        public bool IsStartPossible
        {
            get
            {
                return !this.IsGameRunning && !this.IsPayoutAvailable;
            }
        }

        /// <summary>
        /// Możemy zebrać wygraną jeśli gra nie chodzi i mamy jakąś wygrana
        /// </summary>
        public bool IsPayoutAvailable
        {
            get
            {
                return !this.IsGameRunning && this.CurrentWin > 0;
            }
        }

        /// <summary>
        /// <inheritdoc cref="Game.Bet"/>
        /// </summary>
        public int Bet
        {
            get
            {
                return _gameModel.Bet;
            }
        }

        /// <summary>
        /// Komenda rozpoczęcia gry
        /// </summary>
        public IAsyncCommand Spin
        {
            get;
            private set;
        }

        /// <summary>
        /// Komenda zbierająca wygraną.
        /// </summary>
        public IAsyncCommand Collect { get; private set; }

        /// <summary>
        /// C-tor ustawiający model gry
        /// </summary>
        /// <param name="gameModel"><inheritdoc cref="_gameModel"/></param>
        public GameMainWindowViewModel(Game gameModel)
        {
            Ensure.ParamNotNull(gameModel, nameof(gameModel));
            _gameModel = gameModel;

            this.Balance = _gameModel.Balance;
            this.CurrentWin = _gameModel.CurrentWin;

            this.Spin = new AsyncCommand(this.OnSpin);
            this.Collect = new AsyncCommand(this.OnCollect);

            Trace.WriteLine($"Game started with RNO: {_gameModel.Spinner.Rno}");

            // Ustawienia początkowe walców
            _reel1 = new ObservableCollection<Symbol>();
            _reel2 = new ObservableCollection<Symbol>();
            _reel3 = new ObservableCollection<Symbol>();
            foreach (PayLine winnerLine in gameModel.Spinner.PayLines)
            {
                _reel1.Add(new Symbol(winnerLine.Line[0]));
                _reel2.Add(new Symbol(winnerLine.Line[1]));
                _reel3.Add(new Symbol(winnerLine.Line[2]));
            }
        }

        /// <summary>
        /// Obsługa komendy <see cref="Spin"/>
        /// </summary>
        private async Task OnSpin()
        {
            try
            {
                this.ClearWinnerStatus();            
                if (_gameModel.IsGamePossible == false)
                {
                    MessageBox.Show($"Game over! Your balance is: {_gameModel.Balance} and the bet is {_gameModel.Bet}");
                    Trace.WriteLine($"User ends game with RNO: {_gameModel.Spinner.Rno};");
                    return;
                }

                this.IsGameRunning = true;
                _gameModel.Start();
                await this.DecorateTheSpin();
                await this.SumUpSpinResult();
            }
            catch (Exception ex)
            {
                this.HandleCriticalError(ex);
            }
            finally
            {
                this.IsGameRunning = false;
            }
        }

        /// <summary>
        /// Czyści status zwycięcy - usuwa wszystko, co mówiło, że gracz wygrał.
        /// </summary>
        private void ClearWinnerStatus()
        {
            this.IsPayLine1 = false;
            this.IsPayLine2 = false;
            this.IsPayLine3 = false;
        }

        /// <summary>
        /// Podsumowanie spina: czy wygrał i ile wygrał.
        /// </summary>
        private async Task SumUpSpinResult()
        {
            // Mówimy czy linia wygrała
            this.IsPayLine1 = this.Reel1[0].IsWinning;
            this.IsPayLine2 = this.Reel1[1].IsWinning;
            this.IsPayLine3 = this.Reel1[2].IsWinning;

            if (_gameModel.CurrentWin < 1)
            {
                // nic nie wygrano
                for (int i = 0; i <= _gameModel.Bet; i++)
                {
                    this.Balance--;
                    await Task.Delay(50);
                }

                // ostatecznie wynik bierzemy z modelu
                this.Balance = _gameModel.Balance;

                return;
            }

            double rest = _gameModel.CurrentWin - Math.Truncate(_gameModel.CurrentWin);
            if (rest > 0.00)
            {
                this.CurrentWin += rest;
                await Task.Delay(50);
            }

            for (int i = 0; i <= _gameModel.CurrentWin; i++)
            {
                // animacj wyniku
                this.CurrentWin++;
                await Task.Delay(50);
            }

            // ostateczny wynik powinien być brany z modelu gry
            this.CurrentWin = _gameModel.CurrentWin;
        }

        /// <summary>
        /// Obsługa komendy <see cref="Collect"/>
        /// </summary>
        private async Task OnCollect()
        {
            try
            {
                if (_isCollecting)
                {
                    return;
                }

                _isCollecting = true;

                for (int i = 0; i < _gameModel.CurrentWin; i++)
                {
                    // animacja
                    this.CurrentWin--;
                    this.Balance++;
                    await Task.Delay(50);
                }

                // jeśli mamy jakąś resztę..
                const double smallestDouble = 0.01;
                for (double d = 0.00; d < this.CurrentWin; d += smallestDouble)
                {
                    this.CurrentWin -= smallestDouble;
                    this.Balance += smallestDouble;

                    await Task.Delay(50);
                }

                // ostateczny stan konta
                this.Balance = _gameModel.Balance;

                this.OnPropertyChange(nameof(this.IsPayoutAvailable), nameof(this.IsStartPossible));
            }
            catch (Exception ex)
            {
                this.HandleCriticalError(ex);
            }
            finally
            {
                _isCollecting = false;
            }

        }

        /// <summary>
        /// Obsługa bardzo poważnego błędu. Wyświetla bład i zamyka aplikację
        /// </summary>
        /// <param name="ex">Exception</param>
        private void HandleCriticalError(Exception ex)
        {
            MessageBox.Show($"Critical failure. {ex.Message}");
            Trace.WriteLine(ex);
            // To jakiś krytyczny błąd - zamykamy grę, bo coś się skopało.
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Helper dokonujący dekoracji zakręcenia maszyną.
        /// </summary>
        private async Task DecorateTheSpin()
        {
            PayLine[] payLines = this.GetVisiblePayLines();

            int reel1 = 0, reel2 = 1, reel3 = 2;
            List<Task> tasks = new List<Task>
            {
                Task.Factory.StartNew( () => { this.AnimateReel(reel1, payLines, this.Reel1); }),
                Task.Factory.StartNew( () => { this.AnimateReel(reel2, payLines, this.Reel2); }),
                Task.Factory.StartNew( () => { this.AnimateReel(reel3, payLines, this.Reel3); })
            };

            await Task.WhenAll(tasks);

            base.OnPropertyChange(nameof(this.Reel1), nameof(this.Reel2), nameof(this.Reel3));
        }

        /// <summary>
        /// Przeprowadzamy animację zakręcenia bębnem - najpierw bierzemy listę unikalnych symboli dla danego walca i przewijamy je po kolei.
        /// Przy ostatnim przesunięciu walca bierzemy symbol z modelu gry (czyli ten, który został "wylosowany").
        /// </summary>
        /// <param name="reelNumber">Numer walca w modelu gry</param>
        /// <param name="payLines"></param>
        /// <param name="reelSymbols">Walec na widoku</param>        
        private void AnimateReel(int reelNumber, PayLine[] payLines, ObservableCollection<Symbol> reelSymbols)
        {

            int[] reelSymbolsUnique = _gameModel.Reels[reelNumber].Symbols.Distinct().ToArray();                       

            // enumerujemy po liczbie spinów, nie po indexie czyli od 1 do liczby spinów włącznie
            for (int spin = 1; spin <= _gameModel.Reels[reelNumber].Spin; spin++)
            {
                int[] availableSymbols = this.ChooseReelSymbols(reelSymbolsUnique, reelSymbols.Count, reelSymbols[0].Value);
                for (int lineNumber = 0; lineNumber < _visibleLinesQuantity; lineNumber++)
                {
                    if (spin < _gameModel.Reels[reelNumber].Spin)
                    {
                        reelSymbols[lineNumber] = new Symbol(availableSymbols[lineNumber]);
                    }
                    else
                    {
                        reelSymbols[lineNumber] = new Symbol(payLines[lineNumber].Line[reelNumber], payLines[lineNumber].IsWinningLine);                        
                    }                    
                }                

                Task.Delay(100).GetAwaiter().GetResult();
            }
        }

        
        /// <summary>
        /// Zbiera zdefiniowaną liczbę symboli dla danego walca 
        /// </summary>
        /// <returns></returns>
        private PayLine[] GetVisiblePayLines()
        {
            int payLinesCount = _gameModel.Spinner.PayLines.Count();
            int additionalPayLines = payLinesCount - _visibleLinesQuantity;

            // Ile linii przerobiliśmy?
            int linesComplete = 0;

            PayLine[] visiblePayLines = new PayLine[_visibleLinesQuantity];
            foreach (PayLine payLine in _gameModel.Spinner.PayLines)
            {
                if (payLine.IsWinningLine == false && additionalPayLines > 0)
                {
                    // Jeśli ta linia nie daje nam wygranej to jej nie wyświetlamy, może następna będzie miała
                    // Robimy tak tylko jeśli mamy więcej wygranych linii niż wyświetlamy.
                    --additionalPayLines;
                    continue;
                }

                visiblePayLines[linesComplete] = payLine;
                linesComplete++;
            }

            return visiblePayLines;
        }

        /// <summary>
        /// Tworzy symbole dla każdej linii wygrywającej konkretnego walca na podstawie unikalnych symboli na walcu.
        /// </summary>
        /// <param name="reelSymbols">Symbole na walcu</param>
        /// <param name="linesCount">Ilość linii widocznych na ekranie</param>
        /// <param name="startSymbol">Symbol, od którego startujemy.</param>
        /// <returns></returns>
        private int[] ChooseReelSymbols(int[] reelSymbols, int linesCount, int startSymbol)
        {
            int[] symbols = new int[linesCount];          

            int startIndex = Array.FindIndex(reelSymbols, s => s == startSymbol);

            for (int line = 0; line < linesCount; line++)
            {
                startIndex++;
                if (startIndex >= reelSymbols.Length)
                {
                    startIndex = 0;
                }

                symbols[line] = reelSymbols[startIndex];
            }

            return symbols;
        }
    }
}
