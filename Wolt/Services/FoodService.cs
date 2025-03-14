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

public class FoodService
{
    public static string baseRoute = @"C:\\Users\\kmami\\RiderProjects\\Wolt\\Logs";
    public static string vanueLog = "vanue.txt";
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
    static public void AddFoodVanue(Owner owner)
    {
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Janitor();
        Line();
        Console.WriteLine("Welcome to Food Vanue Add Section!");
        Console.WriteLine("Enter Food Vanue Name:");
        string name = Console.ReadLine();
        Console.WriteLine();
        
        Console.WriteLine("Enter Food Vanue City:");
        string city = Console.ReadLine();
        Console.WriteLine();

        Console.WriteLine("Enter Food Vanue Address:");
        string address = Console.ReadLine();
        Console.WriteLine();

        Console.WriteLine("Enter Food Vanue Phone Number:");
        string phone = Console.ReadLine();
        Console.WriteLine();
        
        Console.WriteLine("Enter Food Vanue Order Fee:");
        decimal orderFee = decimal.Parse(Console.ReadLine());
        Console.WriteLine();

        Console.WriteLine("Enter Food Vanue Estimated Order Time:");
        TimeSpan orderTime = TimeSpan.Parse(Console.ReadLine());
        Console.WriteLine();
        Console.WriteLine("Please Wait...");

        FoodChain newFoodChain = new FoodChain()
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
        
        var validator = new FoodChainValidator();
        var result = validator.Validate(newFoodChain);

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
            
            _context.FoodChains.Add(newFoodChain);
            _context.SaveChanges();
            Janitor();
            string fullRoute = Path.Combine(baseRoute, vanueLog);

            using (StreamWriter wr = new StreamWriter(fullRoute, true))
            {
                wr.WriteLine($"[LOG] Vanue added -> [{newFoodChain.Name}] - {DateTime.Now}");
            }
            Console.WriteLine($"Owner {owner.FirstName} {owner.LastName} Created new Food Vanue '{newFoodChain.Name}'");
        }
    }
    static public void AddDetails(Owner owner)
    {
        WoltColor();
        Janitor();
        Line();
        Console.WriteLine($"{owner.FirstName} {owner.LastName}'s Food Vanues:");
        Console.WriteLine();
        
        var allVenues = _context.FoodChains
                    .Where(x=>x.OwnerId == owner.Id)
                    .ToList();
        
        foreach (var venue in allVenues)
        {
            Console.WriteLine($"{venue.Name} - ({venue.Id})");
            Console.WriteLine();
        }

        Console.WriteLine("Enter Venue ID to see and add more details:");
        int choice = int.Parse(Console.ReadLine());
        var choosenVenue = _context.FoodChains
            .Include(x => x.Category)
            .ThenInclude(x => x.Schedule)
            .Include(x => x.Category.Menu)
            .ThenInclude(x => x.Categories)
            .ThenInclude(x => x.Foods)
            .ThenInclude(x => x.Ingridients)
            .FirstOrDefault(x => x.Id == choice);

        if (choosenVenue == null)
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
            Console.WriteLine($"{choosenVenue.Name} Info:");
            Console.WriteLine();
            Console.WriteLine($"City : {choosenVenue.City}");
            Console.WriteLine($"Address : {choosenVenue.Address}");
            Console.WriteLine($"Phone Number : {choosenVenue.Phone : N0}");
            Console.WriteLine($"Order Fee : {choosenVenue.OrderFee}");
            Console.WriteLine($"Order Time : {choosenVenue.OrderTime}");
            Console.WriteLine($"Rating : {choosenVenue.Rating}");
            Console.WriteLine();
            Console.WriteLine("1) Change Current Information");
            Console.WriteLine("2) Add Food Vanue Category and Create Menu Configuration!");
            Console.WriteLine("3) Add Menu Category");
            Console.WriteLine("4) Add Dishes");
            Console.WriteLine("5) See Menu");
            Console.WriteLine("6) Change Menu Information");
            Console.WriteLine("7) Add Vanue Scedule");
            Console.WriteLine("8) Change Vanue Scedule");
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
                ChangeFoodVanue(choosenVenue);
            }
            else if (input == "2")
            {
                MenuConfiguration(choosenVenue);
            }
            else if (input == "3")
            {
                if (choosenVenue.Category == null || choosenVenue.Category.Menu == null)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Janitor();
                    Line();
                    Console.WriteLine("You must Configure Menu First!");
                }
                else
                {
                    AddMenuCategory(choosenVenue.Category.Menu);
                }
            }
            else if (input == "4")
            {
                if (choosenVenue.Category.Menu.Categories == null)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Janitor();
                    Line();
                    Console.WriteLine("You must Create Menu Category First!");
                }
                AddDishes(choosenVenue.Category.Menu);
            }
            else if (input == "5")
            {
                SeeAllMenu(choosenVenue);
            }
            else if (input == "6")
            {
                ChangeMenuInfo(choosenVenue);
            }
            else if (input == "7")
            {
                if (choosenVenue.Category == null || choosenVenue.Category.Menu == null)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Janitor();
                    Line();
                    Console.WriteLine("You must Configure Menu First!");
                }
                else
                {
                    AddScedule(choosenVenue);
                } 
            }
            else if (input == "8")
            {
                if (choosenVenue?.Category == null || choosenVenue.Category.Schedule == null)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Janitor();
                    Line();
                    Console.WriteLine("You must Configure Menu Schedule First!");
                }
                else
                {
                    ChangeSchedule(choosenVenue.Category.Schedule);
                }
            }
            else if (input == "9")
            {
                DeleteInfo(choosenVenue);
            }
            else if (input == "10")
            {
                Console.WriteLine();
                Console.WriteLine("logging off...");
                Environment.Exit(0);
            }
        }
    }
    static public void ChangeFoodVanue(FoodChain foodChain)
    {
        WoltColor();
        Janitor();
        Line();
        Console.WriteLine("Choose what to change:");
        Console.WriteLine();
        Console.WriteLine("1) Change Vanue name");
        Console.WriteLine("2) Change Vanue City");
        Console.WriteLine("3) Change Vanue Address");
        Console.WriteLine("4) Change Vanue Phone Number");
        Console.WriteLine("5) Change Vanue Order Fee");
        Console.WriteLine("6) Change Vanue Order Time");
        
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
            Console.WriteLine($"Current Vanue Name: {foodChain.Name}");
            Console.WriteLine();
            Console.WriteLine("Enter new Vanue Name:");
            string newName = Console.ReadLine();
            Console.WriteLine("Please wait...");
            foodChain.Name = newName;
            var vanueValidator = new FoodChainValidator();
            var venueResult = vanueValidator.Validate(foodChain);
            
            if (!venueResult.IsValid)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Janitor();
                Line();
                foreach (var error in venueResult.Errors)
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
                Console.WriteLine($"Vanue name updated to: {foodChain.Name}");
            }
        }
        else if (input == "2")
        {
            Janitor();
            Line();
            Console.WriteLine($"Current Vanue City: {foodChain.City}");
            Console.WriteLine();
            Console.WriteLine("Enter new Vanue City:");
            string newCity = Console.ReadLine();
            Console.WriteLine("Please wait...");
            foodChain.City = newCity;
            var vanueValidator = new FoodChainValidator();
            var venueResult = vanueValidator.Validate(foodChain);
            
            if (!venueResult.IsValid)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Janitor();
                Line();
                foreach (var error in venueResult.Errors)
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
                Console.WriteLine($"Vanue city updated to: {foodChain.City}");
            }
        }
        else if (input == "3")
        {
            Janitor();
            Line();
            Console.WriteLine($"Current Vanue Address: {foodChain.Address}");
            Console.WriteLine();
            Console.WriteLine("Enter new Vanue Address:");
            string newAddress = Console.ReadLine();
            Console.WriteLine("Please wait...");
            foodChain.Address = newAddress;
            var vanueValidator = new FoodChainValidator();
            var venueResult = vanueValidator.Validate(foodChain);
            
            if (!venueResult.IsValid)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Janitor();
                Line();
                foreach (var error in venueResult.Errors)
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
                Console.WriteLine($"Vanue address updated to: {foodChain.Address}");
            }
        }
        else if (input == "4")
        {
            Janitor();
            Line();
            Console.WriteLine($"Current Vanue Phone Number: {foodChain.Phone}");
            Console.WriteLine();
            Console.WriteLine("Enter new Vanue Phone Number:");
            string newPhone = Console.ReadLine();
            Console.WriteLine("Please wait...");
            foodChain.Phone = newPhone;
            var vanueValidator = new FoodChainValidator();
            var venueResult = vanueValidator.Validate(foodChain);
            
            if (!venueResult.IsValid)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Janitor();
                Line();
                foreach (var error in venueResult.Errors)
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
                Console.WriteLine($"Vanue phone numeber updated to: {foodChain.Phone}");
            }
        }
        else if (input == "5")
        {
            Janitor();
            Line();
            Console.WriteLine($"Current Vanue Order Fee: {foodChain.OrderFee}");
            Console.WriteLine();
            Console.WriteLine("Enter new Vanue Order Fee:");
            decimal newOrderFee = decimal.Parse(Console.ReadLine());
            Console.WriteLine("Please wait...");
            foodChain.OrderFee = newOrderFee;
            var vanueValidator = new FoodChainValidator();
            var venueResult = vanueValidator.Validate(foodChain);
            
            if (!venueResult.IsValid)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Janitor();
                Line();
                foreach (var error in venueResult.Errors)
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
                Console.WriteLine($"Vanue order fee updated to: {foodChain.OrderFee}");
            }
        }
        else if (input == "6")
        {
            Janitor();
            Line();
            Console.WriteLine($"Current Vanue Order Time: {foodChain.OrderTime}");
            Console.WriteLine();
            Console.WriteLine("Enter new Vanue Order Time:");
            TimeSpan newOrderTime = TimeSpan.Parse(Console.ReadLine());
            Console.WriteLine("Please wait...");
            foodChain.OrderTime = newOrderTime;
            var vanueValidator = new FoodChainValidator();
            var venueResult = vanueValidator.Validate(foodChain);
            
            if (!venueResult.IsValid)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Janitor();
                Line();
                foreach (var error in venueResult.Errors)
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
                Console.WriteLine($"Vanue Order Time updated to: {foodChain.OrderTime}");
            }
        }
    }
    static public void MenuConfiguration(FoodChain foodChain)
    {
        WoltColor();
        Janitor();
        Line();
        Console.WriteLine($"Welcome to {foodChain.Name} Configuration!");
        Console.WriteLine();
        
        Console.WriteLine("Enter Venue Category Type (Cafe, Fast Food e.t):");
        string newCategoryName = Console.ReadLine();
        Console.WriteLine();

        FoodCategory newCategory = new FoodCategory
        {
            Name = newCategoryName,
            FoodChainId = foodChain.Id
        };
        var categoryValidator = new FoodCategoryValidator();
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
            _context.FoodCategories.Add(newCategory);
            _context.SaveChanges();

            Console.WriteLine("Enter Menu Description:");
            string newMenuDescription = Console.ReadLine();
            Console.WriteLine();

            Menu newMenu = new Menu
            {
                Description = newMenuDescription,
                FoodCategoryId = foodChain.Category.Id
            };
        
            var menuValidator = new MenuValidator();
            var menuResult = menuValidator.Validate(newMenu);

            if (!menuResult.IsValid)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Janitor();
                Line();
                foreach (var error in menuResult.Errors)
                {
                    Console.WriteLine($"ERROR: Wrong {error.PropertyName} - {error.ErrorMessage}");
                }
            }
            else
            {
                _context.Menus.Add(newMenu);
                _context.SaveChanges();

                Console.WriteLine("Menu Configuration Ended!");
            }
        
            
        }
        

    }
    static public void AddMenuCategory(Menu menu)
    {
        WoltColor();
        Janitor();
        Line();
        Console.WriteLine("Enter Menu Category (Main, Drinks, e.t):");
        string newName = Console.ReadLine();
        Console.WriteLine();

        Console.WriteLine("Enter Menu Category Description:");
        string newDescription = Console.ReadLine();
        Console.WriteLine();

        Console.WriteLine("Please wait...");

        MenuCategory newCategoty = new MenuCategory
        {
            Name = newName,
            Description = newDescription,
            MenuId = menu.Id
        };
        
        var menuCategoryValidator = new MenuCategoryValidator();
        var menuCategoryResult = menuCategoryValidator.Validate(newCategoty);

        if (!menuCategoryResult.IsValid)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Janitor();
            Line();
            foreach (var error in menuCategoryResult.Errors)
            {
                Console.WriteLine($"ERROR: Wrong {error.PropertyName} - {error.ErrorMessage}");
            }
        }
        else
        {
            Janitor();
            Line();
            Console.WriteLine("Saving Category...");
            _context.MenuCategories.Add(newCategoty);
            _context.SaveChanges();
            Console.WriteLine($"Category {newCategoty.Name} Added!");
        }
        
        
    }
    static public void AddDishes(Menu menu)
    {
        WoltColor();
        Janitor();
        Line();
        Console.WriteLine("Choose Dish Category ID:");
        Console.WriteLine();
        var allMenuCategory = menu.Categories.ToList();

        foreach (var category in allMenuCategory)
        {
            Console.WriteLine($"{category.Name} - ({category.Id})");
        }
        
        int choosenCategoryId = int.Parse(Console.ReadLine());
        var choosenCategory = _context.MenuCategories
                                .FirstOrDefault(x => x.Id == choosenCategoryId);

        if (choosenCategory == null)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Janitor();
            Line();
            Console.WriteLine("Choose Correct Dish Category ID!");
        }
        else
        {
            Console.WriteLine("Enter Dish Name:");
            string newDishName = Console.ReadLine();
            Console.WriteLine();

            Console.WriteLine("Enter Dish Price:");
            decimal newDishPrice = decimal.Parse(Console.ReadLine());
            Console.WriteLine();

            Console.WriteLine("Please Wait...");
            Console.WriteLine();
            
            Food newFood = new Food
            {
                Name = newDishName,
                Price = newDishPrice,
                MenuCategoryId = choosenCategoryId
            };
            
            var foodValidator = new FoodValidator();
            var foodResult = foodValidator.Validate(newFood);

            if (!foodResult.IsValid)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Janitor();
                Line();
                foreach (var error in foodResult.Errors)
                {
                    Console.WriteLine($"ERROR: Wrong {error.PropertyName} - {error.ErrorMessage}");
                }
            }
            else
            {
                _context.Foods.Add(newFood);
                _context.SaveChanges();

                Console.WriteLine("How many Ingredients do you want to add?");
                int ingridientCount;
                bool isValid = int.TryParse(Console.ReadLine(), out ingridientCount);
                if (!isValid)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Janitor();
                    Line();
                    Console.WriteLine("please enter a valid integer.");
                }
                else
                {
                    for (int i = 1; i <= ingridientCount; i++)
                    {
                        Console.WriteLine($"Enter Dish's Ingridient number {i} :");
                        string newDishIngridientsName = Console.ReadLine();
                        Console.WriteLine();

                        Console.WriteLine("Enter Ingridient Calories:");
                        decimal newDishIngridientCal = decimal.Parse(Console.ReadLine());
                        Console.WriteLine();

                        Console.WriteLine("Enter Ingridient Additional Price:");
                        decimal newDishAdditionalPrice = decimal.Parse(Console.ReadLine());
                        Console.WriteLine();
                        Console.WriteLine("Please Wait...");

                        Ingridients newIngridients = new Ingridients
                        {
                            Name = newDishIngridientsName,
                            Calories = newDishIngridientCal,
                            AdditionalPrice = newDishAdditionalPrice,
                            FoodId = newFood.Id
                        };
                
                        var ingridientsValidator = new IngridientsValidator();
                        var ingridientsResult = ingridientsValidator.Validate(newIngridients);
                
                        if (!ingridientsResult.IsValid)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Janitor();
                            Line();
                            foreach (var error in ingridientsResult.Errors)
                            {
                                Console.WriteLine($"ERROR: Wrong {error.PropertyName} - {error.ErrorMessage}");
                            }
                        }
                        else
                        {
                            _context.Ingridients.Add(newIngridients);
                            _context.SaveChanges();
                            Console.WriteLine("Ingridient added!");
                        }
                    }
                }

                
            }
            
            
        }
        
        
    }
    static public void SeeAllMenu(FoodChain foodChain)
    {
        WoltColor();
        Janitor();
        Line();
        Console.WriteLine($"============= {foodChain.Name} ============");
        Console.WriteLine();
        Console.WriteLine($"Venue Category: {foodChain.Category.Name}");
        Console.WriteLine($"Menu: {foodChain.Category.Menu.Description}");
        Console.WriteLine();
        if (foodChain.Category.Schedule == null)
        {
            Console.WriteLine("Schedule is not configured!");
        }
        else
        {
            ScheduleWrite(foodChain.Category.Schedule);
        }
        
        Console.WriteLine();
        Console.WriteLine("==================== Menu ==================");
        var allMenuCategories = foodChain.Category.Menu.Categories.ToList();

        foreach (var category in allMenuCategories)
        {
            Line();
            Console.WriteLine($"[Category] - ({category.Name}) - {category.Description}");
            Console.WriteLine("Dishes:");
            Console.WriteLine();
            foreach (var food in category.Foods)
            {
                Console.WriteLine("******************************************");
                Console.WriteLine($"[Dish] - {food.Name} - {food.Price :C} -> [{food.Id}]");
                Console.WriteLine("------------------------------------------");
                Console.WriteLine("Ingridients:");
                Console.WriteLine();
                foreach (var ingridients in food.Ingridients)
                {
                    Console.WriteLine($"{ingridients.Name} - {ingridients.Calories}CAL - Additional Price: {ingridients.AdditionalPrice :C}");
                }
                Console.WriteLine("------------------------------------------");
            }
        }
    }
    static public void ChangeMenuInfo(FoodChain foodChain)
    {
        WoltColor();
        Janitor();
        Line();
        Console.WriteLine("Choose What to change:");
        Console.WriteLine("1) Change Venue Category Name");
        Console.WriteLine("2) Change Menu Description");
        Console.WriteLine("3) Change Menu Category");
        Console.WriteLine("4) Change Dish Info");
        Console.WriteLine("5) Change Dish Ingridients");
        
        string input = Console.ReadLine();
        Choices inputChoices = new Choices(){ Choice = input};
        var choiceValidator = new ChoicesValidator(5);
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
            Console.WriteLine($"Actual Venue Category: {foodChain.Category.Name}");
            Console.WriteLine("Enter new Vanue Category Name:");
            string newVanueCategoryName = Console.ReadLine();
            
            foodChain.Category.Name = newVanueCategoryName;
            var categoryValidator = new FoodCategoryValidator();
            var categoryResult = categoryValidator.Validate(foodChain.Category);
            
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
                Console.WriteLine($"New Venue Category: {foodChain.Category.Name}");
            }

            
        }
        else if (input == "2")
        {
            Janitor();
            Line();
            Console.WriteLine($"Actual Menu Description: {foodChain.Category.Menu.Description}");
            Console.WriteLine("Enter new Menu Description:");
            string newMenuDesc = Console.ReadLine();
            
            foodChain.Category.Menu.Description = newMenuDesc;
            var menuValidator = new MenuValidator();
            var menuResult = menuValidator.Validate(foodChain.Category.Menu);
            
            if (menuResult.IsValid == false)
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
                Console.WriteLine($"New Menu Description: {foodChain.Category.Menu.Description}");
            }

            
        }
        else if (input == "3")
        {
            Janitor();
            Line();
            Console.WriteLine("Choose Menu Category ID:");
            Console.WriteLine();
            var allCategory = foodChain.Category.Menu.Categories.ToList();
            foreach (var category in allCategory)
            {
                Console.WriteLine($"{category.Name} - ({category.Id})");
            }

            Console.WriteLine("Enter Menu Category ID:");
            int newMenuCategoryId = int.Parse(Console.ReadLine());
            var choosenCategory = allCategory.FirstOrDefault(c => c.Id == newMenuCategoryId);

            if (choosenCategory == null)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Janitor();
                Line();
                Console.WriteLine("Select correct a Category ID!");
            }
            else
            {
                Console.WriteLine("Enter new Menu Category Name:");
                string newMenuCategoryName = Console.ReadLine();
                Console.WriteLine();

                Console.WriteLine("Enter new Menu Category Description:");
                string newMenuCategoryDesc = Console.ReadLine();
                Console.WriteLine();
            
                choosenCategory.Name = newMenuCategoryName;
                choosenCategory.Description = newMenuCategoryDesc;
            
                var menuCategoryValidator = new MenuCategoryValidator();
                var menuCategoryResult =  menuCategoryValidator.Validate(choosenCategory);
            
                if (menuCategoryResult.IsValid == false)
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
                    Console.WriteLine($"New Menu Category Name: {choosenCategory.Name},");
                    Console.WriteLine($"New Menu Category Description: {choosenCategory.Description},");
                }
            }
            

        }
        else if (input == "4")
        {
            Janitor();
            Line();
            Console.WriteLine("Choose Menu Category ID:");
            Console.WriteLine();
            var allCategory = foodChain.Category.Menu.Categories.ToList();
            foreach (var category in allCategory)
            {
                Console.WriteLine($"{category.Name} - ({category.Id})");
            }

            Console.WriteLine("Enter Menu Category ID:");
            int newMenuCategoryId = int.Parse(Console.ReadLine());
            var choosenCategory = allCategory.FirstOrDefault(c => c.Id == newMenuCategoryId);

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
                Console.WriteLine("Choose Dish Id");
                var allDish = choosenCategory.Foods.ToList();

                foreach (var dish in allDish)
                {
                    Console.WriteLine($"{dish.Name} - ({dish.Id})");
                }

                Console.WriteLine("Enter Dish Id:");
                int newDishId = int.Parse(Console.ReadLine());
                var choosenDish = allDish.FirstOrDefault(d => d.Id == newDishId);

                if (choosenDish == null)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Janitor();
                    Line();
                    Console.WriteLine("Select correct a Dish ID!");
                }
                else
                {
                    Janitor();
                    Line();
            
                    Console.WriteLine("Enter new Dish Name:");
                    string newDishName = Console.ReadLine();
                    Console.WriteLine();

                    Console.WriteLine("Enter new Dish Price:");
                    decimal newDishPrice = decimal.Parse(Console.ReadLine());
                    Console.WriteLine();
            
                    choosenDish.Name = newDishName;
                    choosenDish.Price = newDishPrice;
            
                    var dishValidator = new FoodValidator();
                    var dishResult =  dishValidator.Validate(choosenDish);
            
                    if (dishResult.IsValid == false)
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
                        Console.WriteLine($"New Dish Name: {choosenDish.Name},");
                        Console.WriteLine($"New Dish Price: {choosenDish.Price}");
                    }
                    
                }
                
            }

            
        }
        else if (input == "5")
        {
            Janitor();
            Line();
            Console.WriteLine("Choose Menu Category ID:");
            Console.WriteLine();
            var allCategory = foodChain.Category.Menu.Categories.ToList();
            foreach (var category in allCategory)
            {
                Console.WriteLine($"{category.Name} - ({category.Id})");
            }

            Console.WriteLine("Enter Menu Category ID:");
            int newMenuCategoryId = int.Parse(Console.ReadLine());
            var choosenCategory = allCategory.FirstOrDefault(c => c.Id == newMenuCategoryId);

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
                Console.WriteLine("Choose Dish Id");
                var allDish = choosenCategory.Foods.ToList();

                foreach (var dish in allDish)
                {
                    Console.WriteLine($"{dish.Name} - ({dish.Id})");
                }
            
                Console.WriteLine("Enter Dish Id:");
                int newDishId = int.Parse(Console.ReadLine());
                var choosenDish = allDish.FirstOrDefault(d => d.Id == newDishId);

                if (choosenDish == null)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Janitor();
                    Line();
                    Console.WriteLine("Select correct a Dish ID!");
                }
                else
                {
                    Janitor();
                    Line();
                    Console.WriteLine("Choose Ingridient Id");
                    Console.WriteLine();
                    var allIngridient = choosenDish.Ingridients.ToList();
                    foreach (var ingridient in allIngridient)
                    {
                        Console.WriteLine($"{ingridient.Name} - ({ingridient.Id})");
                    }

                    Console.WriteLine("Enter Ingridient Id:");
                    int newIngridientId = int.Parse(Console.ReadLine());
                    var choosenIngridient = choosenDish.Ingridients.FirstOrDefault(x => x.Id == newIngridientId);
                    if (choosenIngridient == null)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Janitor();
                        Line();
                        Console.WriteLine("Select correct a Ingridient ID!");
                    }
                    else
                    {
                        Janitor();
                        Line();
                        Console.WriteLine("Enter new Ingidient Name:");
                        string newIngName = Console.ReadLine();
                        Console.WriteLine();

                        Console.WriteLine("Enter new Ingridient Calories:");
                        decimal newIngCal = decimal.Parse(Console.ReadLine());
                        Console.WriteLine();
                    
                        Console.WriteLine("Enter new Ingridient Additional Price:");
                        decimal newIngPrice = decimal.Parse(Console.ReadLine());
                        Console.WriteLine();
                    
                        choosenIngridient.Name = newIngName;
                        choosenIngridient.Calories = newIngCal;
                        choosenIngridient.AdditionalPrice = newIngPrice;
                    
                        var ingValidator = new IngridientsValidator();
                        var ingResult =  ingValidator.Validate(choosenIngridient);
                    
                        if (ingResult.IsValid == false)
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
                            Console.WriteLine($"New Ingridient Name: {choosenIngridient.Name},");
                            Console.WriteLine($"New Ingridient Calories: {choosenIngridient.Calories}");
                            Console.WriteLine($"New Ingridient Additional Pricce: {choosenIngridient.AdditionalPrice}");
                        }

                        
                    }
                    
                }
                
                
            }

            
        }
    }
    static public void AddScedule(FoodChain foodChain)
    {
        WoltColor();
        Janitor();
        Line();
        Console.WriteLine($"Configure {foodChain.Name} Scedule...");
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

        Schedule newSchedule = new Schedule
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
            
            FoodCategoryId = foodChain.Category.Id,
        };
        
        var scheduleValidator = new ScheduleValidator();
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
        
            _context.Schedules.Add(newSchedule);
            _context.SaveChanges();
            Janitor();
            Line();
            Console.WriteLine("Schedule saved!");
        }

        
    }
    static public void ScheduleWrite(Schedule schedule)
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
    static public void ChangeSchedule(Schedule schedule)
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
            Console.WriteLine("Is Venue Open in Monday? y/n");
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
            Console.WriteLine("Is Venue Open in Tuesday? y/n");
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
            Console.WriteLine("Is Venue Open in Wednesday? y/n");
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
            Console.WriteLine("Is Venue Open in Thursday? y/n");
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
            Console.WriteLine("Is Venue Open in Friday? y/n");
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
            Console.WriteLine("Is Venue Open in Saturday? y/n");
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
            Console.WriteLine("Is Venue Open in Sunday? y/n");
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
    static public void DeleteInfo(FoodChain foodChain)
    {
        WoltColor();
        Janitor();
        Line();
        Console.WriteLine("Choose what to delate:");
        Console.WriteLine("1) Remove Vanue");
        Console.WriteLine("2) Remove Vanue Category (it will remove all data connected to it)");
        Console.WriteLine("3) Remove Vanue Menu (it will remove all data connected to it)");
        Console.WriteLine("4) Remove Menu Category (it will remove all data connected to it)");
        Console.WriteLine("5) Remove Dish (it will remove all data connected to it)");
        Console.WriteLine("6) Remove Dish Ingridient");
        Console.WriteLine("7) Remove Schedule");
        
        string input = Console.ReadLine();
        Choices inputChoice = new Choices { Choice = input };

        var validator = new ChoicesValidator(7);
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
            Console.WriteLine($"Removing {foodChain.Name}");
            Console.WriteLine("Are you Sure? y/n");
            string sure = Console.ReadLine();

            if (sure.ToLower() == "y")
            {
                Console.WriteLine();
                Console.WriteLine("Removing...");
                _context.Remove(foodChain);
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
            Console.WriteLine($"Removing {foodChain.Category.Name}");
            Console.WriteLine("Are you Sure? y/n");
            string sure = Console.ReadLine();

            if (sure.ToLower() == "y")
            {
                Console.WriteLine();
                Console.WriteLine("Removing...");
                _context.Remove(foodChain.Category);
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
            Console.WriteLine($"Removing Menu: {foodChain.Category.Menu.Description}");
            Console.WriteLine("Are you Sure? y/n");
            string sure = Console.ReadLine();

            if (sure.ToLower() == "y")
            {
                Console.WriteLine();
                Console.WriteLine("Removing...");
                _context.Remove(foodChain.Category.Menu);
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
            Console.WriteLine($"Removing Menu Categories: ");
            Console.WriteLine();
            var allMenuCategories = foodChain.Category.Menu.Categories.ToList();
            foreach (var category in allMenuCategories)
            {
                Console.WriteLine($"{category.Name} - ({category.Id})");
            }

            Console.WriteLine("Choose category ID to remove");
            int chooseCategoryID = int.Parse(Console.ReadLine());
            var choosenCategory = allMenuCategories.FirstOrDefault(x => x.Id == chooseCategoryID);

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
            var allMenuCategories = foodChain.Category.Menu.Categories.ToList();
            foreach (var category in allMenuCategories)
            {
                Console.WriteLine($"{category.Name} - ({category.Id})");
            }

            Console.WriteLine("Choose category ID to Search dish");
            int chooseCategoryID = int.Parse(Console.ReadLine());
            var choosenCategory = allMenuCategories.FirstOrDefault(x => x.Id == chooseCategoryID);

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
                Console.WriteLine("Choose Dish ID to Remove");
                Console.WriteLine();
                var allDishes = choosenCategory.Foods.ToList();
                foreach (var dish in allDishes)
                {
                    Console.WriteLine($"{dish.Name} - ({dish.Id})");
                }
                
                int choosenDishID = int.Parse(Console.ReadLine());
                var choosenDish = allDishes.FirstOrDefault(x => x.Id == choosenDishID);
                if (choosenDish == null)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Janitor();
                    Line();
                    Console.WriteLine("Wrong dish ID");
                }
                else
                {
                    Console.WriteLine($"Removing {choosenDish.Name}");
                    Console.WriteLine("Are you Sure? y/n");
                    string sure = Console.ReadLine();

                    if (sure.ToLower() == "y")
                    {
                        Console.WriteLine();
                        Console.WriteLine("Removing...");
                        _context.Remove(choosenDish);
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
            Console.WriteLine($"Removing Dish Ingridient: ");
            Console.WriteLine();
            var allMenuCategories = foodChain.Category.Menu.Categories.ToList();
            foreach (var category in allMenuCategories)
            {
                Console.WriteLine($"{category.Name} - ({category.Id})");
            }

            Console.WriteLine("Choose category ID to Search dish");
            int chooseCategoryID = int.Parse(Console.ReadLine());
            var choosenCategory = allMenuCategories.FirstOrDefault(x => x.Id == chooseCategoryID);

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
                Console.WriteLine("Choose Dish ID");
                Console.WriteLine();
                var allDishes = choosenCategory.Foods.ToList();
                foreach (var dish in allDishes)
                {
                    Console.WriteLine($"{dish.Name} - ({dish.Id})");
                }
                
                int choosenDishID = int.Parse(Console.ReadLine());
                var choosenDish = allDishes.FirstOrDefault(x => x.Id == choosenDishID);
                if (choosenDish == null)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Janitor();
                    Line();
                    Console.WriteLine("Wrong dish ID");
                }
                else
                {   
                    Janitor();
                    Line();
                    Console.WriteLine("Choose Ingridient to remove");
                    var allIngridient = choosenDish.Ingridients.ToList();
                    foreach (var ingridient in allIngridient)
                    {
                        Console.WriteLine($"{ingridient.Name} - ({ingridient.Id})");
                    }
                    int choosenIngId = int.Parse(Console.ReadLine());
                    var choosenIng = allIngridient.FirstOrDefault(x => x.Id == choosenIngId);

                    if (choosenIng == null)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Janitor();
                        Line();
                        Console.WriteLine("Wrong ingridient ID");
                    }
                    else
                    {
                        Console.WriteLine($"Removing {choosenIng.Name}");
                        Console.WriteLine("Are you Sure? y/n");
                        string sure = Console.ReadLine();

                        if (sure.ToLower() == "y")
                        {
                            Console.WriteLine();
                            Console.WriteLine("Removing...");
                            _context.Remove(choosenIng);
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
        }
        else if (input == "7")
        {
            Janitor();
            Line();
            Console.WriteLine($"Removing {foodChain.Name} Schedule");
            Console.WriteLine("Are you Sure? y/n");
            string sure = Console.ReadLine();

            if (sure.ToLower() == "y")
            {
                Console.WriteLine();
                Console.WriteLine("Removing...");
                _context.Remove(foodChain.Category.Schedule);
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