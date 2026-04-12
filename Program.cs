namespace Romano_Kurt_Maynardson_ShoppingCartActivity
{
    class Item
    {
        public int Id;
        public string Name = "";
        public double Price;
        public int Stock;

        public void DisplayItems()
        {
            Console.WriteLine($" | {Id} | {Name} | PHP{Price} | Stock: {Stock} |");
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("ZAWARUDOOO");
            // SAMPLE ITEM AND TEST
            Item i1 = new Item();
            i1.Id = 1;
            i1.Name = "Hany";
            i1.Price = 4;
            i1.Stock = 67;

            i1.DisplayItems();
            Console.WriteLine("Name: " + i1.Name);
            Console.WriteLine("Item ID: " + i1.Id);
            Console.WriteLine("Price: " + i1.Price);
            Console.WriteLine("Stock: " + i1.Stock);


        }
    }
}



