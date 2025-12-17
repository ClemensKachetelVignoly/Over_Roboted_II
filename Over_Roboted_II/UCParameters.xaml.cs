using System.Windows;
using System.Windows.Controls;

namespace Over_Roboted_II
{
    /// <summary>
    /// Logique d'interaction pour UCParameters.xaml
    /// </summary>
    public partial class UCParameters : UserControl
    {
        private Dictionary<string, string> parametersDict = new Dictionary<string, string>()
        {
            { "fullscreen", "false" },
            { "volume", "0.5" }
        };
        private Dictionary<string, string> tempDict = new Dictionary<string, string>();

        public UCParameters()
        {
            InitializeComponent();
            UpdateValues(parametersDict, tempDict);
        }

        private void butExit_Click(object sender, RoutedEventArgs e)
        {
            /// <summary>
            /// L'utilisateur ferme la fenêtre des paramètres sans mise à jour
            /// Les paramètres retournent à leur état d'origine
            /// </summary>


            UCDemarrage.sonClick.Play();
            UpdateValues(parametersDict, tempDict);
            UpdateSettings(parametersDict, 0);
            MainWindow.mainWindow.ShowUC("_game");
        }

        private void butApply_Click(object sender, RoutedEventArgs e)
        {
            /// <summary>
            ///  L'utilisateur ferme la fenêtre des paramètres avec mise à jour
            /// </summary>
            UCDemarrage.sonClick.Play();
            tempDict["volume"] = sliderSon.Value.ToString();
            UpdateValues(tempDict, parametersDict);
            UpdateSettings(parametersDict, 1);
            MainWindow.mainWindow.ShowUC("_game");
        }
        

        

        private void CheckFullscreen_Click(object sender, RoutedEventArgs e)
        {
            UCDemarrage.sonClick.Play();
            tempDict["fullscreen"] = (checkFullscreen.IsChecked == true).ToString();
        }

        private void UpdateSettings(Dictionary<string, string> parameters, int value)
        {
            /// <summary>
            /// Met à jour visuellement les paramètres, et les applique si value == 1
            /// </summary>

            checkFullscreen.IsChecked = Convert.ToBoolean(parameters["fullscreen"]);
            sliderSon.Value = Convert.ToDouble(parameters["volume"]);

            if (value == 1)
            {
                if (checkFullscreen.IsChecked == true)
                {
                    Application.Current.MainWindow.WindowState = WindowState.Maximized;
                }
                else
                {
                    Application.Current.MainWindow.WindowState = WindowState.Normal;
                }
                if (MainWindow.musiqueJeu != null)
                {
                    MainWindow.musiqueJeu.Volume = sliderSon.Value;
                }
            }
        }

        private void UpdateValues(Dictionary<string, string> dic1, Dictionary<string, string> dic2)
        {
            /// <summary>
            /// Permet de mettre toutes les valeurs du dic1 dans le dic2
            /// </summary>
            foreach (var item in dic1)
            {
                dic2[item.Key] = item.Value;
            }
        }

        private void butRetour_Click(object sender, RoutedEventArgs e)
        {
            
            UCDemarrage.sonClick.Play();
            MainWindow.mainWindow.ShowUC("_demarrage");
        }

        private void sliderSon_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            
            if (MainWindow.musiqueJeu != null)
            {
                MainWindow.musiqueJeu.Volume = sliderSon.Value;
            }
        
    }
    }
}
