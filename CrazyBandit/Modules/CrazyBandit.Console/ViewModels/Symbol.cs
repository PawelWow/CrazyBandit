namespace CrazyBandit.Console.ViewModels
{
    /// <summary>
    /// Deskryptor konkretnego symbolu na walcu 
    /// </summary>
    internal class Symbol
    {
        /// <summary>
        /// Co to za symbol?
        /// </summary>
        public int Value { get; private set; }

        /// <summary>
        /// Czy to symbol na linii wygrywającej?
        /// </summary>
        public bool IsWinning { get; private set; }

        /// <summary>
        /// Adres obrazka w resource'ach. Każdy symbol MUSI mieć tam swój obrazek.
        /// </summary>
        public string Image
        {
            get
            {
                return $"/Resources/Images/Symbols/{this.Value}.png";
            }
        }

        /// <summary>
        /// Dekoracja symbolu o danym ID
        /// </summary>
        /// <param name="id">Id symbolu, który ma być udekorowany.</param>
        /// <param name="isWinning">Czy symbol leży na linii wygrywającej?</param>
        public Symbol(int id, bool isWinning = false)
        {
            this.Value = id;
            this.IsWinning = isWinning;
        }
    }
}
