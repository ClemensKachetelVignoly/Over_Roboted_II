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
    internal class CraftingTable
    {
        private static List<CraftingTable> AllCraftingTables = new List<CraftingTable>();

        public int X { get; set; } 
        public int Y { get; set; }

        public Image imgCT;
        public BitmapImage source;
        public BitmapImage sourceOK;
        public BitmapImage sourceError;

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

            source = new BitmapImage(new Uri("pack://application:,,,/Images/CraftingTable.jpg"));
            sourceOK = new BitmapImage(new Uri("pack://application:,,,/Images/CraftingTableOK.jpg"));
            sourceError = new BitmapImage(new Uri("pack://application:,,,/Images/CraftingTableError.jpg"));

            imgCT = new Image {
                Source = source
            };

            imgCT.MouseLeftButtonDown += OnMouseDown;
            imgCT.MouseLeftButtonUp += OnMouseUp;
            imgCT.MouseMove += OnMouseMove;

            AllCraftingTables.Add(this);
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            isDragging = true;

            imgCT.CaptureMouse();
            
            dragStartMouse = e.GetPosition(imgCT.Parent as UIElement);
            dragStartBlock = new Point(X, Y);

            e.Handled = true;

            oldHitbox = this.Hitbox;

            imgCT.Opacity = 0.6;

            Canvas.SetZIndex(imgCT, 1);
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Point mousePos = e.GetPosition(imgCT.Parent as UIElement);

                double dx = mousePos.X - dragStartMouse.X;
                double dy = mousePos.Y - dragStartMouse.Y;

                X = (int)(dragStartBlock.X + dx);
                Y = (int)(dragStartBlock.Y + dy);

                Canvas.SetLeft(imgCT, X);
                Canvas.SetTop(imgCT, Y);

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

                if (changeColor) imgCT.Source = sourceError;
                else imgCT.Source = sourceOK;
            }
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;
            imgCT.ReleaseMouseCapture();

            if (imgCT.Source == sourceError)
            {
                Canvas.SetLeft(imgCT, oldHitbox.X);
                Canvas.SetTop(imgCT, oldHitbox.Y);
                this.X = (int)oldHitbox.X;
                this.Y = (int)oldHitbox.Y;

                oldHitbox = new Rect();
            }

            imgCT.Source = source;
            imgCT.Opacity = 1;
            Canvas.SetZIndex(imgCT, 0);
        }

        public void Interact(Rect playerHitbox)
        {
            if (playerHitbox.IntersectsWith(this.InteractHitbox) && imgCT.Source != sourceError)
            {
                foreach (var c in AllCraftingTables)
                {
                    if (c.imgCT.Opacity == 0.7)
                    {
                        c.imgCT.Opacity = 1;
                    }
                    imgCT.Opacity = 0.7;
                    canInteract = true;
                }
            }
            else
            {
                if (imgCT.Source != sourceError)
                {
                    imgCT.Opacity = isDragging ? 0.6 : 1;
                    canInteract = false;
                }
            }
        }

        public void Draw(Canvas canvas)
        {
            imgCT.Height = 80;
            imgCT.Width = 160;
            canvas.Children.Add(imgCT);
            Canvas.SetLeft(imgCT, X);
            Canvas.SetTop(imgCT, Y);
        }

        public Rect Hitbox => new Rect(X, Y, 160, 80);
        public Rect InteractHitbox => new Rect(X-10, Y-10, 180, 100);
    }
}