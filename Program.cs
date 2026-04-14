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

            //################################################### ITEMS AREA ########################################################

            Item[] items = new Item[]
            {
                new () { Id = 1, Name = "Hany", Price = 4, Stock = 24},
                new () { Id = 2, Name = "Karate Belt", Price = 2, Stock = 30},
                new () { Id = 3, Name = "Attack Helicopter", Price = 40000000, Stock = 4},
                new () { Id = 5, Name = "Frutos", Price = 1, Stock = 10},
            };

            //################################################### DISPLAY ITEMS ########################################################

            Console.WriteLine("--------- ITEMS 4 SALE ---------");
            Console.WriteLine(" | ID | Name       | Price   | Stock |");
            Console.WriteLine("--------------------------------------");

            foreach (Item item in items)
            {
                item.DisplayItems();
            }

            //################################################### CART AREA ########################################################

            Item? selectedItem = null;
            int quantity = 0;

            Console.WriteLine("\n--------- YOUR CART ---------");
            Console.WriteLine(" (empty)\n");
            Console.WriteLine("--------------------------------------\n");

            //################################################### USER INPUT ########################################################

            Console.Write("Enter product ID: ");
            int sid = Convert.ToInt32(Console.ReadLine());

            foreach (Item item in items)
            {
                if (item.Id == sid)
                {
                    selectedItem = item;
                    break;
                }
            }

            if (selectedItem == null)
            {
                Console.WriteLine("Input ID not found");
                return;
            }

            Console.Write("Enter quantity: ");
            quantity = Convert.ToInt32(Console.ReadLine());

            if (quantity > selectedItem.Stock)
            {
                Console.WriteLine("Input quantity is more than current stock");
                return;
            }

            selectedItem.Stock -= quantity;

            //################################################### OUTPUT ########################################################

            Console.WriteLine("\n--------- YOUR CART ---------");
            Console.WriteLine($" Item: {selectedItem.Name} | Quantity: {quantity} | Total: PHP{selectedItem.Price * quantity}");

            Console.WriteLine($"\n {selectedItem.Name} has been purchased");
        }
    }
}



