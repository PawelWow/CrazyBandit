namespace CrazyBandit.Engine
{
    /// <summary>
    /// Interfejs kompozytora wyników
    /// </summary>
    interface IResultsComposer
    {
        /// <summary>
        /// Linie ułożone na bębnach
        /// </summary>
        int[][] Lines { get; }
    }
}
