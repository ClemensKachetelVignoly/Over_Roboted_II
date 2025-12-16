using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Over_Roboted_II
{
    /// <summary>
    /// Logique d'interaction pour UCGame.xaml
    /// </summary>
    public partial class UCGame : UserControl
    {
        List<CraftingTable> CraftingTablesList = new List<CraftingTable>();
        List<ResourceGenerator> ResourceGeneratorsList = new List<ResourceGenerator>();
        List<Resource> ResourcesList = new List<Resource>();

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

        BitmapImage[] imageDevant = new BitmapImage[5];
        BitmapImage[] imageDerriere = new BitmapImage[5];
        BitmapImage[] imageGauche = new BitmapImage[5];
        BitmapImage[] imageDroite = new BitmapImage[5];

        
        public UCGame()
        {
            InitializeComponent();
            InitializeCraftingTable();
            InitializeResourceGeneratorsList();

            SizeChanged += OnWindowSizeChanged;
            InitializeImagesMiku();
            
            

            stopwatch.Start();
            CompositionTarget.Rendering += GameLoop;
        }
       
        private void UpdateResourceVisual()
        {
            ResourcesPanel.Children.Clear();
            foreach (var r in ResourcesList)
            {
                ResourcesPanel.Children.Add(r.Draw());
            }
        }

        private void InitializeCraftingTable()
        {
            for (int i = 0; i < 4; i++)
            {
                CraftingTablesList.Add(new CraftingTable(200 + i * 180, 0, $"pack://application:,,,/Images/craftingtable{i + 1}.jpg"));
            }


            foreach (var c in CraftingTablesList)
            {
                c.Draw(GameCanvas);
            }

        }
        private void InitializeResourceGeneratorsList()
        {
            ResourcesList.Add(new Resource("copper", 0));
            ResourcesList.Add(new Resource("diamond", 0));
            ResourcesList.Add(new Resource("gold", 0));

            ResourceGeneratorsList.Add(new ResourceGenerator(100, 50, ResourcesList[0]));
            ResourceGeneratorsList.Add(new ResourceGenerator(800, 200, ResourcesList[1]));
            ResourceGeneratorsList.Add(new ResourceGenerator(1000, 300, ResourcesList[2]));

            foreach (var r in ResourceGeneratorsList)
            {
                r.Draw(GameCanvas);
            }
        }


        private void InitializeImagesMiku()
        {
            for (int i = 0; i < imageDevant.Length; i++)
            {
                imageDevant[i] = new BitmapImage(new Uri($"pack://application:,,,/Images/Miku/devant{i + 1}.png"));
                imageDerriere[i] = new BitmapImage(new Uri($"pack://application:,,,/Images/Miku/derriere{i + 1}.png"));
                imageDroite[i] = new BitmapImage(new Uri($"pack://application:,,,/Images/Miku/droite{i + 1}.png"));
                imageGauche[i] = new BitmapImage(new Uri($"pack://application:,,,/Images/Miku/gauche{i + 1}.png"));

            }
        }
        int nombre = 0;
        public void AnimeMiku(BitmapImage[] tabImg)
        {

            nombre++;
            if (nombre / 5 == tabImg.Length)
                nombre = 1;
            if (nombre % 5 == 0)
                Player.Source = tabImg[nombre / 5];
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
                if (inputX > 0)
                    AnimeMiku(imageDroite);
                else
                    AnimeMiku(imageGauche);
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
                if (inputX > 0)
                    AnimeMiku(imageDroite);
                else if (inputX < 0)
                    AnimeMiku(imageGauche);
                else if (inputY > 0)
                    AnimeMiku(imageDevant);
                else
                    AnimeMiku(imageDerriere);
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

            foreach (var c in CraftingTablesList)
            {
                if (futureX.IntersectsWith(c.Hitbox))
                {
                    collideX = true;
                    break;
                }
            }
            foreach (var r in ResourceGeneratorsList)
            {
                if (futureX.IntersectsWith(r.Hitbox))
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

            foreach (var c in CraftingTablesList)
            {
                if (futureY.IntersectsWith(c.Hitbox))
                {
                    collideY = true;
                    break;
                }
            }
            foreach (var r in ResourceGeneratorsList)
            {
                if (futureY.IntersectsWith(r.Hitbox))
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

            foreach (var c in CraftingTablesList)
            {
                c.Interact(playerHitbox);
            }
            foreach (var r in ResourceGeneratorsList)
            {
                r.Interact(playerHitbox);
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
            if (e.Key == Key.Q)
            {
                inputX = -1;

            }
            if (e.Key == Key.D)
            {
                inputX = 1;

            }
            if (e.Key == Key.Z)
            {
                inputY = -1;

            }
            if (e.Key == Key.S)
            {
                inputY = 1;


            }
            if (e.Key == Key.Space)
                MainWindow.mainWindow.Close();
            if (e.Key == Key.E)
                Interact();
        }

        private void GameKeyUp(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Q) && inputX == -1) inputX = 0;
            if ((e.Key == Key.D) && inputX == 1) inputX = 0;

            if ((e.Key == Key.Z) && inputY == -1) inputY = 0;
            if ((e.Key == Key.S) && inputY == 1) inputY = 0;


        }

        private void Interact()
        {
            bool interactCrafting = false;

            foreach (var c in CraftingTablesList)
            {
                if (c.canInteract)
                {
                    if (stopwatch.IsRunning)
                    {
                        c.isInteracting = true;
                        stopwatch.Stop();
                    }
                    else
                    {
                        c.isInteracting = false;
                        stopwatch.Start();
                    }
                    Popup.Visibility = Popup.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
                    interactCrafting = true;
                }
            }
            if (!interactCrafting)
            {
                foreach (var r in ResourceGeneratorsList)
                {
                    if (r.canInteract)
                    {
                        r.Res.Add(1);
                        UpdateResourceVisual();
                    }
                }
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


    }
}