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
    }
}