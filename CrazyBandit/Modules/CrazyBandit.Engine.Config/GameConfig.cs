using System;
using System.Collections.Generic;
using System.Text;

namespace CrazyBandit.Engine.Config
{
    /// <summary>
    /// Klasa do bindowania danych z pliku konfiguracyjnego
    /// </summary>
    internal class GameConfig
    {
        /// <summary>
        /// Walce wraz symbolami [id_walca][symbole_walca]
        /// </summary>
        public int[][] Reels { get; set; }

        /// <summary>
        /// O ile każdy kolejny walec obraca się za pojedynczym zakręceniem
        /// </summary>
        public int[] Spin { get; set; }

        /// <summary>
        /// Wygrana za ułożone symbole na danym walcu
        /// </summary>
        public int[] Winnings { get; set; }        
    }
}
