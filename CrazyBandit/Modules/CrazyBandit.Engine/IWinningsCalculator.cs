namespace CrazyBandit.Engine
{
    /// <summary>
    /// Interfejs kalkulatora wygranych
    /// </summary>
    public interface IWinningsCalculator
    {
        /// <summary>
        /// Oblicza wygraną za dany spin
        /// </summary>
        /// <param name="bet">Stawka jaką chcemy przemnożyć</param>
        /// <param name="symbol">Symbol</param>
        /// <returns>Ile wygrał za dany symbol</returns>
        double Calculate(int bet, int symbol);
    }
}
