using System;
using System.Threading.Tasks;

namespace CrazyBandit.Console.Extensions
{
	/// <summary>
	/// Rozszerzenia dla tasków
	/// </summary>
	internal static class TasksExtensions
    {
		/// <summary>
		/// Umożliwia bezpieczny await funkcji "void". Zwykle awaitowana funkcja powinna zwracać <see cref="Task"/> - jest to
		/// odpowiednik "void" dla funkcji asynchronicznych. Nie zawsze jednak mamy taką możliwość (3rd party dostarcza interfejsy
		/// nie asynchroniczne). 
		/// Niniejszy extension pozwala na bezpieczne awaitowanie - z obsługą błędów lub bez handlera - z wygaszeniem błędów
		/// </summary>
		/// <param name="task">Task, na którym chcemy użyć funkcji.</param>
		/// <param name="handler">Obsługa błedów.</param>
#pragma warning disable RECS0165
        public static async void FireAndForgetSafeAsync(this Task task, IErrorHandler handler)
#pragma warning restore RECS0165
        {
			try
			{
				await task;
			}
			catch (Exception ex)
			{

				handler?.HandleError(ex); 
			}
        }
    }
}
