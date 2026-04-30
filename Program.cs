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

            // moved the old display item block into a method
            DisplayItems(items);

            // same with display items
            DisplayCart(cartItems, cartQuantity, cartCount);

            bool running = true;
            while (running)
            {
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
                    Console.Clear();
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
                        double subtotal = totalItems[i].GetItemTotal(totalQuantity[i]);
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
                    Console.WriteLine("\n--------------------------------");
                    Console.WriteLine($"Grand Total: PHP{total:0.00}");

                    if (discount > 0)
                    {
                        Console.WriteLine($"Discount (10%): PHP{discount:0.00}");
                    }

                    double finalTotal = total - discount;

                    Console.WriteLine($"Final Total: PHP{finalTotal:0.00}");
                    Console.WriteLine("Please Come Again!");
                    Console.WriteLine("----------------------------------------------------------\n");
                    Console.WriteLine("---------------------- Final Stock -----------------------");
                    foreach (Item item in items)
                    {
                        item.DisplayItems();
                    }

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
                //================ INPUT VALIDATION =================
                if (quantity <= 0)
                {
                    Console.WriteLine("You cant grab nothing");
                    continue;
                }

                //================ TOTAL CART LIMIT CHECK =================
                
                int totalCartQuantity = 0;
                for (int i = 0; i < cartCount; i++)
                {
                    totalCartQuantity += cartQuantity[i];
                }

                if (totalCartQuantity + quantity > 50)
                {
                    Console.WriteLine("Cart limit is 50 items total. Cannot add that many.");
                    continue;
                }
                //=========================================================

                if (!selectedItem.HasEnoughStock(quantity))
                {
                    Console.WriteLine("Input quantity is more than current stock");
                    continue;
                }  

                if (cartCount >= cartItems.Length)
                {
                    Console.WriteLine("Cart is full.");
                    continue;
                }

                // deduct
                selectedItem.DeductStock(quantity);

                // add items into cart
                cartItems[cartCount] = selectedItem;
                cartQuantity[cartCount] = quantity;
                cartCount++;

                Console.WriteLine($"\n{selectedItem.Name} added to cart");
                Console.Clear();

                //#####################################################################################

                // moved the old display item block into a method
                DisplayItems(items);

                // same with display items
                DisplayCart(cartItems, cartQuantity, cartCount);
            }
        }
    }
}

