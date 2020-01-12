using System;
using System.Collections.Generic;
using System.Text;

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
        IEnumerable<int[]> Lines { get; }
    }
}
