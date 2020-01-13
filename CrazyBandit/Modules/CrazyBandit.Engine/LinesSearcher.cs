using CrazyBandit.Common;
using System;
using System.Linq;

namespace CrazyBandit.Engine
{
    /// <summary>
    /// Klasa do odnajdywania linii w kolekcji wszystkich możliwych kombinacji.
    /// </summary>
    public class LinesSearcher
    {
        /// <summary>
        /// Wszystkie możliwe linii
        /// </summary>
        public int[][] Lines { get; private set; }

        /// <summary>
        /// Konstruktor ustawiający linie, które chcemy szukać
        /// </summary>
        /// <param name="lines"></param>
        public LinesSearcher(int[][] lines)
        {
            Ensure.TwoDimensionalArrayParamNotNullOrEmpty(lines, nameof(lines));

            this.Lines = lines;
        }

        /// <summary>
        /// Próbuje znaleźć daną linię  w całej kolekcji. Rzuci wyjątek jak nic nie znajdzie, jeśli ktoś poda linie, której nie ma w kolekcji.
        /// </summary>
        /// <param name="startIndex">Od jakiego miejsca ma szukać?</param>
        /// <param name="line">szukana linia</param>
        /// <returns></returns>
        public PayLine Find(int[] line, int startIndex = 0)
        {
            PayLine result = this.Search(startIndex, line);

            static bool IsFound(PayLine result)
            {
                return result is PayLineEmpty == false;
            }

            if (IsFound(result))
            {
                return result;
            }

            // nie znaleziono - może dojechaliśmy do końca walca - to lecimy od początku
            result = this.Search(0, line);
            if (IsFound(result))
            {
                return result;
            }


            // Nie powinniśmy do tego dopuszczać - powinniśmy operować tylko na liniach, które zostały ustalone na początku programu
            throw new InvalidOperationException($"There is no line in collection. The sequence: {string.Join(' ', line)}");
        }

        /// <summary>
        /// Helper dokonujący właściwego wyszukiwania
        /// </summary>
        /// <param name="startIndex">Od którego miejsca startujemy</param>
        /// <param name="line">Szukana linia</param>
        /// <returns>Albo zwrócimy linię, albo pusto.</returns>
        private PayLine Search(int startIndex, int[] line)
        {          
            for (int linesIndex = startIndex; linesIndex < this.Lines.Length; linesIndex++)
            {
                if (this.Lines[linesIndex].SequenceEqual(line))
                {
                    return new PayLine(linesIndex, line);
                }
            }

            return new PayLineEmpty();
        }
    }
}
