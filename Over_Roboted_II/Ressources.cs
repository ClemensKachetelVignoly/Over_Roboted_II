using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Emit;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace Over_Roboted_II
{
    internal class Ressources
    {
        private static List<Ressources> AllRessources = new List<Ressources>();


        public Image imgR;
        public BitmapImage source1;
        public BitmapImage source2;
        public BitmapImage source3;

        public int X { get; set; }
        public int Y { get; set; }

        public bool canInteract = false;
        public bool isInteracting = false;
        public Ressources(int x, int y)
        {
            X = x;
            Y = y;

            source1 = new BitmapImage(new Uri("pack://application:,,,/Images/ressource1.jpg"));
            source2 = new BitmapImage(new Uri("pack://application:,,,/Images/ressource2.jpg"));
            source3 = new BitmapImage(new Uri("pack://application:,,,/Images/ressource3.jpg"));

            imgR = new Image
            {
                Source = source1
            };

            

            AllRessources.Add(this);
        }
        public void Interact(Rect playerHitbox)
        {
            if (playerHitbox.IntersectsWith(this.InteractHitbox))
            {
                foreach (var c in AllRessources)
                {
                    if (c.imgR.Opacity == 0.7)
                    {
                        c.imgR.Opacity = 1;
                    }
                    imgR.Opacity = 0.7;
                    canInteract = true;
                }
            }
            
        }

        public void Draw(Canvas canvas)
        {
            imgR.Height = 80;
            imgR.Width = 160;
            canvas.Children.Add(imgR);
            Canvas.SetLeft(imgR, X);
            Canvas.SetTop(imgR, Y);
        }
        public Rect Hitbox => new Rect(X, Y, 160, 80);
        public Rect InteractHitbox => new Rect(X - 10, Y - 10, 180, 100);
    }
}
