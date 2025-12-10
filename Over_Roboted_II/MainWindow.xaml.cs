using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SAE_1._01
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public UCGame _game = new UCGame();
        public UCParameters _parameters = new UCParameters();

        public MainWindow()
        {
            InitializeComponent();

            ShowUC("_game");
        }

        public void ShowUC(string uc)
        {                       
            if (uc == "_game")
            { 
                _game.stopwatch.Start();
                contentControl.Content = _game;
            }

            else if (uc == "_parameters")
            {
                contentControl.Content = _parameters;
            }
        }
    }
}