using CrazyBandit.Common;
using CrazyBandit.Engine;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
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
        /// Symbole walców
        /// </summary>
        private ObservableCollection<string> _reel1;
        private ObservableCollection<string> _reel2;
        private ObservableCollection<string> _reel3;


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

            this.Spin = new RelayCommand(execute => this.OnSpin());
            _reel1 = new ObservableCollection<string>();
            _reel2 = new ObservableCollection<string>();
            _reel3 = new ObservableCollection<string>();
            foreach (PayLine winnerLine in gameModel.Spinner.PayLines)
            {
                _reel1.Add(this.NumberToResource(winnerLine.Line[0]));
                _reel2.Add(this.NumberToResource(winnerLine.Line[1]));
                _reel3.Add(this.NumberToResource(winnerLine.Line[2]));
            }

        }

        public ObservableCollection<string> Reel1
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

        public ObservableCollection<string> Reel2
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

        public ObservableCollection<string> Reel3
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
        private void OnSpin()
        {
            if (_gameModel.IsGamePossible == false)
            {
                MessageBox.Show("Game over!");
            }

            _gameModel.Spinner.Spin();
            this.SetReels(_gameModel.Spinner.PayLines);
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
            foreach (PayLine winnerLine in payLines)
            {
                Reel1.Add(this.NumberToResource(winnerLine.Line[0]));
                Reel2.Add(this.NumberToResource(winnerLine.Line[1]));
                Reel3.Add(this.NumberToResource(winnerLine.Line[2]));
            }
        }

        /// <summary>
        /// Zamienia numer symbolu na odpowiedni resource
        /// </summary>
        /// <param name="symbolNumber"></param>
        /// <returns></returns>
        private string NumberToResource(int symbolNumber)
        {
            return $"/Resources/Images/Symbols/{symbolNumber}.png";
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
