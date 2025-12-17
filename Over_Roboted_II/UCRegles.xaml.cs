using System.Windows;
using System.Windows.Controls;

namespace Over_Roboted_II
{
    /// <summary>
    /// Logique d'interaction pour UCRegles.xaml
    /// </summary>
    public partial class UCRegles : UserControl
    {
        public UCRegles()
        {
            InitializeComponent();
        }

        private void butQuitter_Click(object sender, RoutedEventArgs e)
        {
            UCDemarrage.sonClick.Play();
            MainWindow.mainWindow.ShowUC("_demarrage");
        }
    }
}
