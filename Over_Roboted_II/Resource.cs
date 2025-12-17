using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Over_Roboted_II
{
    internal class Resource
    {
        public string Name { get; }
        public int Amount { get; set; }

        public bool isWaiting = false;

        public Resource(string name, int amount)
        {
            Name = name;
            Amount = amount;
        }

        public async Task Add(int nb)
        {
            isWaiting = true;
            
            Amount += nb;
            
            await Task.Delay(1000);
            isWaiting = false;
        }
 

        public TextBlock Draw()
        {
            TextBlock text = new TextBlock
            {
                Text = $"{this.Name} : {this.Amount}",
                FontSize = 16,
                Foreground = Brushes.White,
                Margin = new Thickness(0, 2, 0, 2)
            };

            return text;
        }
    }
}