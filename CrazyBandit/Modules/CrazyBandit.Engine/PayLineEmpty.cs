namespace CrazyBandit.Engine
{
    /// <summary>
    /// Pusty obiekt linni zwycięskiej dla zastosowania "empty pattern", gdy nie chcemy nulla  wyszukiwania.
    /// </summary>
    public class PayLineEmpty : PayLine
    {
        /// <summary>
        /// C-tor ustawia puste wartości
        /// </summary>
        public PayLineEmpty() : base(0, new int[0])
        {
            // co do zasady to nie jest linia wygrywająca
            this.IsWinningLine = false;
        }
    }
}
