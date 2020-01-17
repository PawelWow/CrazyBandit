using CrazyBandit.Engine;
using CrazyBandit.Engine.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CrazyBandit.Console.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class GameMainWindow : Window
    {
        public GameMainWindow()
        {
            InitializeComponent();

            // model gry TODO - ma trafić do viewmodelu
            Game game = Game.Create();
        }
    }
}
