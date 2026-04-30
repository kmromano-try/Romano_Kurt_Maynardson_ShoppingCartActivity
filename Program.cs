using System.ComponentModel;

namespace Romano_Kurt_Maynardson_ShoppingCartActivity
{
    class Item
    {
        public int Id;
        public string Name = "";
        public double Price;
        public int Stock;

        // display
        public void DisplayItems()
        {
            Console.WriteLine(
                $"| {Id,-3} | {Name,-25} | PHP {Price,8:0.00} | {Stock,5} |");
        }

        // subtotal
        public double GetItemTotal(int quantity)
        {
            return Price * quantity;
        }

        // check stock
        public bool HasEnoughStock(int quantity)
        {
            return quantity <= Stock;
        }

        // deduct stock
        public void DeductStock(int quantity)
        {
            Stock -= quantity;
        }
    }

    internal class Program
    {
        // moved the display item in Main into a method so it only has to be called in main
        static void DisplayItems(Item[] items)
        {
            Console.WriteLine("--------- ITEMS 4 SALE ---------");
            Console.WriteLine("| ID  | NAME                      | PRICE      | STOCK |");
            Console.WriteLine("----------------------------------------------------------");

            foreach (Item item in items)
            {
                item.DisplayItems();
            }
        }

        // also turned display cart into a method to be called in main
        static void DisplayCart(Item[] cartItems, int[] cartQuantity, int cartCount)
        {
            Console.WriteLine("\n--------- YOUR CART --------- ");
            Console.WriteLine("(Duplicate items intentionally don't merge, its for purchase history)");

            if (cartCount == 0)
            {
                Console.WriteLine(" (empty)");
            }
            else
            {
                for (int i = 0; i < cartCount; i++)
                {
                    Console.WriteLine($"| {cartItems[i].Name,-25} | x{cartQuantity[i],2} | PHP {(cartItems[i].GetItemTotal(cartQuantity[i])):0.00,10} |");
                }
            }

            Console.WriteLine("\n--------------------------------------\n");
        }

        // moving the whole user input and add to cart logic into a method as well,
        static void AddItem(Item[] items, Item[] cartItems,int[] cartQuantity,ref int cartCount)
        {
            Console.Write("Enter product ID (0 to exit): ");
            int sid;

            if (!int.TryParse(Console.ReadLine(), out sid))
            {
                Console.WriteLine("Invalid input. Please enter a number.");
                return;
            }

            if (sid == 0)
            {
                return;
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
                return;
            }

            Console.WriteLine($"Item Selected: {selectedItem.Name}");
            Console.Write("Enter quantity: ");
            int quantity;

            if (!int.TryParse(Console.ReadLine(), out quantity))
            {
                Console.WriteLine("Invalid input. Please enter a number.");
                return;
            }

            if (quantity <= 0)
            {
                Console.WriteLine("You cant grab nothing");
                return;
            }

            int totalCartQuantity = 0;
            for (int i = 0; i < cartCount; i++)
            {
                totalCartQuantity += cartQuantity[i];
            }

            if (totalCartQuantity + quantity > 50)
            {
                Console.WriteLine("Cart limit is 50 items total. Cannot add that many.");
                return;
            }

            if (!selectedItem.HasEnoughStock(quantity))
            {
                Console.WriteLine("Input quantity is more than current stock");
                return;
            }

            if (cartCount >= cartItems.Length)
            {
                Console.WriteLine("Cart is full.");
                return;
            }

            selectedItem.DeductStock(quantity);

            cartItems[cartCount] = selectedItem;
            cartQuantity[cartCount] = quantity;
            cartCount++;

            Console.WriteLine($"\n{selectedItem.Name} added to cart");
        }

        // will add a method for checkout below



        // main
        static void Main(string[] args)
        {
            //################################################### ITEMS AREA ########################################################

            Item[] items = new Item[]
            {
                new () { Id = 1, Name = "Hany", Price = 4, Stock = 24},
                new () { Id = 2, Name = "Karate Belt", Price = 2, Stock = 30},
                new () { Id = 3, Name = "Le Chocolat Bar", Price = 9000, Stock = 4},
                new () { Id = 4, Name = "Frutos", Price = 1, Stock = 10},
                new () { Id = 5, Name = "Stick O", Price = 1, Stock = 50},
                new () { Id = 6, Name = "Six", Price = 7, Stock = 67},
                new () { Id = 7, Name = "Chocolate Bar", Price = 50, Stock = 64},
            };

            Item[] cartItems = new Item[10]; 
            int[] cartQuantity = new int[10];
            int cartCount = 0;

            //################################################### DISPLAY ITEMS AND CART ########################################################

            Console.Clear();
            DisplayItems(items);
            // removed the old display codes to implement the menu system

            bool running = true;
            while (running)
            {
                // implementing menu system

                Console.WriteLine("\n========= MENU =========");
                Console.WriteLine("1 - Add Item to Cart");
                Console.WriteLine("2 - Toggle Cart");
                Console.WriteLine("3 - Checkout");
                Console.WriteLine("0 - Exit");
                Console.Write("Choose option: ");

                int choice;

                if (!int.TryParse(Console.ReadLine(), out choice))
                {
                    Console.WriteLine("Invalid input. Enter a number.");
                    continue;
                }

                switch (choice)
                {
                    case 0:
                        running = false;
                        break;

                    case 1:
                        AddItem(items, cartItems, cartQuantity, ref cartCount);
                        break;

                    case 2:
                        Console.Clear();
                        DisplayItems(items);
                        DisplayCart(cartItems, cartQuantity, cartCount);
                        break;

                    case 3:
                        //checkout
                        break;

                    default:
                        Console.WriteLine("Invalid Input");
                        break;
                }
            }
        }
    }
}

