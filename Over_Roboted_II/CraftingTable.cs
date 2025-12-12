using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
        private static List<CraftingTable> AllCraftingTables = new List<CraftingTable>();

        public int X { get; set; } 
        public int Y { get; set; }

        public Rectangle myRect = new Rectangle();

        public bool canInteract = false;
        public bool isInteracting = false;

        private bool isDragging = false;
        private Point dragStartMouse;
        private Point dragStartBlock;

        public static Rect oldHitbox = new Rect();

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

            oldHitbox = this.Hitbox;
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

                bool changeColor = false;

                for (int i = 0;  i < AllCraftingTables.Count; i++)
                {
                    if (i == AllCraftingTables.IndexOf(this)) continue;
                    else
                    {
                        if (this.Hitbox.IntersectsWith(AllCraftingTables[i].Hitbox) || this.Hitbox.IntersectsWith(UCGame.playerHitbox))
                        {
                            changeColor = true;
                        }
                    }
                }

                if (changeColor) myRect.Fill = Brushes.Red;
                else myRect.Fill = Brushes.BlueViolet;
                changeColor = false;
            }
        }
        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;
            myRect.ReleaseMouseCapture();
            
            if (myRect.Fill == Brushes.Red)
            {
                Canvas.SetLeft(myRect, oldHitbox.X);
                Canvas.SetTop(myRect, oldHitbox.Y);
                this.X = (int)oldHitbox.X;
                this.Y = (int)oldHitbox.Y;

                myRect.Fill = Brushes.BlueViolet;
                oldHitbox = new Rect();
            }
        }

        public void Interact(Rect playerHitbox)
        {
            if (playerHitbox.IntersectsWith(this.InteractHitbox) && myRect.Fill != Brushes.Red)
            {
                foreach (var c in AllCraftingTables)
                {
                    if (c.myRect.Fill == Brushes.Black)
                    {
                        c.myRect.Fill = Brushes.BlueViolet;
                    }
                    myRect.Fill = Brushes.Black;
                    canInteract = true;
                }
            }
            else
            {
                if (myRect.Fill != Brushes.Red)
                {
                    myRect.Fill = Brushes.BlueViolet;
                    canInteract = false;
                }
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
        public Rect InteractHitbox => new Rect(X-10, Y-10, 180, 100);
    }
}