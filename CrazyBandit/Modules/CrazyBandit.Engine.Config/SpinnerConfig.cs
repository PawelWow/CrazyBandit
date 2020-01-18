using CrazyBandit.Common;
using System;

namespace CrazyBandit.Engine.Config
{
    /// <summary>
    /// Konfig dla <see cref="Spinner"/>, który ma zagwarantować poprawne jego działanie.
    /// </summary>
    public class SpinnerConfig
    {
        /// <summary>
        /// Definicje walców
        /// </summary>
        public Reel[] Reels { get; private set; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public int[][] Lines { get; private set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public int[] Spin { get; private set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public int Rno { get; private set; }

        /// <summary>
        /// Liczba linii wygrywających
        /// </summary>
        public int PayLines { get; private set; }

        /// <summary>
        /// C-tor konfiguratora dla spinnera. Przeprowdza walidację, jeśli dane będą nieodpowiednie to wysypie się z błędem.
        /// </summary>
        /// <param name="reels">Walce</param>
        /// <param name="rnoConfig">Konfig początkowego rno</param>
        /// <param name="payLines">Liczba linii wygrywających. Domyślnie brana z <see cref="Defs.PayLines"/></param>
        public SpinnerConfig(Reel[] reels, RnoConfig rnoConfig, int payLines)
        {
            Ensure.ParamNotNullOrEmpty(reels, nameof(reels));
            Ensure.ParamNotNull(rnoConfig, nameof(rnoConfig));

            this.Reels = reels;
            this.Spin = this.GetSpins(reels);   
            
            // TODO jeszcze premyśleć. Ten composer powinien być w engine, a nie w configach
            ResultsComposer results = new ResultsComposer(reels);            

            this.ValidateConfig(results.Lines, rnoConfig, payLines);

            this.Lines = results.Lines;            
            this.Rno = rnoConfig.Value;
            this.PayLines = payLines;
            
        }

        /// <summary>
        /// Przeprowadza walidację konfigu dla spinnera. Rzuci wyjątkiem, jeśli spinner nie będzie miał prawa działać na tych danych.
        /// </summary>
        /// <param name="lines">Zestaw wszystkich kombinacji (możliwych linii)</param>
        /// <param name="spin">O ile każdy walec miałby się przesówać</param>
        /// <param name="rnoConfig">Konfiguracja początkowego rno dla gry</param>
        private void ValidateConfig(int[][]lines, RnoConfig rnoConfig, int winningLines)
        {         
            // Wartość początkowa nie może być większa niż liczba linii
            if (lines.Length < rnoConfig.MaxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(rnoConfig), "Invalid initial rno value");
            }

            if (lines.Length < winningLines)
            {
                throw new ArgumentOutOfRangeException(nameof(winningLines), "Too much winning lines for this configuration.");
            }


        }

        /// <summary>
        /// Przeprowadza walidację configa
        /// </summary>
        private void ValidateConfig()
        {

        }

        /// <summary>
        /// Z walców wyciąga spiny
        /// </summary>
        /// <param name="reels"></param>
        /// <returns></returns>
        private int[] GetSpins(Reel[] reels)
        {
            int[] spin = new int[reels.Length];
            for (int i = 0; i < spin.Length; i++)
            {
                spin[i] = reels[i].Spin;
            }

            return spin;
        }
    }
}
