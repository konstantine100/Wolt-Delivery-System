using Microsoft.EntityFrameworkCore;
using Wolt.Data;
using Wolt.Models;
using Wolt.Validator;
using Wolt.SMTP;
using BCrypt.Net;
using System.Globalization;
using System.Text;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace Wolt.Services;

public class StoreService
{
    public static string baseRoute = @"C:\\Users\\kmami\\RiderProjects\\Wolt\\Logs";
    public static string storeLog = "store.txt";
    static public void WoltColor()
    {
        Console.ForegroundColor = ConsoleColor.DarkCyan;
    }
    static public DataContext _context = new DataContext();
    static public void Line()
    {
        Console.WriteLine("==============================================");
    }
    static public void Janitor()
    {
        Console.Clear();
    }
    static public void AddStore(Owner owner)
    {
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Janitor();
        Line();
        Console.WriteLine("Welcome to Store Add Section!");
        Console.WriteLine("Enter Store Name:");
        string name = Console.ReadLine();
        Console.WriteLine();
        
        Console.WriteLine("Enter Store City:");
        string city = Console.ReadLine();
        Console.WriteLine();

        Console.WriteLine("Enter Store Address:");
        string address = Console.ReadLine();
        Console.WriteLine();

        Console.WriteLine("Enter Store Phone Number:");
        string phone = Console.ReadLine();
        Console.WriteLine();
        
        Console.WriteLine("Enter Store Order Fee:");
        decimal orderFee = decimal.Parse(Console.ReadLine());
        Console.WriteLine();

        Console.WriteLine("Enter Store Estimated Order Time:");
        TimeSpan orderTime = TimeSpan.Parse(Console.ReadLine());
        Console.WriteLine();
        Console.WriteLine("Please Wait...");

        Store newStore = new Store()
        {
            Name = name,
            City = city,
            Address = address,
            Phone = phone,
            OrderFee = orderFee,
            OrderTime = orderTime,
            Rating = 0,
            ParticipantNumber = 0,
            ParticipantScore = 0,
            OwnerId = owner.Id,
        };
        
        var validator = new StoreValidator();
        var result = validator.Validate(newStore);

        if (!result.IsValid)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Janitor();
            Line();
            foreach(var error in result.Errors)
            {
                Console.WriteLine($"ERROR: Wrong {error.PropertyName} - {error.ErrorMessage}");
            }
            Line();
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Janitor();
            Console.WriteLine("Saving Info...");
            
            _context.Stores.Add(newStore);
            _context.SaveChanges();
            Janitor();
            string fullRoute = Path.Combine(baseRoute, storeLog);

            using (StreamWriter wr = new StreamWriter(fullRoute, true))
            {
                wr.WriteLine($"[LOG] Store added -> [{newStore.Name}] - {DateTime.Now}");
            }
            Console.WriteLine($"Owner {owner.FirstName} {owner.LastName} Created new Store '{newStore.Name}'");
        }
    }
    static public void AddDetails(Owner owner)
    {
        WoltColor();
        Janitor();
        Line();
        Console.WriteLine($"{owner.FirstName} {owner.LastName}'s Stores:");
        Console.WriteLine();
        
        var allVenues = _context.Stores
                    .Where(x=>x.OwnerId == owner.Id)
                    .ToList();
        
        foreach (var store in allVenues)
        {
            Console.WriteLine($"{store.Name} - ({store.Id})");
            Console.WriteLine();
        }

        Console.WriteLine("Enter Stores ID to see and add more details:");
        int choice = int.Parse(Console.ReadLine());
        var choosenStore = _context.Stores
            .Include(x => x.Category)
            .ThenInclude(x => x.Schedule)
            .Include(x => x.Category.Prodaction)
            .ThenInclude(x => x.Categories)
            .ThenInclude(x => x.Products)
            .FirstOrDefault(x => x.Id == choice);

        if (choosenStore == null)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Janitor();
            Line();
            Console.WriteLine("Enter Venue ID!");
        }
        else
        {
            Janitor();
            Line();
            Console.WriteLine($"{choosenStore.Name} Info:");
            Console.WriteLine();
            Console.WriteLine($"City : {choosenStore.City}");
            Console.WriteLine($"Address : {choosenStore.Address}");
            Console.WriteLine($"Phone Number : {choosenStore.Phone : N0}");
            Console.WriteLine($"Order Fee : {choosenStore.OrderFee}");
            Console.WriteLine($"Order Time : {choosenStore.OrderTime}");
            Console.WriteLine($"Rating : {choosenStore.Rating}");
            Console.WriteLine();
            Console.WriteLine("1) Change Current Information");
            Console.WriteLine("2) Add Store Category and Create Prodaction Configuration!");
            Console.WriteLine("3) Add Prodaction Category");
            Console.WriteLine("4) Add Products");
            Console.WriteLine("5) See Store All Info");
            Console.WriteLine("6) Change Prodaction Information");
            Console.WriteLine("7) Add Store Scedule");
            Console.WriteLine("8) Change Store Scedule");
            Console.WriteLine("9) Delete Info");
            Console.WriteLine("10) Exit");
            string input = Console.ReadLine();
            Choices inputChoices = new Choices
            {
                Choice = input,
            };
            var validator = new ChoicesValidator(10);
            var result = validator.Validate(inputChoices);
            
            if (result.IsValid == false)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Janitor();
                Line();
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"ERROR: Wrong {error.PropertyName} - {error.ErrorMessage}");
                }
            }
            else if (input == "1")
            {
                ChangeStore(choosenStore);
            }
            else if (input == "2")
            {
                ProductionConfiguration(choosenStore);
            }
            else if (input == "3")
            {
                if (choosenStore.Category == null || choosenStore.Category.Prodaction == null)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Janitor();
                    Line();
                    Console.WriteLine("You must Configure Production First!");
                }
                else
                {
                    AddProductionCategory(choosenStore.Category.Prodaction);
                }
            }
            else if (input == "4")
            {
                if (choosenStore.Category.Prodaction.Categories == null)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Janitor();
                    Line();
                    Console.WriteLine("You must Create Production Category First!");
                }
                AddProducts(choosenStore.Category.Prodaction);
            }
            else if (input == "5")
            {
                SeeAllProdaction(choosenStore);
            }
            else if (input == "6")
            {
                ChangeProductionInfo(choosenStore);
            }
            else if (input == "7")
            {
                if (choosenStore.Category == null || choosenStore.Category.Prodaction == null)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Janitor();
                    Line();
                    Console.WriteLine("You must Configure Production First!");
                }
                else
                {
                    AddScedule(choosenStore);
                } 
            }
            else if (input == "8")
            {
                if (choosenStore?.Category == null || choosenStore.Category.Schedule == null)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Janitor();
                    Line();
                    Console.WriteLine("You must Configure Production Schedule First!");
                }
                else
                {
                    ChangeSchedule(choosenStore.Category.Schedule);
                }
            }
            else if (input == "9")
            {
                DeleteInfo(choosenStore);
            }
            else if (input == "10")
            {
                Console.WriteLine();
                Console.WriteLine("Logging off...");
                Environment.Exit(0);
            }
        }
    }
    static public void ChangeStore(Store store)
    {
        WoltColor();
        Janitor();
        Line();
        Console.WriteLine("Choose what to change:");
        Console.WriteLine();
        Console.WriteLine("1) Change Store name");
        Console.WriteLine("2) Change Store City");
        Console.WriteLine("3) Change Store Address");
        Console.WriteLine("4) Change Store Phone Number");
        Console.WriteLine("5) Change Store Order Fee");
        Console.WriteLine("6) Change Store Order Time");
        
        string input = Console.ReadLine();
        Choices newChoice = new Choices(){ Choice = input };
        var validator = new ChoicesValidator(6);
        var result = validator.Validate(newChoice);

        if (!result.IsValid)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Janitor();
            Line();
            foreach (var error in result.Errors)
            {
                Console.WriteLine($"ERROR: Wrong {error.PropertyName} - {error.ErrorMessage}");
            }
        }
        else if (input == "1")
        {
            Janitor();
            Line();
            Console.WriteLine($"Current Store Name: {store.Name}");
            Console.WriteLine();
            Console.WriteLine("Enter new Store Name:");
            string newName = Console.ReadLine();
            Console.WriteLine("Please wait...");
            store.Name = newName;
            var storeValidator = new StoreValidator();
            var storeResult = storeValidator.Validate(store);
            
            if (!storeResult.IsValid)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Janitor();
                Line();
                foreach (var error in storeResult.Errors)
                {
                    Console.WriteLine($"ERROR: Wrong {error.PropertyName} - {error.ErrorMessage}");
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Janitor();
                Line();
                Console.WriteLine("Saving changes...");
                _context.SaveChanges();
                Console.WriteLine($"Store name updated to: {store.Name}");
            }
        }
        else if (input == "2")
        {
            Janitor();
            Line();
            Console.WriteLine($"Current Store City: {store.City}");
            Console.WriteLine();
            Console.WriteLine("Enter new Store City:");
            string newCity = Console.ReadLine();
            Console.WriteLine("Please wait...");
            store.City = newCity;
            var storeValidator = new StoreValidator();
            var storeResult = storeValidator.Validate(store);
            
            if (!storeResult.IsValid)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Janitor();
                Line();
                foreach (var error in storeResult.Errors)
                {
                    Console.WriteLine($"ERROR: Wrong {error.PropertyName} - {error.ErrorMessage}");
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Janitor();
                Line();
                Console.WriteLine("Saving changes...");
                _context.SaveChanges();
                Console.WriteLine($"Store city updated to: {store.City}");
            }
        }
        else if (input == "3")
        {
            Janitor();
            Line();
            Console.WriteLine($"Current Store Address: {store.Address}");
            Console.WriteLine();
            Console.WriteLine("Enter new Store Address:");
            string newAddress = Console.ReadLine();
            Console.WriteLine("Please wait...");
            store.Address = newAddress;
            var storeValidator = new StoreValidator();
            var storeResult = storeValidator.Validate(store);
            
            if (!storeResult.IsValid)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Janitor();
                Line();
                foreach (var error in storeResult.Errors)
                {
                    Console.WriteLine($"ERROR: Wrong {error.PropertyName} - {error.ErrorMessage}");
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Janitor();
                Line();
                Console.WriteLine("Saving changes...");
                _context.SaveChanges();
                Console.WriteLine($"Store address updated to: {store.Address}");
            }
        }
        else if (input == "4")
        {
            Janitor();
            Line();
            Console.WriteLine($"Current Store Phone Number: {store.Phone}");
            Console.WriteLine();
            Console.WriteLine("Enter new Store Phone Number:");
            string newPhone = Console.ReadLine();
            Console.WriteLine("Please wait...");
            store.Phone = newPhone;
            var storeValidator = new StoreValidator();
            var storeResult = storeValidator.Validate(store);
            
            if (!storeResult.IsValid)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Janitor();
                Line();
                foreach (var error in storeResult.Errors)
                {
                    Console.WriteLine($"ERROR: Wrong {error.PropertyName} - {error.ErrorMessage}");
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Janitor();
                Line();
                Console.WriteLine("Saving changes...");
                _context.SaveChanges();
                Console.WriteLine($"Store phone numeber updated to: {store.Phone}");
            }
        }
        else if (input == "5")
        {
            Janitor();
            Line();
            Console.WriteLine($"Current Store Order Fee: {store.OrderFee}");
            Console.WriteLine();
            Console.WriteLine("Enter new Store Order Fee:");
            decimal newOrderFee = decimal.Parse(Console.ReadLine());
            Console.WriteLine("Please wait...");
            store.OrderFee = newOrderFee;
            var storeValidator = new StoreValidator();
            var storeResult = storeValidator.Validate(store);
            
            if (!storeResult.IsValid)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Janitor();
                Line();
                foreach (var error in storeResult.Errors)
                {
                    Console.WriteLine($"ERROR: Wrong {error.PropertyName} - {error.ErrorMessage}");
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Janitor();
                Line();
                Console.WriteLine("Saving changes...");
                _context.SaveChanges();
                Console.WriteLine($"Store order fee updated to: {store.OrderFee}");
            }
        }
        else if (input == "6")
        {
            Janitor();
            Line();
            Console.WriteLine($"Current Store Order Time: {store.OrderTime}");
            Console.WriteLine();
            Console.WriteLine("Enter new Store Order Time:");
            TimeSpan newOrderTime = TimeSpan.Parse(Console.ReadLine());
            Console.WriteLine("Please wait...");
            store.OrderTime = newOrderTime;
            var storeValidator = new StoreValidator();
            var storeResult = storeValidator.Validate(store);
            
            if (!storeResult.IsValid)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Janitor();
                Line();
                foreach (var error in storeResult.Errors)
                {
                    Console.WriteLine($"ERROR: Wrong {error.PropertyName} - {error.ErrorMessage}");
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Janitor();
                Line();
                Console.WriteLine("Saving changes...");
                _context.SaveChanges();
                Console.WriteLine($"Store Order Time updated to: {store.OrderTime}");
            }
        }
    }
    static public void ProductionConfiguration(Store store)
    {
        WoltColor();
        Janitor();
        Line();
        Console.WriteLine($"Welcome to {store.Name} Configuration!");
        Console.WriteLine();
        
        Console.WriteLine("Enter Store Category Type (Technics, Groceries etc):");
        string newCategoryName = Console.ReadLine();
        Console.WriteLine();

        StoreCategory newCategory = new StoreCategory()
        {
            Name = newCategoryName,
            StoreId = store.Id
        };
        var categoryValidator = new StoreCategoryValidator();
        var categoryResult = categoryValidator.Validate(newCategory);
        if (!categoryResult.IsValid)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Janitor();
            Line();
            foreach (var error in categoryResult.Errors)
            {
                Console.WriteLine($"ERROR: Wrong {error.PropertyName} - {error.ErrorMessage}");
            }
        }
        else
        {
            _context.StoreCategories.Add(newCategory);
            _context.SaveChanges();

            Console.WriteLine("Enter Production Description:");
            string newProductionDescription = Console.ReadLine();
            Console.WriteLine();

            Prodaction newProdaction = new Prodaction()
            {
                Description = newProductionDescription,
                StoreCategoryId = store.Category.Id
            };
        
            var productionValidator = new ProdactionValidator();
            var productionResult = productionValidator.Validate(newProdaction);

            if (!productionResult.IsValid)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Janitor();
                Line();
                foreach (var error in productionResult.Errors)
                {
                    Console.WriteLine($"ERROR: Wrong {error.PropertyName} - {error.ErrorMessage}");
                }
            }
            else
            {
                _context.Prodactions.Add(newProdaction);
                _context.SaveChanges();

                Console.WriteLine("Production Configuration Ended!");
            }
        
            
        }
        

    }
    static public void AddProductionCategory(Prodaction prodaction)
    {
        WoltColor();
        Janitor();
        Line();
        Console.WriteLine("Enter Production Category (Phones, Fruits, etc):");
        string newName = Console.ReadLine();
        Console.WriteLine();

        Console.WriteLine("Enter Production Category Description:");
        string newDescription = Console.ReadLine();
        Console.WriteLine();

        Console.WriteLine("Please wait...");

        ProdCategory newCategoty = new ProdCategory()
        {
            Name = newName,
            Description = newDescription,
            ProdactionId = prodaction.Id
        };
        
        var prodactionCategoryValidator = new ProdCategoryValidator();
        var prodactionCategoryResult = prodactionCategoryValidator.Validate(newCategoty);

        if (!prodactionCategoryResult.IsValid)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Janitor();
            Line();
            foreach (var error in prodactionCategoryResult.Errors)
            {
                Console.WriteLine($"ERROR: Wrong {error.PropertyName} - {error.ErrorMessage}");
            }
        }
        else
        {
            Janitor();
            Line();
            Console.WriteLine("Saving Category...");
            _context.ProdCategories.Add(newCategoty);
            _context.SaveChanges();
            Console.WriteLine($"Category {newCategoty.Name} Added!");
        }
        
        
    }
    static public void AddProducts(Prodaction prodaction)
    {
        WoltColor();
        Janitor();
        Line();
        Console.WriteLine("Choose Production Category ID:");
        Console.WriteLine();
        var allProdCategory = prodaction.Categories.ToList();

        foreach (var category in allProdCategory)
        {
            Console.WriteLine($"{category.Name} - ({category.Id})");
        }
        
        int choosenCategoryId = int.Parse(Console.ReadLine());
        var choosenCategory = _context.ProdCategories
                                .FirstOrDefault(x => x.Id == choosenCategoryId);

        if (choosenCategory == null)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Janitor();
            Line();
            Console.WriteLine("Choose Correct Product Category ID!");
        }
        else
        {
            Console.WriteLine("Enter Product Name:");
            string newProductName = Console.ReadLine();
            Console.WriteLine();

            Console.WriteLine("Enter Product Price:");
            decimal newProductPrice = decimal.Parse(Console.ReadLine());
            Console.WriteLine();

            Console.WriteLine("Enter Product Quantity:");
            int newProductQuantity = int.Parse(Console.ReadLine());
            Console.WriteLine();

            Console.WriteLine("Please Wait...");
            Console.WriteLine();
            
            Product newProduct = new Product
            {
                Name = newProductName,
                Price = newProductPrice,
                Quantity = newProductQuantity,
                IsAvailable = true,
                ProdCategoryId = choosenCategoryId
            };
            
            var productValidator = new ProductValidator();
            var productResult = productValidator.Validate(newProduct);

            if (!productResult.IsValid)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Janitor();
                Line();
                foreach (var error in productResult.Errors)
                {
                    Console.WriteLine($"ERROR: Wrong {error.PropertyName} - {error.ErrorMessage}");
                }
            }
            else
            {
                _context.Products.Add(newProduct);
                _context.SaveChanges();
                Console.WriteLine("Product Added!");
            }
            
            
        }
        
        
    }
    static public void SeeAllProdaction(Store store)
    {
        WoltColor();
        Janitor();
        Line();
        Console.WriteLine($"============= {store.Name} ============");
        Console.WriteLine();
        Console.WriteLine($"Store Category: {store.Category.Name}");
        Console.WriteLine($"Product: {store.Category.Prodaction.Description}");
        Console.WriteLine();
        if (store.Category.Schedule == null)
        {
            Console.WriteLine("Schedule is not configured!");
        }
        else
        {
            ScheduleWrite(store.Category.Schedule);
        }
        
        Console.WriteLine();
        Console.WriteLine("==================== Prodaction ==================");
        var allProdactionCategories = store.Category.Prodaction.Categories.ToList();

        foreach (var category in allProdactionCategories)
        {
            Line();
            Console.WriteLine($"[Category] - ({category.Name}) - {category.Description}");
            Console.WriteLine("Products:");
            Console.WriteLine();
            foreach (var product in category.Products)
            {
                Console.WriteLine("******************************************");
                Console.WriteLine($"[Product] - {product.Name} - {product.Price :C} -> [{product.Id}]");
                Console.WriteLine($"{product.Quantity} In stock");
                if (product.IsAvailable == true)
                {
                    Console.WriteLine("Product Available");
                }
                else if (product.IsAvailable == false)
                {
                    Console.WriteLine("Product Not Available!");
                }
            }
        }
    }
    static public void ChangeProductionInfo(Store store)
    {
        WoltColor();
        Janitor();
        Line();
        Console.WriteLine("Choose What to change:");
        Console.WriteLine("1) Change Store Category Name");
        Console.WriteLine("2) Change Prodaction Description");
        Console.WriteLine("3) Change Prodaction Category");
        Console.WriteLine("4) Change Product Info");
        
        string input = Console.ReadLine();
        Choices inputChoices = new Choices(){ Choice = input};
        var choiceValidator = new ChoicesValidator(4);
        var result = choiceValidator.Validate(inputChoices);
            
        if (result.IsValid == false)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Janitor();
            Line();
            foreach (var error in result.Errors)
            {
                Console.WriteLine($"ERROR: Wrong {error.PropertyName} - {error.ErrorMessage}");
            }
        }
        
        else if (input == "1")
        {
            Janitor();
            Line();
            Console.WriteLine($"Actual Store Category: {store.Category.Name}");
            Console.WriteLine("Enter new Store Category Name:");
            string newStoreCategoryName = Console.ReadLine();
            
            store.Category.Name = newStoreCategoryName;
            var categoryValidator = new StoreCategoryValidator();
            var categoryResult = categoryValidator.Validate(store.Category);
            
            if (categoryResult.IsValid == false)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Janitor();
                Line();
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"ERROR: Wrong {error.PropertyName} - {error.ErrorMessage}");
                }
            }

            else
            {
                Console.WriteLine("Saving Changes...");
                _context.SaveChanges();
                Console.WriteLine($"New Store Category: {store.Category.Name}");
            }

            
        }
        else if (input == "2")
        {
            Janitor();
            Line();
            Console.WriteLine($"Actual Prodaction Description: {store.Category.Prodaction.Description}");
            Console.WriteLine("Enter new Prodaction Description:");
            string newProdactionDesc = Console.ReadLine();
            
            store.Category.Prodaction.Description = newProdactionDesc;
            var prodactionValidator = new ProdactionValidator();
            var prodactionResult = prodactionValidator.Validate(store.Category.Prodaction);
            
            if (prodactionResult.IsValid == false)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Janitor();
                Line();
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"ERROR: Wrong {error.PropertyName} - {error.ErrorMessage}");
                }
            }
            else
            {
                Console.WriteLine("Saving Changes...");
                _context.SaveChanges();
                Console.WriteLine($"New Prodaction Description: {store.Category.Prodaction.Description}");
            }

            
        }
        else if (input == "3")
        {
            Janitor();
            Line();
            Console.WriteLine("Choose Prodaction Category ID:");
            Console.WriteLine();
            var allCategory = store.Category.Prodaction.Categories.ToList();
            foreach (var category in allCategory)
            {
                Console.WriteLine($"{category.Name} - ({category.Id})");
            }

            Console.WriteLine("Enter Prodaction Category ID:");
            int newProdactionCategoryId = int.Parse(Console.ReadLine());
            var choosenCategory = allCategory.FirstOrDefault(c => c.Id == newProdactionCategoryId);

            if (choosenCategory == null)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Janitor();
                Line();
                Console.WriteLine("Select correct a Category ID!");
            }
            else
            {
                Console.WriteLine("Enter new Prodaction Category Name:");
                string newProdactionCategoryName = Console.ReadLine();
                Console.WriteLine();

                Console.WriteLine("Enter new Prodaction Category Description:");
                string newProdactionCategoryDesc = Console.ReadLine();
                Console.WriteLine();
            
                choosenCategory.Name = newProdactionCategoryName;
                choosenCategory.Description = newProdactionCategoryDesc;
            
                var prodactionCategoryValidator = new ProdCategoryValidator();
                var prodactionCategoryResult =  prodactionCategoryValidator.Validate(choosenCategory);
            
                if (prodactionCategoryResult.IsValid == false)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Janitor();
                    Line();
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"ERROR: Wrong {error.PropertyName} - {error.ErrorMessage}");
                    }
                }

                else
                {
                    Console.WriteLine("Saving Changes...");
                    _context.SaveChanges();
                    Console.WriteLine($"New Prodaction Category Name: {choosenCategory.Name},");
                    Console.WriteLine($"New Prodaction Category Description: {choosenCategory.Description},");
                }
            }
            

        }
        else if (input == "4")
        {
            Janitor();
            Line();
            Console.WriteLine("Choose Prodaction Category ID:");
            Console.WriteLine();
            var allCategory = store.Category.Prodaction.Categories.ToList();
            foreach (var category in allCategory)
            {
                Console.WriteLine($"{category.Name} - ({category.Id})");
            }

            Console.WriteLine("Enter Prodaction Category ID:");
            int newProdactionCategoryId = int.Parse(Console.ReadLine());
            var choosenCategory = allCategory.FirstOrDefault(c => c.Id == newProdactionCategoryId);

            if (choosenCategory == null)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Janitor();
                Line();
                Console.WriteLine("Select correct a Category ID!");
            }
            else
            {
                Janitor();
                Line();
                Console.WriteLine("Choose Product Id");
                var allProduct = choosenCategory.Products.ToList();

                foreach (var product in allProduct)
                {
                    Console.WriteLine($"{product.Name} - ({product.Id})");
                }

                Console.WriteLine("Enter Product Id:");
                int newProductId = int.Parse(Console.ReadLine());
                var choosenProduct = allProduct.FirstOrDefault(d => d.Id == newProductId);

                if (choosenProduct == null)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Janitor();
                    Line();
                    Console.WriteLine("Select correct a Product ID!");
                }
                else
                {
                    Janitor();
                    Line();
            
                    Console.WriteLine("Enter new Product Name:");
                    string newProductName = Console.ReadLine();
                    Console.WriteLine();

                    Console.WriteLine("Enter new Product Price:");
                    decimal newProductPrice = decimal.Parse(Console.ReadLine());
                    Console.WriteLine();

                    Console.WriteLine("Enter new Product Quantity:");
                    int newProductQuantity = int.Parse(Console.ReadLine());
                    Console.WriteLine();
                    
                    choosenProduct.Name = newProductName;
                    choosenProduct.Price = newProductPrice;
                    choosenProduct.Quantity = newProductQuantity;
            
                    var productValidator = new ProductValidator();
                    var productResult =  productValidator.Validate(choosenProduct);
            
                    if (productResult.IsValid == false)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Janitor();
                        Line();
                        foreach (var error in result.Errors)
                        {
                            Console.WriteLine($"ERROR: Wrong {error.PropertyName} - {error.ErrorMessage}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Saving Changes...");
                        _context.SaveChanges();
                        Console.WriteLine($"New Product Name: {choosenProduct.Name},");
                        Console.WriteLine($"New Product Price: {choosenProduct.Price}");
                        Console.WriteLine($"New Product Quantity: {choosenProduct.Quantity}");
                    }
                    
                }
                
            }

            
        }
    }
    static public void AddScedule(Store store)
    {
        WoltColor();
        Janitor();
        Line();
        Console.WriteLine($"Configure {store.Name} Scedule...");
        Console.WriteLine();
        Console.WriteLine("Is Monday open? y/n");
        string inputModay = Console.ReadLine();
        Console.WriteLine();

        Console.WriteLine("Is Tuesday open? y/n");
        string inputTuesday = Console.ReadLine();
        Console.WriteLine();
        
        Console.WriteLine("Is Wednesday open? y/n");
        string inputWednesday = Console.ReadLine();
        Console.WriteLine();
        
        Console.WriteLine("Is Thursday open? y/n");
        string inputThursday = Console.ReadLine();
        Console.WriteLine();
        
        Console.WriteLine("Is Friday open? y/n");
        string inputFriday = Console.ReadLine();
        Console.WriteLine();
        
        Console.WriteLine("Is Saturday open? y/n");
        string inputSaturday = Console.ReadLine();
        Console.WriteLine();
        
        Console.WriteLine("Is Sunday open? y/n");
        string inputSunday = Console.ReadLine();
        Console.WriteLine();
        Console.WriteLine("Please Wait...");

        bool IsMondayOpen = false;
        bool IsTuesdayOpen = false;
        bool IsWednesdayOpen = false;
        bool IsThursdayOpen = false;
        bool IsFridayOpen = false;
        bool IsSaturdayOpen = false;
        bool IsSundayOpen = false;
        
        bool IsMondayAllDay = false;
        bool IsTuesdayAllDay = false;
        bool IsWednesdayAllDay = false;
        bool IsThursdayAllDay = false;
        bool IsFridayAllDay = false;
        bool IsSaturdayAllDay = false;
        bool IsSundayAllDay = false;

        TimeSpan mondayOpenTime = new TimeSpan(0, 0, 0);
        TimeSpan tuesdayOpenTime = new TimeSpan(0, 0, 0);
        TimeSpan wednesdayOpenTime = new TimeSpan(0, 0, 0);
        TimeSpan thursdayOpenTime = new TimeSpan(0, 0, 0);;
        TimeSpan fridayOpenTime = new TimeSpan(0, 0, 0);;
        TimeSpan saturdayOpenTime = new TimeSpan(0, 0, 0);;
        TimeSpan sundayOpenTime = new TimeSpan(0, 0, 0);;
        
        TimeSpan mondayCloseTime = new TimeSpan(0, 0, 0);;
        TimeSpan tuesdayCloseTime = new TimeSpan(0, 0, 0);;
        TimeSpan wednesdayCloseTime = new TimeSpan(0, 0, 0);;
        TimeSpan thursdayCloseTime = new TimeSpan(0, 0, 0);;
        TimeSpan fridayCloseTime = new TimeSpan(0, 0, 0);;
        TimeSpan saturdayCloseTime = new TimeSpan(0, 0, 0);;
        TimeSpan sundayCloseTime = new TimeSpan(0, 0, 0);;
        Janitor();
        Line();
        if (inputModay.ToLower() == "y")
        {
            IsMondayOpen = true;
            Console.WriteLine("Is monday open for all day? y/n");
            string isAllday = Console.ReadLine();

            if (isAllday.ToLower() == "y")
            {
                IsMondayAllDay = true;
                mondayOpenTime = new TimeSpan(00, 00, 01);
                mondayCloseTime = new TimeSpan(23, 59, 59);
                Console.WriteLine("Monday Schedule saved!");
            }
            else if (isAllday.ToLower() == "n")
            {
                IsMondayAllDay = false;
                Console.WriteLine("Enter Monday Open Time:");
                mondayOpenTime = TimeSpan.Parse(Console.ReadLine());
                Console.WriteLine();
                Console.WriteLine("Enter Monday Close Time:");
                mondayCloseTime = TimeSpan.Parse(Console.ReadLine());
                Console.WriteLine();
                Console.WriteLine("Monday Schedule saved!");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine();
                Console.WriteLine("Incorrect Input!");
            }
        }
        else if (inputModay.ToLower() == "n")
        {
            IsMondayOpen = false;
            IsMondayAllDay = false;
            mondayOpenTime = new TimeSpan(00, 00, 01);
            mondayCloseTime = new TimeSpan(00, 00, 02);
            Console.WriteLine("Monday Schedule saved!");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine();
            Console.WriteLine("Incorrect Monday Input!");
        }
        
        if (inputTuesday.ToLower() == "y")
        {
            IsTuesdayOpen = true;
            Console.WriteLine("Is tuesday open for all day? y/n");
            string isAllday = Console.ReadLine();

            if (isAllday.ToLower() == "y")
            {
                IsTuesdayAllDay = true;
                tuesdayOpenTime = new TimeSpan(00, 00, 01);
                tuesdayCloseTime = new TimeSpan(23, 59, 59);
                Console.WriteLine("Tuesday Schedule saved!");
            }
            else if (isAllday.ToLower() == "n")
            {
                IsTuesdayAllDay = false;
                Console.WriteLine("Enter Tuesday Open Time:");
                tuesdayOpenTime = TimeSpan.Parse(Console.ReadLine());
                Console.WriteLine();
                Console.WriteLine("Enter Tuesday Close Time:");
                tuesdayCloseTime = TimeSpan.Parse(Console.ReadLine());
                Console.WriteLine();
                Console.WriteLine("Tuesday Schedule saved!");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine();
                Console.WriteLine("Incorrect Input!");
            }
        }
        else if (inputTuesday.ToLower() == "n")
        {
            IsTuesdayOpen = false;
            IsTuesdayAllDay = false;
            tuesdayOpenTime = new TimeSpan(00, 00, 01);
            tuesdayCloseTime = new TimeSpan(00, 00, 02);
            Console.WriteLine("Tuesday Schedule saved!");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine();
            Console.WriteLine("Incorrect Tuesday Input!");
        }
        
        if (inputWednesday.ToLower() == "y")
        {
            IsWednesdayOpen = true;
            Console.WriteLine("Is wednesday open for all day? y/n");
            string isAllday = Console.ReadLine();

            if (isAllday.ToLower() == "y")
            {
                IsWednesdayAllDay = true;
                wednesdayOpenTime = new TimeSpan(00, 00, 01);
                wednesdayCloseTime = new TimeSpan(23, 59, 59);
                Console.WriteLine("Wednesday Schedule saved!");
            }
            else if (isAllday.ToLower() == "n")
            {
                IsWednesdayAllDay = false;
                Console.WriteLine("Enter Wednesday Open Time:");
                wednesdayOpenTime = TimeSpan.Parse(Console.ReadLine());
                Console.WriteLine();
                Console.WriteLine("Enter Wednesday Close Time:");
                wednesdayCloseTime = TimeSpan.Parse(Console.ReadLine());
                Console.WriteLine();
                Console.WriteLine("Wednesday Schedule saved!");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine();
                Console.WriteLine("Incorrect Input!");
            }
        }
        else if (inputWednesday.ToLower() == "n")
        {
            IsWednesdayOpen = false;
            IsWednesdayAllDay = false;
            wednesdayOpenTime = new TimeSpan(00, 00, 01);
            wednesdayCloseTime = new TimeSpan(00, 00, 02);
            Console.WriteLine("Wednesday Schedule saved!");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine();
            Console.WriteLine("Incorrect Wednesday Input!");
        }
        
        if (inputThursday.ToLower() == "y")
        {
            IsThursdayOpen = true;
            Console.WriteLine("Is thursday open for all day? y/n");
            string isAllday = Console.ReadLine();

            if (isAllday.ToLower() == "y")
            {
                IsThursdayAllDay = true;
                thursdayOpenTime = new TimeSpan(00, 00, 01);
                thursdayCloseTime = new TimeSpan(23, 59, 59);
                Console.WriteLine("Thursday Schedule saved!");
            }
            else if (isAllday.ToLower() == "n")
            {
                IsThursdayAllDay = false;
                Console.WriteLine("Enter Thursday Open Time:");
                thursdayOpenTime = TimeSpan.Parse(Console.ReadLine());
                Console.WriteLine();
                Console.WriteLine("Enter Thursday Close Time:");
                thursdayCloseTime = TimeSpan.Parse(Console.ReadLine());
                Console.WriteLine();
                Console.WriteLine("Thursday Schedule saved!");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine();
                Console.WriteLine("Incorrect Input!");
            }
        }
        else if (inputThursday.ToLower() == "n")
        {
            IsThursdayOpen = false;
            IsThursdayAllDay = false;
            thursdayOpenTime = new TimeSpan(00, 00, 01);
            thursdayCloseTime = new TimeSpan(00, 00, 02);
            Console.WriteLine("Thursday Schedule saved!");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine();
            Console.WriteLine("Incorrect Thursday Input!");
        }
        
        if (inputFriday.ToLower() == "y")
        {
            IsFridayOpen = true;
            Console.WriteLine("Is friday open for all day? y/n");
            string isAllday = Console.ReadLine();

            if (isAllday.ToLower() == "y")
            {
                IsFridayAllDay = true;
                fridayOpenTime = new TimeSpan(00, 00, 01);
                fridayCloseTime = new TimeSpan(23, 59, 59);
                Console.WriteLine("Friday Schedule saved!");
            }
            else if (isAllday.ToLower() == "n")
            {
                IsFridayAllDay = false;
                Console.WriteLine("Enter Friday Open Time:");
                fridayOpenTime = TimeSpan.Parse(Console.ReadLine());
                Console.WriteLine();
                Console.WriteLine("Enter Friday Close Time:");
                fridayCloseTime = TimeSpan.Parse(Console.ReadLine());
                Console.WriteLine();
                Console.WriteLine("Friday Schedule saved!");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine();
                Console.WriteLine("Incorrect Input!");
            }
        }
        else if (inputFriday.ToLower() == "n")
        {
            IsFridayOpen = false;
            IsFridayAllDay = false;
            fridayOpenTime = new TimeSpan(00, 00, 01);
            fridayCloseTime = new TimeSpan(00, 00, 02);
            Console.WriteLine("Friday Schedule saved!");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine();
            Console.WriteLine("Incorrect Friday Input!");
        }
        
        if (inputSaturday.ToLower() == "y")
        {
            IsSaturdayOpen = true;
            Console.WriteLine("Is saturday open for all day? y/n");
            string isAllday = Console.ReadLine();

            if (isAllday.ToLower() == "y")
            {
                IsSaturdayAllDay = true;
                saturdayOpenTime = new TimeSpan(00, 00, 01);
                saturdayCloseTime = new TimeSpan(23, 59, 59);
                Console.WriteLine("Saturday Schedule saved!");
            }
            else if (isAllday.ToLower() == "n")
            {
                IsSaturdayAllDay = false;
                Console.WriteLine("Enter Saturday Open Time:");
                saturdayOpenTime = TimeSpan.Parse(Console.ReadLine());
                Console.WriteLine();
                Console.WriteLine("Enter Saturday Close Time:");
                saturdayCloseTime = TimeSpan.Parse(Console.ReadLine());
                Console.WriteLine();
                Console.WriteLine("Saturday Schedule saved!");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine();
                Console.WriteLine("Incorrect Input!");
            }
        }
        else if (inputSaturday.ToLower() == "n")
        {
            IsSaturdayOpen = false;
            IsSaturdayAllDay = false;
            saturdayOpenTime = new TimeSpan(00, 00, 01);
            saturdayCloseTime = new TimeSpan(00, 00, 02);
            Console.WriteLine("Saturday Schedule saved!");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine();
            Console.WriteLine("Incorrect Saturday Input!");
        }
        
        if (inputSunday.ToLower() == "y")
        {
            IsSundayOpen = true;
            Console.WriteLine("Is sunday open for all day? y/n");
            string isAllday = Console.ReadLine();

            if (isAllday.ToLower() == "y")
            {
                IsSundayAllDay = true;
                sundayOpenTime = new TimeSpan(00, 00, 01);
                sundayCloseTime = new TimeSpan(23, 59, 59);
                Console.WriteLine("Sunday Schedule saved!");
            }
            else if (isAllday.ToLower() == "n")
            {
                IsSundayAllDay = false;
                Console.WriteLine("Enter Sunday Open Time:");
                sundayOpenTime = TimeSpan.Parse(Console.ReadLine());
                Console.WriteLine();
                Console.WriteLine("Enter Sunday Close Time:");
                sundayCloseTime = TimeSpan.Parse(Console.ReadLine());
                Console.WriteLine();
                Console.WriteLine("Sunday Schedule saved!");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine();
                Console.WriteLine("Incorrect Input!");
            }
        }
        else if (inputSunday.ToLower() == "n")
        {
            IsSundayOpen = false;
            IsSundayAllDay = false;
            sundayOpenTime = new TimeSpan(00, 00, 01);
            sundayCloseTime = new TimeSpan(00, 00, 02);
            Console.WriteLine("Sunday Schedule saved!");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine();
            Console.WriteLine("Incorrect Sunday Input!");
        }

        Console.WriteLine("Please Wait...");

        StoreSchedule newSchedule = new StoreSchedule
        {
            isMondayOpen = IsMondayOpen,
            isTuesdayOpen = IsTuesdayOpen,
            isWednesdayOpen = IsWednesdayOpen,
            isThursdayOpen = IsThursdayOpen,
            isFridayOpen = IsFridayOpen,
            isSaturdayOpen = IsSaturdayOpen,
            isSundayOpen = IsSundayOpen,
            
            isMondayAllDay = IsMondayAllDay,
            isTuesdayAllDay = IsTuesdayAllDay,
            isWednesdayAllDay = IsWednesdayAllDay,
            isThursdayAllDay = IsThursdayAllDay,
            isFridayAllDay = IsFridayAllDay,
            isSaturdayAllDay = IsSaturdayAllDay,
            isSundayAllDay = IsSundayAllDay,
            
            MondayOpenTime = mondayOpenTime,
            TuesdayOpenTime = tuesdayOpenTime,
            WednesdayOpenTime = wednesdayOpenTime,
            ThursdayOpenTime = thursdayOpenTime,
            FridayOpenTime = fridayOpenTime,
            SaturdayOpenTime = saturdayOpenTime,
            SundayOpenTime = sundayOpenTime,
            
            MondayCloseTime = mondayCloseTime,
            TuesdayCloseTime = tuesdayCloseTime,
            WednesdayCloseTime = wednesdayCloseTime,
            ThursdayCloseTime = thursdayCloseTime,
            FridayCloseTime = fridayCloseTime,
            SaturdayCloseTime = saturdayCloseTime,
            SundayCloseTime = sundayCloseTime,
            
            StoreCategoryId = store.Category.Id,
        };
        
        var scheduleValidator = new StoreScheduleValidator();
        var result = scheduleValidator.Validate(newSchedule);

        if (!result.IsValid)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Janitor();
            Line();
            foreach (var error in result.Errors)
            {
                Console.WriteLine($"ERROR: Wrong {error.PropertyName} - {error.ErrorMessage}");
            }
        }
        else
        {
            Janitor();

            Console.WriteLine("Saving Schedule...");
        
            _context.StoreSchedules.Add(newSchedule);
            _context.SaveChanges();
            Janitor();
            Line();
            Console.WriteLine("Schedule saved!");
        }

        
    }
    static public void ScheduleWrite(StoreSchedule schedule)
    {
        WoltColor();
        Line();
        Console.WriteLine();
        if (schedule.isMondayOpen == false)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Monday: Closed");
            WoltColor();
        }
        else if (schedule.isMondayAllDay == true)
        {
            Console.WriteLine("Monday: Open All Day");
        }
        else
        {
            Console.WriteLine($"Monday: {schedule.MondayOpenTime} - {schedule.MondayCloseTime}");
        }

        Console.WriteLine();
        if (schedule.isTuesdayOpen == false)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Tuesday: Closed");
            WoltColor();
        }
        else if (schedule.isTuesdayAllDay == true)
        {
            Console.WriteLine("Tuesday: Open All Day");
        }
        else
        {
            Console.WriteLine($"Tuesday: {schedule.TuesdayOpenTime} - {schedule.TuesdayCloseTime}");
        }

        Console.WriteLine();
        if (schedule.isWednesdayOpen == false)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Wednesday: Closed");
            WoltColor();
        }
        else if (schedule.isWednesdayAllDay == true)
        {
            Console.WriteLine("Wednesday: Open All Day");
        }
        else
        {
            Console.WriteLine($"Wednesday: {schedule.WednesdayOpenTime} - {schedule.WednesdayCloseTime}");
        }

        Console.WriteLine();
        if (schedule.isThursdayOpen == false)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Thursday: Closed");
            WoltColor();
        }
        else if (schedule.isThursdayAllDay == true)
        {
            Console.WriteLine("Thursday: Open All Day");
        }
        else
        {
            Console.WriteLine($"Thursday: {schedule.ThursdayOpenTime} - {schedule.ThursdayCloseTime}");
        }

        Console.WriteLine();
        if (schedule.isFridayOpen == false)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Friday: Closed");
            WoltColor();
        }
        else if (schedule.isFridayAllDay == true)
        {
            Console.WriteLine("Friday: Open All Day");
        }
        else
        {
            Console.WriteLine($"Friday: {schedule.FridayOpenTime} - {schedule.FridayCloseTime}");
        }

        Console.WriteLine();
        if (schedule.isSaturdayOpen == false)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Saturday: Closed");
            WoltColor();
        }
        else if (schedule.isSaturdayAllDay == true)
        {
            Console.WriteLine("Saturday: Open All Day");
        }
        else
        {
            Console.WriteLine($"Saturday: {schedule.SaturdayOpenTime} - {schedule.SaturdayCloseTime}");
        }

        Console.WriteLine();
        if (schedule.isSundayOpen == false)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Sunday: Closed");
            WoltColor();
        }
        else if (schedule.isSundayAllDay == true)
        {
            Console.WriteLine("Sunday: Open All Day");
        }
        else
        {
            Console.WriteLine($"Sunday: {schedule.SundayOpenTime} - {schedule.SundayCloseTime}");
        }

        Console.WriteLine();
        Line();
    }
    static public void ChangeSchedule(StoreSchedule schedule)
    {
        WoltColor();
        Janitor();
        Line();
        Console.WriteLine("Change Schedule For:");
        Console.WriteLine("1) Monday");
        Console.WriteLine("2) Tuesday");
        Console.WriteLine("3) Wednesday");
        Console.WriteLine("4) Thursday");
        Console.WriteLine("5) Friday");
        Console.WriteLine("6) Saturday");
        Console.WriteLine("7) Sunday");
        
        string input = Console.ReadLine();
        Choices inputChoice = new Choices() {Choice = input};

        var validator = new ChoicesValidator(7);
        var result = validator.Validate(inputChoice);
        
        if (result.IsValid == false)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Janitor();
            Line();
            foreach (var error in result.Errors)
            {
                Console.WriteLine($"ERROR: Wrong {error.PropertyName} - {error.ErrorMessage}");
            }
        }
        else if(input == "1")
        {
            Janitor();
            Line();
            Console.WriteLine("Is Store Open in Monday? y/n");
            string inputModay = Console.ReadLine();
            
            if (inputModay.ToLower() == "y")
            {
                schedule.isMondayOpen = true;
                Console.WriteLine("Is monday open for all day? y/n");
                string isAllday = Console.ReadLine();

                if (isAllday.ToLower() == "y")
                {
                    schedule.isMondayOpen = true;
                    schedule.MondayOpenTime = new TimeSpan(00, 00, 01);
                    schedule.MondayCloseTime = new TimeSpan(23, 59, 59);
                    _context.SaveChanges();
                    Console.WriteLine("Monday Schedule saved!");
                }
                else if (isAllday.ToLower() == "n")
                {
                    schedule.isMondayAllDay = false;
                    Console.WriteLine("Enter Monday Open Time:");
                    schedule.MondayOpenTime = TimeSpan.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.WriteLine("Enter Monday Close Time:");
                    schedule.MondayCloseTime = TimeSpan.Parse(Console.ReadLine());
                    Console.WriteLine();
                    _context.SaveChanges();
                    Console.WriteLine("Monday Schedule saved!");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine();
                    Console.WriteLine("Incorrect Input!");
                }
            }
            else if (inputModay.ToLower() == "n")
            {
                schedule.isMondayOpen = false;
                schedule.isMondayAllDay = false;
                schedule.MondayOpenTime = new TimeSpan(00, 00, 00);
                schedule.MondayCloseTime = new TimeSpan(00, 00, 00);
                _context.SaveChanges();
                Console.WriteLine("Monday Schedule saved!");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine();
                Console.WriteLine("Incorrect Monday Input!");
            }
            
        }
        else if(input == "2")
        {
            Janitor();
            Line();
            Console.WriteLine("Is Store Open in Tuesday? y/n");
            string inputTuesday = Console.ReadLine();
            
            if (inputTuesday.ToLower() == "y")
            {
                schedule.isTuesdayOpen = true;
                Console.WriteLine("Is tuesday open for all day? y/n");
                string isAllday = Console.ReadLine();

                if (isAllday.ToLower() == "y")
                {
                    schedule.isTuesdayOpen = true;
                    schedule.TuesdayOpenTime = new TimeSpan(00, 00, 01);
                    schedule.TuesdayCloseTime = new TimeSpan(23, 59, 59);
                    _context.SaveChanges();
                    Console.WriteLine("Tuesday Schedule saved!");
                }
                else if (isAllday.ToLower() == "n")
                {
                    schedule.isTuesdayAllDay = false;
                    Console.WriteLine("Enter Tuesday Open Time:");
                    schedule.TuesdayOpenTime = TimeSpan.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.WriteLine("Enter Tuesday Close Time:");
                    schedule.TuesdayCloseTime = TimeSpan.Parse(Console.ReadLine());
                    Console.WriteLine();
                    _context.SaveChanges();
                    Console.WriteLine("Tuesday Schedule saved!");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine();
                    Console.WriteLine("Incorrect Input!");
                }
            }
            else if (inputTuesday.ToLower() == "n")
            {
                schedule.isTuesdayOpen = false;
                schedule.isTuesdayAllDay = false;
                schedule.TuesdayOpenTime = new TimeSpan(00, 00, 00);
                schedule.TuesdayCloseTime = new TimeSpan(00, 00, 00);
                _context.SaveChanges();
                Console.WriteLine("Tuesday Schedule saved!");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine();
                Console.WriteLine("Incorrect Tuesday Input!");
            }
        }
        else if(input == "3")
        {
            Janitor();
            Line();
            Console.WriteLine("Is Store Open in Wednesday? y/n");
            string inputWednesday = Console.ReadLine();
            
            if (inputWednesday.ToLower() == "y")
            {
                schedule.isWednesdayOpen = true;
                Console.WriteLine("Is wednesday open for all day? y/n");
                string isAllday = Console.ReadLine();

                if (isAllday.ToLower() == "y")
                {
                    schedule.isWednesdayOpen = true;
                    schedule.WednesdayOpenTime = new TimeSpan(00, 00, 01);
                    schedule.WednesdayCloseTime = new TimeSpan(23, 59, 59);
                    _context.SaveChanges();
                    Console.WriteLine("Wednesday Schedule saved!");
                }
                else if (isAllday.ToLower() == "n")
                {
                    schedule.isWednesdayAllDay = false;
                    Console.WriteLine("Enter Wednesday Open Time:");
                    schedule.WednesdayOpenTime = TimeSpan.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.WriteLine("Enter Wednesday Close Time:");
                    schedule.WednesdayCloseTime = TimeSpan.Parse(Console.ReadLine());
                    Console.WriteLine();
                    _context.SaveChanges();
                    Console.WriteLine("Wednesday Schedule saved!");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine();
                    Console.WriteLine("Incorrect Input!");
                }
            }
            else if (inputWednesday.ToLower() == "n")
            {
                schedule.isWednesdayOpen = false;
                schedule.isWednesdayAllDay = false;
                schedule.WednesdayOpenTime = new TimeSpan(00, 00, 00);
                schedule.WednesdayCloseTime = new TimeSpan(00, 00, 00);
                _context.SaveChanges();
                Console.WriteLine("Wednesday Schedule saved!");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine();
                Console.WriteLine("Incorrect Wednesday Input!");
            }
        }
        else if(input == "4")
        {
            Janitor();
            Line();
            Console.WriteLine("Is Store Open in Thursday? y/n");
            string inputThursday = Console.ReadLine();
            
            if (inputThursday.ToLower() == "y")
            {
                schedule.isThursdayOpen = true;
                Console.WriteLine("Is thursday open for all day? y/n");
                string isAllday = Console.ReadLine();

                if (isAllday.ToLower() == "y")
                {
                    schedule.isThursdayOpen = true;
                    schedule.ThursdayOpenTime = new TimeSpan(00, 00, 01);
                    schedule.ThursdayCloseTime = new TimeSpan(23, 59, 59);
                    _context.SaveChanges();
                    Console.WriteLine("Thursday Schedule saved!");
                }
                else if (isAllday.ToLower() == "n")
                {
                    schedule.isThursdayAllDay = false;
                    Console.WriteLine("Enter Thursday Open Time:");
                    schedule.ThursdayOpenTime = TimeSpan.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.WriteLine("Enter Thursday Close Time:");
                    schedule.ThursdayCloseTime = TimeSpan.Parse(Console.ReadLine());
                    Console.WriteLine();
                    _context.SaveChanges();
                    Console.WriteLine("Thursday Schedule saved!");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine();
                    Console.WriteLine("Incorrect Input!");
                }
            }
            else if (inputThursday.ToLower() == "n")
            {
                schedule.isThursdayOpen = false;
                schedule.isThursdayAllDay = false;
                schedule.ThursdayOpenTime = new TimeSpan(00, 00, 00);
                schedule.ThursdayCloseTime = new TimeSpan(00, 00, 00);
                _context.SaveChanges();
                Console.WriteLine("Thursday Schedule saved!");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine();
                Console.WriteLine("Incorrect Thursday Input!");
            }
        }
        else if(input == "5")
        {
            Janitor();
            Line();
            Console.WriteLine("Is Store Open in Friday? y/n");
            string inputFriday = Console.ReadLine();
            
            if (inputFriday.ToLower() == "y")
            {
                schedule.isFridayOpen = true;
                Console.WriteLine("Is friday open for all day? y/n");
                string isAllday = Console.ReadLine();

                if (isAllday.ToLower() == "y")
                {
                    schedule.isFridayOpen = true;
                    schedule.FridayOpenTime = new TimeSpan(00, 00, 01);
                    schedule.FridayCloseTime = new TimeSpan(23, 59, 59);
                    _context.SaveChanges();
                    Console.WriteLine("Friday Schedule saved!");
                }
                else if (isAllday.ToLower() == "n")
                {
                    schedule.isFridayAllDay = false;
                    Console.WriteLine("Enter Friday Open Time:");
                    schedule.FridayOpenTime = TimeSpan.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.WriteLine("Enter Friday Close Time:");
                    schedule.FridayCloseTime = TimeSpan.Parse(Console.ReadLine());
                    Console.WriteLine();
                    _context.SaveChanges();
                    Console.WriteLine("Friday Schedule saved!");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine();
                    Console.WriteLine("Incorrect Input!");
                }
            }
            else if (inputFriday.ToLower() == "n")
            {
                schedule.isFridayOpen = false;
                schedule.isFridayAllDay = false;
                schedule.FridayOpenTime = new TimeSpan(00, 00, 00);
                schedule.FridayCloseTime = new TimeSpan(00, 00, 00);
                _context.SaveChanges();
                Console.WriteLine("Friday Schedule saved!");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine();
                Console.WriteLine("Incorrect Friday Input!");
            }
        }
        else if(input == "6")
        {
            Janitor();
            Line();
            Console.WriteLine("Is Store Open in Saturday? y/n");
            string inputSaturday = Console.ReadLine();
            
            if (inputSaturday.ToLower() == "y")
            {
                schedule.isSaturdayOpen = true;
                Console.WriteLine("Is saturday open for all day? y/n");
                string isAllday = Console.ReadLine();

                if (isAllday.ToLower() == "y")
                {
                    schedule.isSaturdayOpen = true;
                    schedule.SaturdayOpenTime = new TimeSpan(00, 00, 01);
                    schedule.SaturdayCloseTime = new TimeSpan(23, 59, 59);
                    _context.SaveChanges();
                    Console.WriteLine("Saturday Schedule saved!");
                }
                else if (isAllday.ToLower() == "n")
                {
                    schedule.isSaturdayAllDay = false;
                    Console.WriteLine("Enter Saturday Open Time:");
                    schedule.SaturdayOpenTime = TimeSpan.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.WriteLine("Enter Saturday Close Time:");
                    schedule.SaturdayCloseTime = TimeSpan.Parse(Console.ReadLine());
                    Console.WriteLine();
                    _context.SaveChanges();
                    Console.WriteLine("Saturday Schedule saved!");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine();
                    Console.WriteLine("Incorrect Input!");
                }
            }
            else if (inputSaturday.ToLower() == "n")
            {
                schedule.isSaturdayOpen = false;
                schedule.isSaturdayAllDay = false;
                schedule.SaturdayOpenTime = new TimeSpan(00, 00, 00);
                schedule.SaturdayCloseTime = new TimeSpan(00, 00, 00);
                _context.SaveChanges();
                Console.WriteLine("Saturday Schedule saved!");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine();
                Console.WriteLine("Incorrect Saturday Input!");
            }
        }
        else if(input == "7")
        {
            Janitor();
            Line();
            Console.WriteLine("Is Store Open in Sunday? y/n");
            string inputSunday = Console.ReadLine();
            
            if (inputSunday.ToLower() == "y")
            {
                schedule.isSundayOpen = true;
                Console.WriteLine("Is sunday open for all day? y/n");
                string isAllday = Console.ReadLine();

                if (isAllday.ToLower() == "y")
                {
                    schedule.isSundayOpen = true;
                    schedule.SundayOpenTime = new TimeSpan(00, 00, 01);
                    schedule.SundayCloseTime = new TimeSpan(23, 59, 59);
                    _context.SaveChanges();
                    Console.WriteLine("Sunday Schedule saved!");
                }
                else if (isAllday.ToLower() == "n")
                {
                    schedule.isSundayAllDay = false;
                    Console.WriteLine("Enter Sunday Open Time:");
                    schedule.SundayOpenTime = TimeSpan.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Console.WriteLine("Enter Sunday Close Time:");
                    schedule.SundayCloseTime = TimeSpan.Parse(Console.ReadLine());
                    Console.WriteLine();
                    _context.SaveChanges();
                    Console.WriteLine("Sunday Schedule saved!");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine();
                    Console.WriteLine("Incorrect Input!");
                }
            }
            else if (inputSunday.ToLower() == "n")
            {
                schedule.isSundayOpen = false;
                schedule.isSundayAllDay = false;
                schedule.SundayOpenTime = new TimeSpan(00, 00, 00);
                schedule.SundayCloseTime = new TimeSpan(00, 00, 00);
                _context.SaveChanges();
                Console.WriteLine("Sunday Schedule saved!");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine();
                Console.WriteLine("Incorrect Sunday Input!");
            }
        }
    }
    static public void DeleteInfo(Store store)
    {
        WoltColor();
        Janitor();
        Line();
        Console.WriteLine("Choose what to delate:");
        Console.WriteLine("1) Remove Store");
        Console.WriteLine("2) Remove Store Category (it will remove all data connected to it)");
        Console.WriteLine("3) Remove Store Prodaction (it will remove all data connected to it)");
        Console.WriteLine("4) Remove Prodaction Category (it will remove all data connected to it)");
        Console.WriteLine("5) Remove Product (it will remove all data connected to it)");
        Console.WriteLine("6) Remove Schedule");
        
        string input = Console.ReadLine();
        Choices inputChoice = new Choices { Choice = input };

        var validator = new ChoicesValidator(6);
        var result = validator.Validate(inputChoice);

        if (!result.IsValid)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed; 
            Janitor();
            Line();
            foreach (var error in result.Errors)
            {
                Console.WriteLine($"ERROR: Wrong {error.PropertyName} - {error.ErrorMessage}");
            }
        }
        else if (input == "1")
        {
            Janitor();
            Line();
            Console.WriteLine($"Removing {store.Name}");
            Console.WriteLine("Are you Sure? y/n");
            string sure = Console.ReadLine();

            if (sure.ToLower() == "y")
            {
                Console.WriteLine();
                Console.WriteLine("Removing...");
                _context.Remove(store);
                _context.SaveChanges();
                Console.WriteLine();
                Console.WriteLine("Delated!");
            }
            else if (sure.ToLower() == "n")
            {
                Console.WriteLine();
                Console.WriteLine("Removing aborted");
            }
            else
            {
                Janitor();
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Line();
                Console.WriteLine("Wrong input!");
            }
        }
        else if (input == "2")
        {
            Janitor();
            Line();
            Console.WriteLine($"Removing {store.Category.Name}");
            Console.WriteLine("Are you Sure? y/n");
            string sure = Console.ReadLine();

            if (sure.ToLower() == "y")
            {
                Console.WriteLine();
                Console.WriteLine("Removing...");
                _context.Remove(store.Category);
                _context.SaveChanges();
                Console.WriteLine();
                Console.WriteLine("Delated!");
            }
            else if (sure.ToLower() == "n")
            {
                Console.WriteLine();
                Console.WriteLine("Removing aborted");
            }
            else
            {
                Janitor();
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Line();
                Console.WriteLine("Wrong input!");
            }
        }
        else if (input == "3")
        {
            Janitor();
            Line();
            Console.WriteLine($"Removing Prodaction: {store.Category.Prodaction.Description}");
            Console.WriteLine("Are you Sure? y/n");
            string sure = Console.ReadLine();

            if (sure.ToLower() == "y")
            {
                Console.WriteLine();
                Console.WriteLine("Removing...");
                _context.Remove(store.Category.Prodaction);
                _context.SaveChanges();
                Console.WriteLine();
                Console.WriteLine("Delated!");
            }
            else if (sure.ToLower() == "n")
            {
                Console.WriteLine();
                Console.WriteLine("Removing aborted");
            }
            else
            {
                Janitor();
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Line();
                Console.WriteLine("Wrong input!");
            }
        }
        else if (input == "4")
        {
            Janitor();
            Line();
            Console.WriteLine($"Removing Prodaction Categories: ");
            Console.WriteLine();
            var allProdactionCategories = store.Category.Prodaction.Categories.ToList();
            foreach (var category in allProdactionCategories)
            {
                Console.WriteLine($"{category.Name} - ({category.Id})");
            }

            Console.WriteLine("Choose category ID to remove");
            int chooseCategoryID = int.Parse(Console.ReadLine());
            var choosenCategory = allProdactionCategories.FirstOrDefault(x => x.Id == chooseCategoryID);

            if (choosenCategory == null)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Janitor();
                Line();
                Console.WriteLine("Wrong category ID");
            }
            else
            {
                Janitor();
                Line();
                Console.WriteLine($"Removing {choosenCategory.Name}");
                Console.WriteLine("Are you Sure? y/n");
                string sure = Console.ReadLine();

                if (sure.ToLower() == "y")
                {
                    Console.WriteLine();
                    Console.WriteLine("Removing...");
                    _context.Remove(choosenCategory);
                    _context.SaveChanges();
                    Console.WriteLine();
                    Console.WriteLine("Delated!");
                }
                else if (sure.ToLower() == "n")
                {
                    Console.WriteLine();
                    Console.WriteLine("Removing aborted");
                }
                else
                {
                    Janitor();
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Line();
                    Console.WriteLine("Wrong input!");
                }
            }
            
        }
        else if (input == "5")
        {
            Janitor();
            Line();
            Console.WriteLine($"Removing Dish: ");
            Console.WriteLine();
            var allProductCategories = store.Category.Prodaction.Categories.ToList();
            foreach (var category in allProductCategories)
            {
                Console.WriteLine($"{category.Name} - ({category.Id})");
            }

            Console.WriteLine("Choose category ID to Search dish");
            int chooseCategoryID = int.Parse(Console.ReadLine());
            var choosenCategory = allProductCategories.FirstOrDefault(x => x.Id == chooseCategoryID);

            if (choosenCategory == null)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Janitor();
                Line();
                Console.WriteLine("Wrong category ID");
            }
            else
            {
                Janitor();
                Line();
                Console.WriteLine("Choose Product ID to Remove");
                Console.WriteLine();
                var allProducts = choosenCategory.Products.ToList();
                foreach (var product in allProducts)
                {
                    Console.WriteLine($"{product.Name} - ({product.Id})");
                }
                
                int choosenProductID = int.Parse(Console.ReadLine());
                var choosenProduct = allProducts.FirstOrDefault(x => x.Id == choosenProductID);
                if (choosenProduct == null)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Janitor();
                    Line();
                    Console.WriteLine("Wrong product ID");
                }
                else
                {
                    Console.WriteLine($"Removing {choosenProduct.Name}");
                    Console.WriteLine("Are you Sure? y/n");
                    string sure = Console.ReadLine();

                    if (sure.ToLower() == "y")
                    {
                        Console.WriteLine();
                        Console.WriteLine("Removing...");
                        _context.Remove(choosenProduct);
                        _context.SaveChanges();
                        Console.WriteLine();
                        Console.WriteLine("Delated!");
                    }
                    else if (sure.ToLower() == "n")
                    {
                        Console.WriteLine();
                        Console.WriteLine("Removing aborted");
                    }
                    else
                    {
                        Janitor();
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Line();
                        Console.WriteLine("Wrong input!");
                    }
                }
                
            }
            
        }
        else if (input == "6")
        {
            Janitor();
            Line();
            Console.WriteLine($"Removing {store.Name} Schedule");
            Console.WriteLine("Are you Sure? y/n");
            string sure = Console.ReadLine();

            if (sure.ToLower() == "y")
            {
                Console.WriteLine();
                Console.WriteLine("Removing...");
                _context.Remove(store.Category.Schedule);
                _context.SaveChanges();
                Console.WriteLine();
                Console.WriteLine("Delated!");
            }
            else if (sure.ToLower() == "n")
            {
                Console.WriteLine();
                Console.WriteLine("Removing aborted");
            }
            else
            {
                Janitor();
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Line();
                Console.WriteLine("Wrong input!");
            }
            
        }
    }
    
}