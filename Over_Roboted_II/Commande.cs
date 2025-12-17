using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Over_Roboted_II
{
    internal class Commande
    {
        public int nbTete;
        public int nbJambe;
        public int nbCorps;
        public int nbBras;
        public Canvas Cnv { get; set; }

        public Commande(Canvas canvas)
        {
            Cnv = canvas;

            Random rnd = new Random();

            Image imgTete = new Image(), imgJambe = new Image(), imgCorps = new Image(), imgBras = new Image(), imgBras2 = new Image();

            List<Image> imgList = new List<Image>();
            imgList.AddRange(new List<Image>
            {
                imgTete, imgJambe, imgCorps, imgBras, imgBras2
            });

            string[] img = { "tête", "jambes", "corps", "bras", "bras2" };

            for (int i = 0; i < img.Length; i++)
            {
                {
                    int nb = rnd.Next(1, 4);
                    imgList[i].Source = new BitmapImage(new Uri($"pack://application:,,,/Images/Robot/{img[i]}{nb}.png"));

                    imgList[i].Width = 50;
                    imgList[i].Height = 50;

                    canvas.Children.Add(imgList[i]);

                    switch (img[i])
                    {
                        case "tête":
                            Canvas.SetTop(imgList[i], 500);
                            
                            break;

                        case "corps":
                            Canvas.SetTop(imgList[i], 540);
                            
                            break;

                        case "jambes":
                            Canvas.SetTop(imgList[i], 577);
                            
                            break;

                        default:
                            Canvas.SetTop(imgList[i], 550);
                            
                            break;
                    }

                    switch (img[i])
                    {
                        case "bras":
                            Canvas.SetLeft(imgList[i], 27);
                            break;

                        case "bras2":
                            Canvas.SetLeft(imgList[i], 73);
                            imgList[i].FlowDirection = FlowDirection.RightToLeft;
                            break;

                        default:
                            Canvas.SetLeft(imgList[i], 50);
                            break;
                    }
                }
            }
        }
    }
} 
