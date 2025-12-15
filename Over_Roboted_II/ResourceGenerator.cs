using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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
    internal class ResourceGenerator
    {
        private static List<ResourceGenerator> AllRessourceGenerators = new List<ResourceGenerator>();

        public int X { get; set; }
        public int Y { get; set; }
        public Resource Res { get; set; }

        public Image imgR;
        public BitmapImage source;

        public bool canInteract = false;

        public ResourceGenerator(int x, int y, Resource resource)
        {
            X = x;
            Y = y;
            Res = resource;

            switch (Res.Name)
            {
                case "gold" :
                    source = new BitmapImage(new Uri("pack://application:,,,/Images/ressource1.jpg"));
                    break;
                case "diamond" :
                    source = new BitmapImage(new Uri("pack://application:,,,/Images/ressource2.jpg"));
                    break;
                case "copper" :
                    source = new BitmapImage(new Uri("pack://application:,,,/Images/ressource3.jpg"));
                    break;
                default:
                    source = new BitmapImage(new Uri("pack://application:,,,/Images/ressource1.jpg"));
                    break;
            }

            imgR = new Image
            {
                Source = source
            };

            AllRessourceGenerators.Add(this);
        }
        public void Interact(Rect playerHitbox)
        {
            if (playerHitbox.IntersectsWith(this.InteractHitbox))
            {
                foreach (var c in AllRessourceGenerators)
                {
                    c.imgR.Opacity = 1;
                }
                this.imgR.Opacity = 0.7;
                canInteract = true;
            } else
            {
                imgR.Opacity = 1;
                canInteract= false;
            }
        }

        public void Draw(Canvas canvas)
        {
            imgR.Height = 130;
            imgR.Width = 120;
            canvas.Children.Add(imgR);
            Canvas.SetLeft(imgR, X);
            Canvas.SetTop(imgR, Y);
        }

        public Rect Hitbox => new Rect(X, Y, 130, 120);
        public Rect InteractHitbox => new Rect(X - 10, Y - 10, 150, 140);
    }
}
