using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
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
        public static SoundPlayer sonClick;
        public UCDemarrage()
        {
            InitializeComponent();
            InitSon();
        }
        public void InitSon()
        {
            sonClick = new SoundPlayer(Application.GetResourceStream(new Uri("pack://application:,,,/Audio/sonBoutonJeu.wav")).Stream);
        }
        private void butDemarrer_Click(object sender, RoutedEventArgs e)
        {
            /// <summary>
            ///  L'utilisateur ferme la fenêtre des paramètres avec mise à jour
            /// </summary>

            sonClick.Play();
            MainWindow.mainWindow.ShowUC("_game");
        }

        private void butRegles_Click(object sender, RoutedEventArgs e)
        {
            sonClick.Play();
            MainWindow.mainWindow.ShowUC("_regles");
        }

        private void butParametres_Click(object sender, RoutedEventArgs e)
        {
            sonClick.Play();
            MainWindow.mainWindow.ShowUC("_parameters");
        }

        private void butQuitterJeu_Click(object sender, RoutedEventArgs e)
        {
            sonClick.Play();
            Application.Current.Shutdown();

        }
    }
}
