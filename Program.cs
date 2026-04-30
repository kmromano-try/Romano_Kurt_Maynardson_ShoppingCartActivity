using System.ComponentModel;

namespace Romano_Kurt_Maynardson_ShoppingCartActivity
{
    class Item
    {
        public int Id;
        public string Name = "";
        public double Price;
        public int Stock;
        // added category
        public string Category = "";

        // display
        public void DisplayItems()
        {
            string warning = Stock <= 5 ? "LOW STOCK" : "";

            Console.WriteLine($"| {Id,-3} | {Category,-10} | {Name,-25} | PHP {Price,8:0.00} | {Stock,5} | {warning,-9} |");
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

        // receipt number
        static int receiptCounter = 1;

        // stores receipts
        static string[] orderHistory = new string[50];
        static int orderCount = 0;

        // moved the display item in Main into a method so it only has to be called in main
        static void DisplayItems(Item[] items)
        {
            Console.WriteLine("--------- ITEMS 4 SALE ---------");
            Console.WriteLine("| ID  | CATEGORY  | NAME                      | PRICE      | STOCK | WARNING   |");
            Console.WriteLine("-----------------------------------------------------------------------------------");

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

            Console.WriteLine("\n-----------------------------------\n");
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

        // checkout method, including the discount, payment, change, etc
        static bool Checkout(Item[] cartItems, int[] cartQuantity, ref int cartCount)
        {
            // validation
            if (cartCount == 0)
            {
                Console.WriteLine("Your cart is empty");
                return true;
            }

            Console.Clear();
            Console.WriteLine("\n========= FINAL RECEIPT =========");
            // added receipt and datetime
            Console.WriteLine($"Receipt No: {receiptCounter}");
            Console.WriteLine($"DateTime: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");

            Item[] totalItems = new Item[50];
            int[] totalQuantity = new int[50];
            int totalCount = 0;

            // merge the duplicates (duplicates are allowed in cart for purchase history, it is intentional)
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

            // total
            double total = 0;

            for (int i = 0; i < totalCount; i++)
            {
                double subtotal = totalItems[i].GetItemTotal(totalQuantity[i]);
                total += subtotal;

                Console.WriteLine($"{totalItems[i].Name} x{totalQuantity[i]} = PHP{subtotal:0.00}");
            }

            // discount (also fixed the discount threshhold only applying if the code scans the total and its over 9 thousand)
            double discount = 0;

            if (total >= 5000)
            {
                discount = total * 0.10;
                Console.WriteLine("10% discount applied!");
            }

            Console.WriteLine("\n--------------------------------");
            Console.WriteLine($"Grand Total: PHP{total:0.00}");

            if (discount > 0)
            {
                Console.WriteLine($"Discount: PHP{discount:0.00}");
            }

            double finalTotal = total - discount;
            Console.WriteLine($"Final Total: PHP{finalTotal:0.00}");


            // this part of the code is new, this adds a payment logic where the user inputs how much money they give
            double payment = 0;

            while (true)
            {
                Console.Write("Enter payment amount: ");

                if (!double.TryParse(Console.ReadLine(), out payment))
                {
                    Console.WriteLine("Invalid input.");
                    continue;
                }

                if (payment < finalTotal)
                {
                    Console.WriteLine("Insufficient Funds");
                    continue;
                }

                break;
            }

            // this calculates the change to be given.
            double change = payment - finalTotal;

            Console.WriteLine($"Payment: PHP{payment:0.00}");
            Console.WriteLine($"Change: PHP{change:0.00}");
            Console.WriteLine("Thank you come again!");

            // adds to the receipt number
            receiptCounter++;

            // stores receipt history
            string record = $"Receipt #{receiptCounter} - PHP{finalTotal:0.00}";
            orderHistory[orderCount] = record;
            orderCount++;

            receiptCounter++;

            // prompt the user to go back or exit
            while (true)
            {
                Console.Write("\nGo back? (Y/N): ");
                string? input = Console.ReadLine()?.ToUpper();

                if (input == "Y")
                {
                    cartCount = 0;
                    Console.Clear();
                    return true;
                }
                else if (input == "N")
                {
                    return false;
                }
                else
                {
                    Console.WriteLine("Invalid input.");
                }
            }
        }

        // search for item
        static void SearchItems(Item[] items)
        {
            Console.Write("Search for item: ");
            string? keyword = Console.ReadLine()?.ToLower(); // turns input into lowercase so its not case sensitive

            // validation
            if (string.IsNullOrWhiteSpace(keyword))
            {
                Console.WriteLine("Empty search.");
                return;
            }

            bool found = false;

            Console.WriteLine("\n--------- SEARCH RESULTS ---------");

            int searchId;
            bool isNumber = int.TryParse(keyword, out searchId);

            Console.WriteLine("| ID  | CATEGORY  | NAME                      | PRICE      | STOCK | WARNING   |");
            Console.WriteLine("-----------------------------------------------------------------------------------");
            foreach (Item item in items)
            {
                if (item.Name.ToLower().Contains(keyword) || item.Category.ToLower().Contains(keyword) || (isNumber && item.Id == searchId)) // searches to all 3 for a match
                {
                    item.DisplayItems();
                    found = true;
                }
            }

            if (!found)
            {
                Console.WriteLine("No items found");
            }

            Console.WriteLine("\n-----------------------------------\n");
        }

        // remove item from cart
        static void RemoveItem(Item[] cartItems, int[] cartQuantity, ref int cartCount)
        {
            // validations
            if (cartCount == 0)
            {
                Console.WriteLine("Cart is empty");
                return;
            }

            Console.WriteLine("\nSelect index of item to remove:");

            // display current cart items with index
            for (int i = 0; i < cartCount; i++)
            {
                Console.WriteLine($"[{i}] {cartItems[i].Name} x{cartQuantity[i]}");
            }

            Console.Write("Enter index: ");
            int index;

            if (!int.TryParse(Console.ReadLine(), out index))
            {
                Console.WriteLine("Invalid input");
                return;
            }

            if (index < 0 || index >= cartCount)
            {
                Console.WriteLine("Invalid index");
                return;
            }

            // puts item back
            cartItems[index].Stock += cartQuantity[index];

            Console.WriteLine($"{cartItems[index].Name} removed from cart.");

            // shift items left to fill empty slot
            for (int i = index; i < cartCount - 1; i++)
            {
                cartItems[i] = cartItems[i + 1];
                cartQuantity[i] = cartQuantity[i + 1];
            }

            // -1
            cartCount--;
        }

        // clear cart
        static void ClearCart(Item[] cartItems, int[] cartQuantity, ref int cartCount)
        {
            for (int i = 0; i < cartCount; i++)
            { // put items in cart back
                cartItems[i].Stock += cartQuantity[i];
            }

            cartCount = 0;
            Console.WriteLine("Cart cleared.");
        }

        // manage cart menu
        static void ManageCart(Item[] cartItems, int[] cartQuantity, ref int cartCount)
        {
            bool managing = true;

            while (managing)
            {
                Console.Clear();
                DisplayCart(cartItems, cartQuantity, cartCount);

                Console.WriteLine("=== MANAGE CART ===");
                Console.WriteLine("1 - Remove Item");
                Console.WriteLine("2 - Clear Cart");
                Console.WriteLine("0 - Back");
                Console.Write("Choose option: ");

                int choice;

                if (!int.TryParse(Console.ReadLine(), out choice))
                {
                    Console.WriteLine("Invalid input");
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        RemoveItem(cartItems, cartQuantity, ref cartCount);
                        break;

                    case 2:
                        ClearCart(cartItems, cartQuantity, ref cartCount);
                        break;

                    case 0:
                        managing = false;
                        break;

                    default:
                        Console.WriteLine("Invalid option");
                        break;
                }
            }
        }

        // receipt history
        static void ShowOrderHistory()
        {
            Console.WriteLine("\n========= ORDER HISTORY =========");

            if (orderCount == 0)
            {
                Console.WriteLine("No transactions yet.");
            }
            else
            { // loop through order history and print
                for (int i = 0; i < orderCount; i++)
                {
                    Console.WriteLine(orderHistory[i]);
                }
            }

            Console.WriteLine("\nPress Enter to return...");
            Console.ReadLine();
        }

        // main
        static void Main(string[] args)
        {
            //################################################### ITEMS AREA ########################################################

            Item[] items = new Item[]
            {
                new () { Id = 1, Category = "Sweets", Name = "Hany", Price = 4, Stock = 24},
                new () { Id = 2, Category = "Sweets", Name = "Karate Belt", Price = 2, Stock = 30},
                new () { Id = 3, Category = "Sweets", Name = "Le Chocolat Bar", Price = 9000, Stock = 4},
                new () { Id = 4, Category = "Sweets", Name = "Frutos", Price = 1, Stock = 10},
                new () { Id = 5, Category = "Sweets", Name = "Stick O", Price = 1, Stock = 50},
                new () { Id = 6, Category = "Brainrot", Name = "Six", Price = 7, Stock = 67},
                new () { Id = 7, Category = "Sweets", Name = "Chocolate Bar", Price = 50, Stock = 64},
                new () { Id = 8, Category = "Brainrot", Name = "Shark", Price = 500, Stock = 50},
                new () { Id = 9, Category = "Brainrot", Name = "Wooden Bat", Price = 250, Stock = 76},
                new () { Id = 10, Category = "Vegetables", Name = "Tomato", Price = 9, Stock = 55},
                new () { Id = 11, Category = "Vegetables", Name = "Potato", Price = 12, Stock = 25},
                new () { Id = 12, Category = "Vegetables", Name = "Carrot", Price = 15, Stock = 17},
                new () { Id = 13, Category = "Vegetables", Name = "Okra", Price = 6, Stock = 20},
            };

            Item[] cartItems = new Item[50]; 
            int[] cartQuantity = new int[50];
            int cartCount = 0;

            //################################################### DISPLAY ITEMS AND CART ########################################################

            Console.Clear();

            bool showCart = false;
            bool running = true;
            while (running)
            {
                DisplayItems(items);

                if (showCart == true)
                {
                    DisplayCart(cartItems, cartQuantity, cartCount);
                }

                Console.WriteLine("\n========= MENU =========");
                Console.WriteLine("1 - Add Item to Cart");
                Console.WriteLine("2 - Toggle Cart");
                Console.WriteLine("3 - Checkout");
                Console.WriteLine("4 - Search Item By ID, Name, or Category");
                Console.WriteLine("5 - Manage Cart");
                Console.WriteLine("6 - View Order History");
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
                        Console.Clear();
                        break;

                    case 2:
                        Console.Clear();
                        if (showCart == false) // instead of show cart i made it so that it toggles cart instead so you dont have to keep showing cart to see what you have, its just better
                        {
                            showCart = true;
                        }
                        else
                        {
                            showCart = false;
                        }
                        break;

                    case 3:
                        running = Checkout(cartItems, cartQuantity, ref cartCount);
                        break;

                    case 4: // search
                        Console.Clear();
                        SearchItems(items);
                        break;

                    case 5:
                        ManageCart(cartItems, cartQuantity, ref cartCount);
                        break;

                    case 6:
                        Console.Clear();
                        ShowOrderHistory();
                        break;

                    default:
                        Console.WriteLine("Invalid Input");
                        break;
                }
            }
        }
    }
}
