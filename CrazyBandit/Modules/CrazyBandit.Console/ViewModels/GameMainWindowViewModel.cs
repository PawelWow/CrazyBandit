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
        /// Symbole walców
        /// </summary>
        private ObservableCollection<Symbol> _reel1;
        private ObservableCollection<Symbol> _reel2;
        private ObservableCollection<Symbol> _reel3;

        private bool _isPayLine1;
        private bool _isPayLine2;
        private bool _isPayLine3;


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
        /// Komenda rozpoczęcia gry
        /// </summary>
        public IAsyncCommand Spin
        {
            get;
            private set;
        }

        /// <summary>
        /// C-tor ustawiający model gry
        /// </summary>
        /// <param name="gameModel"><inheritdoc cref="_gameModel"/></param>
        public GameMainWindowViewModel(Game gameModel)
        {
            Ensure.ParamNotNull(gameModel, nameof(gameModel));
            _gameModel = gameModel;          

            this.Spin = new AsyncCommand(this.OnSpin);
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
        /// Obsługa komendy <see cref="Spin"/>
        /// </summary>
        private async Task OnSpin()
        {
            try
            {
                if (_gameModel.IsGamePossible == false)
                {
                    MessageBox.Show("Game over!");
                }

                this.IsPayLine1 = false;
                this.IsPayLine2 = false;
                this.IsPayLine3 = false;

                // TODO zerowaie wyników potrzebne
                //base.OnPropertyChange(nameof(this.CurrentWin), nameof(this.Balance));

                await this.DecorateTheSpin();

                _gameModel.Start();
                this.SetReels(_gameModel.Spinner.PayLines);

                this.IsPayLine1 = this.Reel1[0].IsWinning;
                this.IsPayLine2 = this.Reel1[1].IsWinning;
                this.IsPayLine3 = this.Reel1[2].IsWinning;

                base.OnPropertyChange(nameof(this.CurrentWin), nameof(this.Balance));

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Spin failed. {ex.Message}");
                Trace.WriteLine(ex);
                // To jakiś krytyczny błąd - zamykamy grę, bo coś się skopało.
                Application.Current.Shutdown();
            }
        }

        /// <summary>
        /// Helper dokonujący dekoracji zakręcenia maszyną.
        /// </summary>
        private async Task DecorateTheSpin()
        {

            List<Task> tasks = new List<Task>
            {
                Task.Factory.StartNew( () => { this.AnimateReel(0, this.Reel1); }),
                Task.Factory.StartNew( () => { this.AnimateReel(1, this.Reel2); }),
                Task.Factory.StartNew( () => { this.AnimateReel(2, this.Reel3); })
            };

            await Task.WhenAll(tasks);
            
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reelNumber">Numer walca w modelu gry</param>
        /// <param name="reelSymbols">Walec na widoku</param>        
        private void AnimateReel(int reelNumber, ObservableCollection<Symbol> reelSymbols)
        {

            int[] reelSymbolsUnique = _gameModel.Reels[reelNumber].Symbols.Distinct().ToArray();   
            
            // enumerujemy po liczbie spinów, nie po indexie czyli od 1 do liczby spinów włącznie
            for (int spin = 1; spin <= _gameModel.Reels[reelNumber].Spin; spin++)
            {
                int startIndex = Array.FindIndex(reelSymbolsUnique, s => s == reelSymbols[0].Value);
                for (int line = 0; line < reelSymbols.Count; line++)
                {
                    startIndex++;
                    if (startIndex >= reelSymbolsUnique.Length)
                    {
                        startIndex = 0;
                    }

                    int symbol = reelSymbolsUnique[startIndex];
                    reelSymbols[line] = new Symbol(symbol);
                }

                Task.Delay(100).GetAwaiter().GetResult();
            }
        }

        /// <summary>
        /// Ustawia walce
        /// </summary>
        /// <param name="payLines"></param>
        private void SetReels(IEnumerable<PayLine> payLines)
        {
            this.Reel1.Clear();
            this.Reel2.Clear();
            this.Reel3.Clear();

            int additionalPayLines = payLines.Count() - _visibleLinesQuantity;

            foreach (PayLine winnerLine in payLines)
            {
                if (winnerLine.IsWinningLine == false && additionalPayLines > 0)
                {
                    // Jeśli ta linia nie daje nam wygranej to jej nie wyświetlamy, może następna będzie miała
                    // Robimy tak tylko jeśli mamy więcej wygranych linii niż wyświetlamy.
                    --additionalPayLines;
                    continue;
                }

                Reel1.Add(new Symbol(winnerLine.Line[0], winnerLine.IsWinningLine));
                Reel2.Add(new Symbol(winnerLine.Line[1], winnerLine.IsWinningLine));
                Reel3.Add(new Symbol(winnerLine.Line[2], winnerLine.IsWinningLine));
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

        public float Balance
        {
            get
            {
                return _gameModel.Balance;
            }
        }

        public float CurrentWin
        {
            get
            {
                return _gameModel.CurrentWin;
            }

        }


    }
}
