using System.Reflection.Emit;
using System.Resources;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Over_Roboted_II
{
    internal class Resource
    {
        public string Name { get; }
        public int Amount { get; set; }

        public Resource(string name, int amount)
        {
            Name = name;
            Amount = amount;
        }

        public void Add(int nb)
        {
            Amount += nb;
        }

        //public void 

        public TextBlock Draw()
        {
            TextBlock text = new TextBlock
            {
                Text = $"{this.Name} : {this.Amount}",
                FontSize = 16,
                Foreground = Brushes.Black,
                Margin = new Thickness(0, 2, 0, 2)
            };

            return text;
        }
    }
}