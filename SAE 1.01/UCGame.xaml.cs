using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
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
    /// Logique d'interaction pour UCGame.xaml
    /// </summary>
    public partial class UCGame : UserControl
    {
        public Stopwatch stopwatch = new Stopwatch();

        private static double lastFrameTime;

        double posX = 0;
        double posY = 0;

        double velX = 0;
        double velY = 0;

        double acceleration = 2000;
        double deceleration = 800;
        double maxSpeed = 200;

        int inputX = 0;
        int inputY = 0;

        public UCGame()
        {
            InitializeComponent();
            stopwatch.Start();
            SizeChanged += OnWindowSizeChanged;

            CompositionTarget.Rendering += GameLoop;
        }

        private void GameLoop(object sender, EventArgs e)
        {
            double current = stopwatch.Elapsed.TotalSeconds;
            double deltaTime = current - lastFrameTime;
            lastFrameTime = current;

            // Accélération X

            if (inputX != 0)
            {
                velX += inputX * acceleration * deltaTime;
                velX = Math.Clamp(velX, -maxSpeed, maxSpeed);
            }
            else
            {
                // Décélération
                if (velX > 0)
                {
                    velX -= deceleration * deltaTime;
                    if (velX < 0) velX = 0;
                }
                else if (velX < 0)
                {
                    velX += deceleration * deltaTime;
                    if (velX > 0) velX = 0;
                }
            }

            // Accélération Y

            if (inputY != 0)
            {
                velY += inputY * acceleration * deltaTime;
                velY = Math.Clamp(velY, -maxSpeed, maxSpeed);
            }
            else
            {
                // Décélération
                if (velY > 0)
                {
                    velY -= deceleration * deltaTime;
                    if (velY < 0) velY = 0;
                }
                else if (velY < 0)
                {
                    velY += deceleration * deltaTime;
                    if (velY > 0) velY = 0;
                }
            }

            // Normalisation (mouvement en diagonale)

            Vector velocity = new Vector(velX, velY);

            if (velocity.Length > maxSpeed)
            {
                velocity.Normalize();
                velocity *= maxSpeed;

                velX = velocity.X;
                velY = velocity.Y;
            }

            // Position

            posX += velX * deltaTime;
            posY += velY * deltaTime;

            Canvas.SetLeft(Player, posX);
            Canvas.SetTop(Player, posY);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.KeyDown += GameKeyDown;
            Application.Current.MainWindow.KeyUp += GameKeyUp;
        }

        private void GameKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                ((MainWindow)(Application.Current.MainWindow)).ShowUC("_parameters");
                stopwatch.Stop();
            }
            if (e.Key == Key.Q) inputX = -1;
            if (e.Key == Key.D) inputX = 1;
            if (e.Key == Key.Z) inputY = -1;
            if (e.Key == Key.S) inputY = 1;
        }

        private void GameKeyUp(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Q) && inputX == -1) inputX = 0;
            if ((e.Key == Key.D) && inputX == 1) inputX = 0;

            if ((e.Key == Key.Z) && inputY == -1) inputY = 0;
            if ((e.Key == Key.S) && inputY == 1) inputY = 0;
        }
        private void OnWindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            /// <summary>
            /// Change la taille du canvas proportionnellement à la taille de la fenêtre
            /// </summary>

            // Taille Canvas
            double designW = GameCanvas.Width;
            double designH = GameCanvas.Height;

            double scaleX = ActualWidth / designW;
            double scaleY = ActualHeight / designH;

            double scale = Math.Min(scaleX, scaleY); // pour garder le ratio

            GameScale.ScaleX = scale;
            GameScale.ScaleY = scale;
        }
    }
}
