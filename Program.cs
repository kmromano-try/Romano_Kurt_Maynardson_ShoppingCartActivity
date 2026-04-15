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
                new () { Id = 3, Name = "Something na mamahalin", Price = 5000, Stock = 4},
                new () { Id = 4, Name = "Frutos", Price = 1, Stock = 10},
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
            int[] cartQuantity = new int[10];
            int cartCount = 0;

            bool running = true;


            while (running)
            {
                Console.WriteLine("\n--------- YOUR CART --------- (Duplicate items intentionally don't merge, its for purchase history)");

                if (cartCount == 0)
                {
                    Console.WriteLine(" (empty)");
                }
                else
                {
                    for (int i = 0; i < cartCount; i++)
                    {
                        Console.WriteLine($" {cartItems[i].Name} x{cartQuantity[i]} = PHP{cartItems[i].Price * cartQuantity[i]:0.00}");
                    }
                }

                Console.WriteLine("\n--------------------------------------\n");

                //################################################### USER INPUT ########################################################

                Console.Write("Enter product ID (0 to exit): ");
                int sid;

                if (!int.TryParse(Console.ReadLine(), out sid))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    continue;
                }


                //======================= RECEIPT CALCULATION AND DISPLAY =========================
                if (sid == 0)
                {
                    Console.WriteLine("\n========= FINAL RECEIPT =========");

                    Item[] totalItems = new Item[10];
                    int[] totalQuantity = new int[10];
                    int totalCount = 0;

                    for (int i = 0; i < cartCount; i++)
                    {
                        bool found = false;

                        for (int j = 0; j < totalCount; j++)
                        {
                            if (totalItems[j].Id == cartItems[i].Id)
                            {
                                totalQuantity[j] += cartQuantity[i];
                                found = true;
                                break;
                            }
                        }

                        if (!found)
                        {
                            totalItems[totalCount] = cartItems[i];
                            totalQuantity[totalCount] = cartQuantity[i];
                            totalCount++;
                        }
                    }
                    //-------------------------------------- CALCULATION -------------------------------------------
                    double total = 0;

                    for (int i = 0; i < totalCount; i++)
                    {
                        double subtotal = totalItems[i].Price * totalQuantity[i];
                        total += subtotal;

                        Console.WriteLine($" {totalItems[i].Name} x{totalQuantity[i]} = PHP{subtotal:0.00}");
                    }
                    //-------------------------------------- DISCOUNT ----------------------------------------------
                    double discount = 0;

                    if (total >= 5000)
                    {
                        discount = total * 0.10;
                        Console.WriteLine("PHP 5,000 has been exceeded, you now qualify for a 10% discount, wow!");
                    }
                    //-------------------------------------- DISPLAY -----------------------------------------------
                    Console.WriteLine("--------------------------------");
                    Console.WriteLine($"Grand Total: PHP{total:0.00}");

                    if (discount > 0)
                    {
                        Console.WriteLine($"Discount (10%): PHP{discount:0.00}");
                    }

                    double finalTotal = total - discount;

                    Console.WriteLine($"Final Total: PHP{finalTotal:0.00}");
                    Console.WriteLine("Please Come Again!");

                    break;
                }

                //=========================================================================================

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
                int quantity;

                if (!int.TryParse(Console.ReadLine(), out quantity))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    continue;
                }

                if (quantity > selectedItem.Stock)
                {
                    Console.WriteLine("Input quantity is more than current stock");
                    continue;
                }

                selectedItem.Stock -= quantity;

                cartItems[cartCount] = selectedItem;
                cartQuantity[cartCount] = quantity;
                cartCount++;

                Console.WriteLine($"\n{selectedItem.Name} added to cart");
            }
        }
    }
}

