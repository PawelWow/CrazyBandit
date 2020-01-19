using CrazyBandit.Common;
using CrazyBandit.Engine;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CrazyBandit.Console.ViewModels
{
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


        /// <summary>
        /// Komenda rozpoczęcia gry
        /// </summary>
        public ICommand Spin
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

            this.Spin = new RelayCommand(async execute => await this.OnSpin());
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

                await this.DecorateTheSpin();

                _gameModel.Spinner.Spin();
                this.SetReels(_gameModel.Spinner.PayLines);
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
            int reel1start = this.Reel1[0].Id;
            int reel2start = this.Reel2[0].Id;
            int reel3start = this.Reel3[0].Id;

            int reel1Symbols = this.Reel1.Count;

            var tasks = new List<Task>();

            tasks.Add(await Task.Factory.StartNew(
                async () =>
                {
                    List<int> previousSymbols = new List<int>();
                    for (int i = 0; i < reel1Symbols; i++)
                    {
                        int spin = 1;
                        if (i == 0)
                        {
                            // Tu jest coś nie tak - nie przesuwa się
                            spin = _gameModel.Reels[0].Spin + reel1start;
                        }

                        int symbol = spin;
                        if (symbol >= _gameModel.Reels[0].Symbols.Length)
                        {
                            // korekta o przekroczoną ilość
                            symbol -= _gameModel.Reels[0].Symbols.Length;
                        }

                        // TODO powinno być w oparciu o liczbę wystąpień
                        if (previousSymbols.Any(s => s == symbol))
                        {
                            symbol = _gameModel.Reels[0].Symbols.FirstOrDefault(s => previousSymbols.Any(x => x != symbol));
                        }
                        previousSymbols.Add(symbol);

                        this.Reel1[i] = new Symbol(symbol);

                        await Task.Delay(300);
                    }
                }

                ));

            //Task.WaitAll(tasks.ToArray());
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

                Reel1.Add(new Symbol(winnerLine.Line[0]));
                Reel2.Add(new Symbol(winnerLine.Line[1]));
                Reel3.Add(new Symbol(winnerLine.Line[2]));
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
