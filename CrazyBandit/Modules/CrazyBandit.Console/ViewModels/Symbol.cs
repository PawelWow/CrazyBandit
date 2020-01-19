namespace CrazyBandit.Console.ViewModels
{
    /// <summary>
    /// Deskryptor konkretnego symbolu na walcu 
    /// </summary>
    internal class Symbol
    {
        /// <summary>
        /// Id symbolu
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Adres obrazka w resource'ach. Każdy symbol MUSI mieć tam swój obrazek.
        /// </summary>
        public string Image
        {
            get
            {
                return $"/Resources/Images/Symbols/{this.Id}.png";
            }
        }

        /// <summary>
        /// Dekoracja symbolu o danym ID
        /// </summary>
        /// <param name="id">Id symbolu, który ma być udekorowany.</param>
        public Symbol(int id)
        {
            this.Id = id;
        }
    }
}
