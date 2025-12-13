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

namespace Over_Roboted_II
{
    /// <summary>
    /// Logique d'interaction pour UCDemarrage.xaml
    /// </summary>
    public partial class UCDemarrage : UserControl
    {
        public UCDemarrage()
        {
            InitializeComponent();
        }

        private void butDemarrer_Click(object sender, RoutedEventArgs e)
        {
            /// <summary>
            ///  L'utilisateur ferme la fenêtre des paramètres avec mise à jour
            /// </summary>

            
            MainWindow.mainWindow.ShowUC("_game");
        }

        private void butRegles_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.mainWindow.ShowUC("_regles");
        }

        private void butParametres_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.mainWindow.ShowUC("_parameters");
        }

        private void butQuitterJeu_Click(object sender, RoutedEventArgs e)
        {
            
            Application.Current.Shutdown();

        }
    }
}
