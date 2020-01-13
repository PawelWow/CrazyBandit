using System;
using System.Collections.Generic;
using System.Linq;

namespace CrazyBandit.Common
{
    /// <summary>
    /// Metody weryfikujące argumenty
    /// </summary>
    public static class Ensure
    {
        /// <summary>
        /// Rzuci wyjątek, jeśli parametr od danej nazwie jest nullem
        /// </summary>
        /// <param name="param">Parametr, dla którego przeprowadzana jest weryfikacja.</param>
        /// <param name="paramName">Nazwa sprawdzanego parametru</param>
        public static void ParamNotNull(object param, string paramName)
        {
            if (param == null)
            {
                throw new ArgumentNullException(paramName);
            }
        }

        /// <summary>
        /// Rzuci wyjątek, jeśli tablica danego typu jest nullem lub ma nullowe elemen, albo po prostu jest pusty
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="paramArray"></param>
        /// <param name="paramName"></param>
        public static void TwoDimensionalArrayParamNotNullOrEmpty<T>(T[][] paramArray, string paramName)
        {
            ParamNotNullOrEmpty(paramArray, paramName);
            if (paramArray.Any() == false)
            {
                throw new ArgumentException("Array is empty", paramName);
            }

            foreach (var item in paramArray)
            {
                if (item == null || item.Length < 1)
                {
                    throw new ArgumentException("Second dimension array is invalid.", paramName);
                }
            }
        }

        /// <summary>
        /// Rzuci wyjątek jeśli parametr będący kolekcją jest nullem, ma nullowe obiekty lub jest pustą kolekcją
        /// </summary>
        /// <param name="param">Parametr, dla którego przeprowadzana jest weryfikacja.</param>
        /// <param name="paramName">Nazwa sprawdzanego parametru</param> 
        public static void ParamNotNullOrEmpty(IEnumerable<object> param, string paramName)
        {
            ParamNotNull(param, paramName);
            if (param.Any() == false || param.Any( item => item == null))
            {
                throw new ArgumentException("Invalid array provided", paramName);
            }            
        }
    }
}
