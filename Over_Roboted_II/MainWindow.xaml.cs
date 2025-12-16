using System.Windows;
using System.Windows.Media;

namespace Over_Roboted_II
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public UCGame _game = new UCGame();
        public UCParameters _parameters = new UCParameters();
        public UCDemarrage _demarrage = new UCDemarrage();
        public UCRegles _regles = new UCRegles();

        public static MainWindow mainWindow = ((MainWindow)(Application.Current.MainWindow));
        private static MediaPlayer musiqueJeu;
        public MainWindow()
        {
            InitializeComponent();
            
            ShowUC("_demarrage");
            
        }


        public void ShowUC(string uc)
        {
            if (uc == "_demarrage")
            {
                contentControl.Content = _demarrage;
            }
            else if (uc == "_game")
            {
                InitMusique();
                _game.stopwatch.Start();
                contentControl.Content = _game;
                
            }

            else if (uc == "_parameters")
            {
                contentControl.Content = _parameters;
            }
            else if (uc == "_regles")
            {
                contentControl.Content = _regles;
            }
        }
        private void InitMusique()
        {
            musiqueJeu = new MediaPlayer();
           
            musiqueJeu.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Audio/musiqueJeu.mp3"));
            
            musiqueJeu.MediaEnded += RelanceMusique;
            musiqueJeu.Volume = 0.5;
            musiqueJeu.Play();
        }
        private void RelanceMusique(object? sender, EventArgs e)
        {
            musiqueJeu.Position = TimeSpan.Zero;
            musiqueJeu.Play();
        }
    }
}