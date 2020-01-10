﻿using System;

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
    }
}
