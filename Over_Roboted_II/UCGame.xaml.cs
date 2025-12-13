using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Over_Roboted_II
{
    /// <summary>
    /// Logique d'interaction pour UCGame.xaml
    /// </summary>
    public partial class UCGame : UserControl
    {
        List<CraftingTable> CraftingTables = new List<CraftingTable>();

        public Stopwatch stopwatch = new Stopwatch();

        private static double lastFrameTime;

        public static Rect playerHitbox;

        double posX = 0;
        double posY = 0;

        double velX = 0;
        double velY = 0;

        double acceleration = 2000;
        double deceleration = 800;
        double maxSpeed = 200;

        int inputX = 0;
        int inputY = 0;

        public enum Dir { Down = 0, Up = 1, Right = 2, Left = 3 }
        public readonly BitmapImage[][] _frames = new BitmapImage[4][];
        public Dir _currentDir = Dir.Down;
        public int _frameIndex = 0;
        public double _animTimer = 0.0;
        public readonly double _frameDuration = 0.10;
        public readonly int _framesPerDirection = 5;

        // Timer dédié à l'animation (ne modifie pas GameLoop)
        private readonly DispatcherTimer _frameTimer;

        public UCGame()
        {
            InitializeComponent();
            InitializeCraftingTable();

            SizeChanged += OnWindowSizeChanged;

            LoadPlayerFrames();

            // initialisation du timer d'animation
            _frameTimer = new DispatcherTimer();
            _frameTimer.Interval = TimeSpan.FromSeconds(_frameDuration);
            _frameTimer.Tick += FrameTimer_Tick;

            stopwatch.Start();
            CompositionTarget.Rendering += GameLoop;
        }

        private void InitializeCraftingTable()
        {
            CraftingTables.Add(new CraftingTable(200, 500));
            CraftingTables.Add(new CraftingTable(360, 0));
            CraftingTables.Add(new CraftingTable(520, 0));

            foreach (var c in CraftingTables)
            {
                c.Draw(GameCanvas);
            }

        }
        private void LoadPlayerFrames()
        {
            _frames[(int)Dir.Down] = LoadFramesFor("devant");
            _frames[(int)Dir.Up] = LoadFramesFor("derriere");
            _frames[(int)Dir.Right] = LoadFramesFor("droite");
            _frames[(int)Dir.Left] = LoadFramesFor("gauche");

            for (int i = 0; i < _frames.Length; i++)
            {
                if (_frames[i] == null || _frames[i].Length == 0)
                    Debug.WriteLine($"[UCGame] Aucune frame chargée pour {((Dir)i).ToString()}. Vérifiez Images/Miku et Build Action.");
            }

            // Image initiale de secours
            var start = _frames[(int)_currentDir];
            if (start != null && start.Length > 0)
                Player.Source = start[0];
        }
        private BitmapImage[] LoadFramesFor(string direction)
        {
            var list = new List<BitmapImage>();
            string asm = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            for (int i = 0; i < _framesPerDirection; i++)
            {
                string relative = $"Images/Miku/{direction}{i + 1}.png";
                string packUri1 = $"pack://application:,,,/{asm};component/{relative}";
                string packUri2 = $"pack://application:,,,/{relative}";
                bool loaded = false;

                foreach (var packUri in new[] { packUri1, packUri2 })
                {
                    try
                    {
                        var uri = new Uri(packUri, UriKind.Absolute);
                        var bmp = new BitmapImage();
                        bmp.BeginInit();
                        bmp.UriSource = uri;
                        bmp.CacheOption = BitmapCacheOption.OnLoad;
                        bmp.EndInit();
                        bmp.Freeze();
                        list.Add(bmp);
                        Debug.WriteLine($"[UCGame] Chargée : {packUri}");
                        loaded = true;
                        break;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"[UCGame] Erreur chargement {packUri} : {ex.Message}");
                    }
                }

                if (!loaded)
                {
                    Debug.WriteLine($"[UCGame] Échec complet pour {relative}");
                }
            }
            return list.ToArray();
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

            // Collision X

            double newX = posX + velX * deltaTime;
            Rect futureX = new Rect(newX, posY, Player.Width, Player.Height);
            bool collideX = false;

            foreach (var c in CraftingTables)
            {
                if (futureX.IntersectsWith(c.Hitbox))
                {
                    collideX = true;
                    break;
                }
            }

            if (!collideX) posX = newX;
            else velX = 0;

            // Collision Y

            double newY = posY + velY * deltaTime;
            Rect futureY = new Rect(posX, newY, Player.Width, Player.Height);
            bool collideY = false;

            foreach (var c in CraftingTables)
            {
                if (futureY.IntersectsWith(c.Hitbox))
                {
                    collideY = true;
                    break;
                }
            }

            if (!collideY) posY = newY;
            else velY = 0;

            // Position

            Canvas.SetLeft(Player, posX);
            Canvas.SetTop(Player, posY);

            playerHitbox = new Rect(posX, posY, Player.Width, Player.Height);
            
            foreach (var c in CraftingTables)
            {
                //c.Interact(new Rect(posX + Player.Width / 2, posY, 0, Player.Height));
                c.Interact(playerHitbox);
            }
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
                MainWindow.mainWindow.ShowUC("_parameters");
                stopwatch.Stop();
            }
            if (e.Key == Key.Q) { inputX = -1; SetDirection(Dir.Left); StartFrameTimer(); }
            if (e.Key == Key.D) { inputX = 1; SetDirection(Dir.Right); StartFrameTimer(); }
            if (e.Key == Key.Z) { inputY = -1; SetDirection(Dir.Up); StartFrameTimer(); }
            if (e.Key == Key.S) { inputY = 1; SetDirection(Dir.Down); StartFrameTimer(); }
            if (e.Key == Key.Space) MainWindow.mainWindow.Close();
        }

        private void GameKeyUp(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Q) && inputX == -1) inputX = 0;
            if ((e.Key == Key.D) && inputX == 1) inputX = 0;

            if ((e.Key == Key.Z) && inputY == -1) inputY = 0;
            if ((e.Key == Key.S) && inputY == 1) inputY = 0;

            // si plus aucune touche de déplacement pressée, stopper l'animation
            if (inputX == 0 && inputY == 0)
            {
                StopFrameTimer();
            }
            else
            {
                // si on a encore un axe maintenu, on s'assure que la direction correspond
                if (inputX > 0) SetDirection(Dir.Right);
                else if (inputX < 0) SetDirection(Dir.Left);
                else if (inputY > 0) SetDirection(Dir.Down);
                else if (inputY < 0) SetDirection(Dir.Up);
            }
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

        // Démarre le DispatcherTimer d'animation
        private void StartFrameTimer()
        {
            if (!_frameTimer.IsEnabled)
            {
                _frameIndex = 0;
                var frames = _frames[(int)_currentDir];
                if (frames != null && frames.Length > 0)
                    Player.Source = frames[_frameIndex];
                _frameTimer.Start();
            }
        }

        // Stoppe et remet la frame au repos (index 0)
        private void StopFrameTimer()
        {
            if (_frameTimer.IsEnabled)
                _frameTimer.Stop();

            _frameIndex = 0;
            var frames = _frames[(int)_currentDir];
            if (frames != null && frames.Length > 0)
                Player.Source = frames[0];
        }

        // Tick du timer : avance la frame
        private void FrameTimer_Tick(object? sender, EventArgs e)
        {
            var frames = _frames[(int)_currentDir];
            if (frames == null || frames.Length == 0) return;

            _frameIndex = (_frameIndex + 1) % _framesPerDirection;
            Player.Source = frames[_frameIndex];
        }

        // Change la direction et réinitialise l'animation (ne démarre pas/stoppe le timer)
        private void SetDirection(Dir d)
        {
            if (_currentDir != d)
            {
                _currentDir = d;
                _frameIndex = 0;
                var frames = _frames[(int)_currentDir];
                if (frames != null && frames.Length > 0)
                    Player.Source = frames[0];
            }
        }
    }
}