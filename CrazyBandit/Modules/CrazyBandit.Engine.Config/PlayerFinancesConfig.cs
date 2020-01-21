namespace CrazyBandit.Engine.Config
{
    /// <summary>
    /// Ustawienia początkowe gracza.
    /// </summary>
    public class PlayerFinancesConfig
    {
        /// <summary>
        /// Stawka
        /// </summary>
        public int Bet { get; private set; }

        /// <summary>
        /// Bilans początkowy
        /// </summary>
        public double Balance { get; private set; }

        /// <summary>
        /// C-tor konfiguracji użytkownika. Zawiera domyślne wartości
        /// </summary>
        /// <param name="bet">Stawka</param>
        /// <param name="balance">Początkowy bilans</param>
        public PlayerFinancesConfig(int bet = 5, double balance = 500.00)
        {
            this.Bet = bet;
            this.Balance = balance;
        }
    }
}
