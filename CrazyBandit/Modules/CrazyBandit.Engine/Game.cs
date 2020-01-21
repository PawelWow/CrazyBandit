using CrazyBandit.Common;
using CrazyBandit.Engine.Config;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CrazyBandit.Engine
{
    /// <summary>
    /// Klasa umożliwająca właściwą grę.
    /// </summary>
    public class Game
    {
        /// <summary>
        /// <inheritdoc cref="ISpinner"/>
        /// </summary>
        public ISpinner Spinner { get; private set; }

        /// <summary>
        /// <inheritdoc cref="IWinningsCalculator"/>
        /// </summary>
        public IWinningsCalculator Winnings { get; set; }

        /// <summary>
        /// Stawka
        /// </summary>
        public int Bet { get; private set; }

        /// <summary>
        /// Bilans wygranych
        /// </summary>
        public float Balance { get; private set; }

        /// <summary>
        /// Wygrana z danej gry
        /// </summary>
        public float CurrentWin { get; private set; }

        /// <summary>
        /// Czy dalsza gra jest możliwa
        /// </summary>
        public bool IsGamePossible
        {
            get
            {
                return this.Balance >= this.Bet;
            }
        }

        /// <summary>
        /// Kolekcja walców
        /// </summary>
        public Reel[] Reels
        {
            get; private set;
        }

        /// <summary>
        /// Konstruktor gry
        /// </summary>
        /// <param name="spinner"></param>
        /// <param name="winnings"></param>
        public Game(Reel[] reels, ISpinner spinner, IWinningsCalculator winnings, PlayerFinancesConfig player)
        {
            // TODO przerób na private, zrób Create()

            Ensure.ParamNotNullOrEmpty(reels, nameof(reels));
            Ensure.ParamNotNull(spinner, nameof(spinner));
            Ensure.ParamNotNull(winnings, nameof(winnings));
            Ensure.ParamNotNull(player, nameof(player));

            this.Reels = reels;
            this.Spinner = spinner;
            this.Winnings = winnings;
            this.Bet = player.Bet;
            this.Balance = player.Balance;
            this.CurrentWin = 0;
        }

        // TODO !!!

           
        /// <summary>
        /// Kreator dostarczający pobierający niezbędne dane do utworzenia gry.
        /// </summary>
        /// <returns>Gra.</returns>
        public static Game Create()
        {
            // Obsługa błędów, tutaj? Nie udało się skonfigurować gry TODO  
                       
            GameConfigProvider gameConfig = new GameConfigProvider();
            ISpinner spinner = new Spinner(new SpinnerConfig(gameConfig.Reels, gameConfig.ConfigRno, gameConfig.PayLines));
            IWinningsCalculator winnings = new WinningsCalculator(gameConfig.Winnings);

            return new Game(gameConfig.Reels, spinner, winnings, gameConfig.ConfigPlayer);            
        }
        

        /// <summary>
        /// Wystartuj grę.
        /// </summary>
        public void Start()
        {
            if (this.IsGamePossible == false)
            {
                // W takie sytuacji nie powinniśmy dopuścić do gry na UI
                throw new InvalidOperationException("Game Over: not enough money to start the game!");
            }

            this.CurrentWin = 0;
            this.Spinner.Spin();
            this.CheckWinnings();
            this.SetBalance();
        }

        /// <summary>
        /// Sprawdź czy są wygrane
        /// </summary>
        private void CheckWinnings()
        {            
            foreach (PayLine line in this.Spinner.PayLines.Where(line => line.IsWinningLine))
            {
                this.CurrentWin += (float)Math.Round((double)this.Winnings.Calculate(this.Bet, line.Line[0]), 2);
            }
         }

        /// <summary>
        /// Ustawia stan konta gracza
        /// </summary>
        private void SetBalance()
        {
            if (this.CurrentWin > 0)
            {
                this.Balance += this.CurrentWin;
                return;
            }

            this.Balance -= this.Bet;
            if (this.Balance <= 0)
            {
                // Nie gnębimy gracza. Nie dość, że przejebał kasę to jeszcze miałby dopłacać? 
                this.Balance = 0;
            }
        }
    }
}
