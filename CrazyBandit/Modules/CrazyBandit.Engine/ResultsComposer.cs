using CrazyBandit.Common;
using System;
using System.Collections.Generic;

namespace CrazyBandit.Engine
{
    /// <summary>
    /// Kompozytor wyników - wszystkich możliwych linii dla zapodanych bębnów
    /// </summary>
    internal class ResultsComposer : IResultsComposer
    {       
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public IEnumerable<int[]> Lines { get; private set; }

        /// <summary>
        /// C-tor ustawiający wszystkie możliwe linie
        /// </summary>
        /// <param name="reels"></param>
        public ResultsComposer(Reel[] reels)
        {
            Ensure.ParamNotNullOrEmpty(reels, nameof(reels));
            foreach (Reel reel in reels)
            {
                if (reel.Symbols == null || reel.Symbols.Length < 1)
                {
                    throw new ArgumentException("Something is wrong with defined reel(s)");
                }
            }            

            this.Lines = this.ComposeLines(reels);
        }

        /// <summary>
        /// Chodzi po bębnach i komponuje wszystkie kombinacje, jakie mogą ustawić się w linii
        /// </summary>
        /// <param name="reels"></param>
        /// <returns></returns>
        /// <remarks>Źródło: https://www.geeksforgeeks.org/combinations-from-n-arrays-picking-one-element-from-each-array/ </remarks>
        private IEnumerable<int[]> ComposeLines(Reel[] reels)
        {
            List<int[]> lines = new List<int[]>();

            // Kombinacje robimy dla tylu elementów, ile jest walców
            int reelsNumber = reels.Length;      
            
            // Wskaźniki dla kolejnych symboli na każdym z walców (
            int[] pointers = new int[reelsNumber];


            for (int i = 0; i < reelsNumber; i++)
            {
                pointers[i] = 0;
            }

            while(true)
            {

                // Tworzymy nową linie i dodajemy do wyników
                List<int> currentLine = new List<int>();
                for (int i = 0; i < reelsNumber; i++)
                {
                    currentLine.Add(reels[i].Symbols[pointers[i]]);
                }
                
                lines.Add(currentLine.ToArray());

                int next = reelsNumber - 1;
                while (next >= 0 && (pointers[next] + 1 >= reels[next].Symbols.Length))
                {
                    // szukamy ostatniego zestawu symboli walca, który ma więcej elementów niż aktualny
                    next--;
                }

                if (next < 0)
                {
                    // wyjdź z while(true), bo nie ma więcej walców
                    return lines;
                }

                pointers[next]++;

                for (int i = next + 1; i < reelsNumber; i++)
                {
                    // TODO DOk
                    pointers[i] = 0;
                }

            }
        }

    }


}

