using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Over_Roboted_II
{
    internal class CraftingTable
    {
        public static List<CraftingTable> AllCraftingTables = new List<CraftingTable>();

        public int X { get; set; }
        public int Y { get; set; }

        public Rectangle myRect = new Rectangle();

        public bool canInteract = false;
        public bool isInteracting = false;

        private bool isDragging = false;
        private Point dragStartMouse;
        private Point dragStartBlock;

        public CraftingTable(int x, int y)
        {
            X = x;
            Y = y;

            myRect.MouseLeftButtonDown += OnMouseDown;
            myRect.MouseLeftButtonUp += OnMouseUp;
            myRect.MouseMove += OnMouseMove;

            AllCraftingTables.Add(this);
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            isDragging = true;
            myRect.CaptureMouse();

            dragStartMouse = e.GetPosition(myRect.Parent as UIElement);
            dragStartBlock = new Point(X, Y);

            e.Handled = true;
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Point mousePos = e.GetPosition(myRect.Parent as UIElement);

                double dx = mousePos.X - dragStartMouse.X;
                double dy = mousePos.Y - dragStartMouse.Y;

                X = (int)(dragStartBlock.X + dx);
                Y = (int)(dragStartBlock.Y + dy);

                Canvas.SetLeft(myRect, X);
                Canvas.SetTop(myRect, Y);

                foreach (var item in AllCraftingTables)
                {
                    if (item == this) continue;

                    if (Hitbox.IntersectsWith(item.Hitbox))
                    {
                        Console.WriteLine("zr");
                        myRect.Fill = Brushes.Red;
                    }
                    else myRect.Fill = Brushes.BlueViolet;
                }
            }
        }
        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;
            myRect.ReleaseMouseCapture();
        }

        public void Interact(Rect playerHitbox)
        {
            if (playerHitbox.IntersectsWith(this.InteractHitbox))
            {
                myRect.Fill = Brushes.Black;
                canInteract = true;
            }
            else
            {
                //myRect.Fill = Brushes.BlueViolet;
                canInteract = false;
            }
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
        public Rect InteractHitbox => new Rect(X, Y+50, 160, 40);
    }
}