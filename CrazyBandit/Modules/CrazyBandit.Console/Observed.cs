using System.ComponentModel;

namespace CrazyBandit.Console
{
    /// <summary>
    /// Klasa umożliwiająca bindowanie view modeli z modelami
    /// </summary>
    internal abstract class Observed : INotifyPropertyChanged
    {
        /// <summary>
        /// Zdarzenie zmiany propertiesa
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Informuje, że zmienił się properties na ViewModelu
        /// </summary>
        /// <param name="propNames">Nazwy propertiesów, które uległy zmianie</param>
        protected void OnPropertyChange(params string[] propNames)
        {
            if (this.PropertyChanged == null)
            {
                return;
            }

            foreach (string propertyName in propNames)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
