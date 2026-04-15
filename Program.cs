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

            Item[] cartItems = new Item[10];
            int[] cartQty = new int[10];
            int cartCount = 0;

            bool running = true;


            while (running)
            {
                Console.WriteLine("\n--------- YOUR CART ---------");

                if (cartCount == 0)
                {
                    Console.WriteLine(" (empty)");
                }
                else
                {
                    for (int i = 0; i < cartCount; i++)
                    {
                        Console.WriteLine($" {cartItems[i].Name} x{cartQty[i]} = PHP{cartItems[i].Price * cartQty[i]}");
                    }
                }

                Console.WriteLine("\n--------------------------------------\n");

                ////################################################### USER INPUT ########################################################

                Console.Write("Enter product ID (0 to exit): ");
                int sid = Convert.ToInt32(Console.ReadLine());

                if (sid == 0)
                {
                    running = false;
                    break;
                }

                Item? selectedItem = null;

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
                    continue;
                }

                Console.WriteLine($"Item Selected: {selectedItem.Name}");
                Console.Write("Enter quantity: ");
                int quantity = Convert.ToInt32(Console.ReadLine());

                if (quantity > selectedItem.Stock)
                {
                    Console.WriteLine("Input quantity is more than current stock");
                    continue;
                }

                selectedItem.Stock -= quantity;

                cartItems[cartCount] = selectedItem;
                cartQty[cartCount] = quantity;
                cartCount++;

                Console.WriteLine($"\n{selectedItem.Name} added to cart!");
            }
        }
    }
}



