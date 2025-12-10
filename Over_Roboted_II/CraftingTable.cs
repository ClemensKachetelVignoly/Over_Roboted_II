using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Over_Roboted_II
{
    internal class CraftingTable
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Rectangle myRect = new Rectangle();

        public CraftingTable(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void Interact()
        {
            
        }

        public void Draw(Canvas canvas)
        {
            myRect.Fill = Brushes.BlueViolet;
            myRect.Height = 80;
            myRect.Width = 160;
            canvas.Children.Add(myRect);
            Canvas.SetLeft(myRect, X);
            Canvas.SetTop(myRect, Y);
        }
        public Rect Hitbox => new Rect(X, Y, 160, 80);
    }
}
