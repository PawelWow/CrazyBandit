using CrazyBandit.Common;
using CrazyBandit.Engine;
using System;
using System.Collections.Generic;
using System.Text;

namespace CrazyBandit.Console.ViewModels
{
    internal class GameMainWindowViewModel : Observed
    {
        /// <summary>
        /// Model gry
        /// </summary>
        private readonly Game _gameModel;

        /// <summary>
        /// C-tor ustawiający model gry
        /// </summary>
        /// <param name="gameModel"><inheritdoc cref="_gameModel"/></param>
        public GameMainWindowViewModel(Game gameModel)
        {
            Ensure.ParamNotNull(gameModel, nameof(gameModel));
            _gameModel = gameModel;

            
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
