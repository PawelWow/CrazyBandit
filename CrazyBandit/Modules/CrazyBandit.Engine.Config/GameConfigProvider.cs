using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;

namespace CrazyBandit.Engine.Config
{
    /// <summary>
    /// Dostawca configów
    /// </summary>
    public class GameConfigProvider
    {
        /// <summary>
        /// Nazwa pliku konfiga
        /// </summary>
        private readonly string _fileName = Properties.Resources.APP_SETTINGS_FILE;

        /// <summary>
        /// Walce wczytane z configu
        /// </summary>
        public Reel[] Reels { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<int, double> Winnings { get; private set; }

        /// <summary>
        /// Ilość linii wygrywających
        /// </summary>
        public int PayLines { get; private set; }

        /// <summary>
        /// Defaultowe ustawienia RNO
        /// </summary>
        public RnoConfig ConfigRno { get; private set; }

        /// <summary>
        /// Ustawienia finansowe gracza <see href="PlayerFinancesConfig"/>
        /// </summary>
        public PlayerFinancesConfig ConfigPlayer { get; private set; }

        /// <summary>
        /// Dostarcza konfigurację poprzez wczytanie pliku configu.
        /// </summary>
        public GameConfigProvider()
        {
            // TODO przemyśleć obsługę błędów

            this.PayLines = Defs.PayLines;
            this.ConfigRno = new RnoConfig();
            this.ConfigPlayer = new PlayerFinancesConfig();

            IConfiguration config = this.GetConfiguration();
            GameConfig settings = config.Get<GameConfig>();

            this.SetReels(settings);
            this.SetWinnings(settings.Winnings);          
        }
        
        /// <summary>
        /// Pobiera config z pliku
        /// </summary>
        /// <param name="workingDirectory"></param>
        /// <returns></returns>
        private IConfiguration GetConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(_fileName, optional: true, reloadOnChange: true)
                .Build();
        }

        /// <summary>
        /// Ustawia property walców w oparciu o dane z settingsów
        /// </summary>
        /// <param name="settings"></param>
        private void SetReels(GameConfig settings)
        {
            this.Reels = new Reel[settings.Reels.Length];
            for (int reel = 0; reel < settings.Reels.Length; reel++)
            {
                this.Reels[reel] = new Reel(settings.Reels[reel], settings.Spin[reel]);
            }
        }

        /// <summary>
        /// Ustawia property wygranych dla poszczególnych symboli w oparciu o dane z settingsów
        /// </summary>
        /// <param name="winnings">Wartości wygranych dla poszczególnych walców</param>
        private void SetWinnings(double[] winnings)
        {
            this.Winnings = new Dictionary<int, double>();
            for (int w = 0; w < winnings.Length; w++)
            {
                this.Winnings.Add(w, winnings[w]);
            }
        }
    }
}
