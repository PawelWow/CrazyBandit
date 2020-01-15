using System;
using System.Collections.Generic;
using System.Text;

namespace CrazyBandit.Engine.Config
{
    /// <summary>
    /// Konfig dla wartości RNO. Możemy go zdefiniować lub ustawić wartość defaultową.
    /// </summary>
    public class RnoConfig
    {   
        /// <summary>
        /// Wartość maksymalna jaka mogła zostać przypisana w configu - będzie się różnić od <see cref="Value"/> jeśli przyjęliśmy 
        /// wartość losową i jednocześnie nie wylosowaliśmy maxa. 
        /// Potrzebne jest to do dokonania walidacji danych configu programu (rno nie może być większe od liczby możliwych kombinacji).
        /// </summary>
        public int MaxValue { get; private set; }

        /// <summary>
        /// Wartość RNO z konfiga
        /// </summary>
        public int Value { get; private set; }

        /// <summary>
        /// Konstruktor tworzy wartość RNO w oparciu o ustalony przedział. Jeśli wartości nie podano to użyje wartości defaultowych.
        /// </summary>
        /// <param name="min"><inheritdoc cref="Defs.MinRnoDefaultValue"/></param>
        /// <param name="max"><inheritdoc cref="Defs.MaxRnoDefaultValue"/></param>
        public RnoConfig(int min = Defs.MinRnoDefaultValue, int max = Defs.MaxRnoDefaultValue)
        {
            if (min > max)
            {
                throw new ArgumentOutOfRangeException("Minimum is bigger than maximum.");
            }

            Random random = new Random();
            this.MaxValue = max;
            this.Value = random.Next(min, max);
        }

        /// <summary>
        /// Ustawia określoną wartość rno
        /// </summary>
        /// <param name="rno">Wartość do ustawienia</param>
        public RnoConfig(int rno)
        {
            this.MaxValue = this.Value = rno;
        }
    }
}
