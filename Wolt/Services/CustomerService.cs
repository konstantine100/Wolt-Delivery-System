using Microsoft.EntityFrameworkCore;
using Wolt.Data;
using Wolt.Models;
using Wolt.Validator;
using Wolt.SMTP;
using BCrypt.Net;
using System.Globalization;
using System.Text;
using Newtonsoft.Json.Linq;
using Xceed.Words.NET;

namespace Wolt.Services;

public class CustomerService
{
    public static string baseRoute = @"C:\\Users\\kmami\\RiderProjects\\Wolt\\Logs";
    public static string customerLog = "customers.txt";
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
    
    static public void AddCustomer()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Janitor();
        Line();
        Console.WriteLine("Create Customer Account");
        
        Console.WriteLine("\r");
        Console.WriteLine("Customer First Name:");
        string firstName = Console.ReadLine();
        
        Console.WriteLine("\r");
        Console.WriteLine("Customer Last Name:");
        string lastName = Console.ReadLine();
        
        Console.WriteLine("\r");
        Console.WriteLine("Customer Email:");
        string email = Console.ReadLine();

        Console.WriteLine("\r");
        Console.WriteLine("Customer Password:");
        string password = Console.ReadLine();
        
        Line();
        Console.WriteLine("Please Wait...");
        
        Customer newCustomer = new Customer
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Password = password,
            Balance = 0.0m
        };
        
        var validator = new CustomerValidator();
        
        var result = validator.Validate(newCustomer);
    
        if (result.IsValid == false)
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
            Console.WriteLine("Processing...");
            string? GenerateRandomPassword()
            {
                const string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                Random random = new Random();
    
                StringBuilder passwordBuilder = new StringBuilder(7);
    
                for (int i = 0; i < 7; i++)
                {
                    int index = random.Next(validChars.Length);
                    passwordBuilder.Append(validChars[index]);
                }
    
                return passwordBuilder.ToString();
            }
    
            string? randomNumber = GenerateRandomPassword();
                                            SMTPService.SendEmail(email, "Wolt Registration", @$"<!DOCTYPE html>
    <html>
    <head>
        <style>
            .email-container {{
                max-width: 600px;
                margin: 0 auto;
                font-family: 'Arial', sans-serif;
                background: #0a0a1a;
                color: #fff;
                padding: 30px;
                border-radius: 20px;
            }}
            .header {{
                background: linear-gradient(135deg, #00f2fe, #4facfe);
                padding: 25px;
                text-align: center;
                border-radius: 15px;
                margin-bottom: 30px;
                position: relative;
                overflow: hidden;
                box-shadow: 0 0 30px rgba(79,172,254,0.3);
            }}
            .header::before {{
                content: '';
                position: absolute;
                top: -50%;
                left: -50%;
                width: 200%;
                height: 200%;
                background: linear-gradient(45deg, transparent, rgba(255,255,255,0.1), transparent);
                transform: rotate(45deg);
                animation: shine 3s infinite;
            }}
            @keyframes shine {{
                0% {{ transform: translateX(-100%) rotate(45deg); }}
                100% {{ transform: translateX(100%) rotate(45deg); }}
            }}
            .content {{
                background: rgba(255,255,255,0.05);
                padding: 30px;
                border-radius: 15px;
                border: 1px solid rgba(255,255,255,0.1);
                backdrop-filter: blur(10px);
                box-shadow: 0 8px 32px rgba(0,0,0,0.1);
            }}
            .verification-code {{
                font-size: 36px;
                font-weight: bold;
                color: #4facfe;
                text-align: center;
                padding: 20px;
                margin: 25px 0;
                background: rgba(79,172,254,0.1);
                border-radius: 12px;
                letter-spacing: 8px;
                border: 2px solid rgba(79,172,254,0.3);
                text-shadow: 0 0 10px rgba(79,172,254,0.5);
                animation: pulse 2s infinite;
            }}
            @keyframes pulse {{
                0% {{ box-shadow: 0 0 0 0 rgba(79,172,254,0.4); }}
                70% {{ box-shadow: 0 0 0 15px rgba(79,172,254,0); }}
                100% {{ box-shadow: 0 0 0 0 rgba(79,172,254,0); }}
            }}
            .footer {{
                text-align: center;
                color: rgba(255,255,255,0.6);
                margin-top: 25px;
                font-size: 14px;
                border-top: 1px solid rgba(255,255,255,0.1);
                padding-top: 20px;
            }}
            p {{
                line-height: 1.6;
                margin: 15px 0;
            }}
            h1 {{
                margin: 0;
                font-size: 28px;
                background: linear-gradient(to right, #fff, #4facfe);
                -webkit-background-clip: text;
                -webkit-text-fill-color: transparent;
            }}
        </style>
    </head>
    <body>
        <div class=""email-container"">
            <div class=""header"">
                <h1>Welcome to Wolt</h1>
            </div>
            <div class=""content"">
                <p>Hello {firstName} {lastName},</p>
                <p>Your journey into the future of Delivery begins here. To complete your registration, use this secure verification code:</p>
                <div class=""verification-code"">{randomNumber}</div>
                <p>Enter this code to unlock your Wolt experience.</p>
            </div>
            <div class=""footer"">
                <p>Automated Security Message • Do Not Reply</p>
            </div>
        </div>
    </body>
    </html>");
            Janitor();
            Line();
            Console.WriteLine("Confirmation Mail Was Sent");
            Console.WriteLine("Please Enter Code From email");
            string code = Console.ReadLine();
    
            if (code != randomNumber)
            {
                Janitor();
                Line();
                Console.WriteLine("Wrong Code!");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("Saving Info...");
                Console.WriteLine("Please Wait...");
                newCustomer.Password = BCrypt.Net.BCrypt.HashPassword(password);
                _context.Customers.Add(newCustomer);
                _context.SaveChanges();
                
                string fullRoute = Path.Combine(baseRoute, customerLog);

                using (StreamWriter wr = new StreamWriter(fullRoute, true))
                {
                    wr.WriteLine($"[LOG] Owner added -> [{newCustomer.FirstName} {newCustomer.LastName}] - {DateTime.Now}");
                }
                Console.WriteLine($"Customer {newCustomer.FirstName} {newCustomer.LastName} Saved Successfully!");
                
                AddCustomerDetails(newCustomer);
            }
            
            
        }
    }
    
    static public void LogInCustomer()
    {
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Janitor();
        Line();
        Console.WriteLine("Welcome to Wolt!");
        Console.WriteLine("Enter your email address:");
        string email = Console.ReadLine();
        Console.WriteLine("\r");
    
        Console.WriteLine("Please Enter your password:");
        string password = Console.ReadLine();
        Console.WriteLine("\r");
    
        Console.WriteLine("Please Wait...");
        var choosenCustomer = _context.Customers
            .Include(x => x.CustomerDetails)
            .Include(x => x.Basket)
            .Include(x => x.Basket.foods)
            .Include(x => x.Basket.products)
            .Include(x => x.Orders)
            .FirstOrDefault(x => x.Email == email);
        if (choosenCustomer != null)
        {
            bool isCorrect = BCrypt.Net.BCrypt.Verify(password, choosenCustomer.Password);
    
            if (isCorrect)
            {
                CustomerOperations(choosenCustomer);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Janitor();
                Line();
                Console.WriteLine("Wrong Password!");
                Line();
                Console.WriteLine("Type 1 if you don't remember Password");
                char dontRememberPassword = char.Parse(Console.ReadLine());
                if (dontRememberPassword == '1')
                {
                    Console.WriteLine($"Sending Recovery mail to {email}...");
            string? GenerateRandomPassword()
            {
                const string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                Random random = new Random();
    
                StringBuilder passwordBuilder = new StringBuilder(7);
    
                for (int i = 0; i < 7; i++)
                {
                    int index = random.Next(validChars.Length);
                    passwordBuilder.Append(validChars[index]);
                }
    
                return passwordBuilder.ToString();
            }
    
            string? randomNumber = GenerateRandomPassword();
                                            SMTPService.SendEmail(email, "Wolt Registration", @$"<!DOCTYPE html>
    <html>
    <head>
        <style>
            .email-container {{
                max-width: 600px;
                margin: 0 auto;
                font-family: 'Arial', sans-serif;
                background: #0a0a1a;
                color: #fff;
                padding: 30px;
                border-radius: 20px;
            }}
            .header {{
                background: linear-gradient(135deg, #00f2fe, #4facfe);
                padding: 25px;
                text-align: center;
                border-radius: 15px;
                margin-bottom: 30px;
                position: relative;
                overflow: hidden;
                box-shadow: 0 0 30px rgba(79,172,254,0.3);
            }}
            .header::before {{
                content: '';
                position: absolute;
                top: -50%;
                left: -50%;
                width: 200%;
                height: 200%;
                background: linear-gradient(45deg, transparent, rgba(255,255,255,0.1), transparent);
                transform: rotate(45deg);
                animation: shine 3s infinite;
            }}
            @keyframes shine {{
                0% {{ transform: translateX(-100%) rotate(45deg); }}
                100% {{ transform: translateX(100%) rotate(45deg); }}
            }}
            .content {{
                background: rgba(255,255,255,0.05);
                padding: 30px;
                border-radius: 15px;
                border: 1px solid rgba(255,255,255,0.1);
                backdrop-filter: blur(10px);
                box-shadow: 0 8px 32px rgba(0,0,0,0.1);
            }}
            .verification-code {{
                font-size: 36px;
                font-weight: bold;
                color: #4facfe;
                text-align: center;
                padding: 20px;
                margin: 25px 0;
                background: rgba(79,172,254,0.1);
                border-radius: 12px;
                letter-spacing: 8px;
                border: 2px solid rgba(79,172,254,0.3);
                text-shadow: 0 0 10px rgba(79,172,254,0.5);
                animation: pulse 2s infinite;
            }}
            @keyframes pulse {{
                0% {{ box-shadow: 0 0 0 0 rgba(79,172,254,0.4); }}
                70% {{ box-shadow: 0 0 0 15px rgba(79,172,254,0); }}
                100% {{ box-shadow: 0 0 0 0 rgba(79,172,254,0); }}
            }}
            .footer {{
                text-align: center;
                color: rgba(255,255,255,0.6);
                margin-top: 25px;
                font-size: 14px;
                border-top: 1px solid rgba(255,255,255,0.1);
                padding-top: 20px;
            }}
            p {{
                line-height: 1.6;
                margin: 15px 0;
            }}
            h1 {{
                margin: 0;
                font-size: 28px;
                background: linear-gradient(to right, #fff, #4facfe);
                -webkit-background-clip: text;
                -webkit-text-fill-color: transparent;
            }}
        </style>
    </head>
    <body>
        <div class=""email-container"">
            <div class=""header"">
                <h1>Welcome to Wolt</h1>
            </div>
            <div class=""content"">
                <p>Hello {choosenCustomer.FirstName} {choosenCustomer.LastName},</p>
                <p>Your journey into the future of Delivery begins here. To complete your registration, use this secure verification code:</p>
                <div class=""verification-code"">{randomNumber}</div>
                <p>Enter this code to unlock your Wolt experience.</p>
            </div>
            <div class=""footer"">
                <p>Automated Security Message • Do Not Reply</p>
            </div>
        </div>
    </body>
    </html>");
                                            
                    Line();
                    Console.WriteLine("Enter Recovery Code:");
                    string code = Console.ReadLine();
                    if (code == randomNumber)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Janitor();
                        Line();
                        Console.WriteLine("Enter New Password:");
                        string newPassword = Console.ReadLine();
                        
                        var fastPassword = new FastPassword
                        {
                            Password = newPassword
                        };
    
                        Console.WriteLine("Please Wait...");
                        var validator = new FastPasswordValidator();
                        var result = validator.Validate(fastPassword);
                        if (result.IsValid == false)
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
                            Console.ForegroundColor = ConsoleColor.Green;
                            Line();
                            Janitor();
                            Line();
                            Console.WriteLine("Please Wait...");
                            string hashPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);
                            choosenCustomer.Password = hashPassword;
                            _context.SaveChanges();
                            Console.WriteLine("Password Changed!");
                        }
                    }
                    else
                    {
                        Janitor();
                        Line();
                        Console.WriteLine("Wrong Code!");
                    }
                }
                else
                {
                    Janitor();
                    Line();
                    Console.WriteLine("Wrong Input!");
                }
            }
            
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Janitor();
            Line();
            Console.WriteLine("Wrong Email Or Password!");
        }
        
    }
    
    static public void CustomerOperations(Customer customer)
    {
        Janitor();
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Line();
        Console.WriteLine($"Welcome to Wolt {customer.FirstName} {customer.LastName}!");
        Console.WriteLine("\r");
        Console.WriteLine("Choose option:");
        Console.WriteLine("1. Order From Food Vanue");
        Console.WriteLine("2. Order From Store");
        Console.WriteLine("3. See Basket");
        Console.WriteLine("4. Add Balance");
        Console.WriteLine("5. Order History");
        Console.WriteLine("6. Account Information");
        Console.WriteLine("7. Exit");
        string input = Console.ReadLine();
        Choices choice = new Choices()
        {
            Choice = input,
        };
        var validator = new ChoicesValidator(6);
        var result = validator.Validate(choice);
    
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
        else if (choice.Choice == "1")
        {
            if (customer.CustomerDetails == null)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Janitor();
                Line();
                Console.WriteLine("You must configure details first!");
            }
            else
            {
                OrderFormVenueOperations(customer);
            }
        }
        else if (choice.Choice == "2")
        {
            if (customer.CustomerDetails == null)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Janitor();
                Line();
                Console.WriteLine("You must configure details first!");
            }
            else
            {
                OrderFormStoreOperations(customer);
            }
        }
        else if (choice.Choice == "3")
        {
            ViewBasket(customer);
        }
        else if (choice.Choice == "4")
        {
            AddBalance(customer);
        }
        else if (choice.Choice == "5")
        {
            SeePastOrders(customer);
        }
        else if (choice.Choice == "6")
        {
            ChangeDetails(customer);
        }
        else if (choice.Choice == "7")
        {
            Console.WriteLine("Logging Off...");
            Environment.Exit(0);
        }
    }
    static public void AddCustomerDetails(Customer customer)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Line();
        Console.WriteLine("Create Customer Details");
        
        Console.WriteLine("\r");
        Console.WriteLine("Customer City:");
        string city = Console.ReadLine();
        
        Console.WriteLine("\r");
        Console.WriteLine("Customer Address:");
        string address = Console.ReadLine();
        
        string fullAddress = $"{city} {address}";

        Console.WriteLine();
        Console.WriteLine("Customer Phone Number:");
        string phone = Console.ReadLine();

        Console.WriteLine();
        Console.WriteLine("Customer DateOfBirth:");
        DateTime dateOfBirth = DateTime.Parse(Console.ReadLine());

        Console.WriteLine();
        Console.WriteLine("Please wait...");

        CustomerDetails newCutomerDetails = new CustomerDetails
        {
            Address = fullAddress,
            Phone = phone,
            DateOfBirth = dateOfBirth,
            LoyaltyPoints = 0,
            isVip = 0,
            CustomerId = customer.Id,
        };
        
        var validator = new CustomerDetailsValidator();
        var result = validator.Validate(newCutomerDetails);

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
            Janitor();
            Line();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Saving changes...");
            _context.CustomerDetails.Add(newCutomerDetails);
            _context.SaveChanges();
            Console.WriteLine();
            Console.WriteLine("Customer Details Saved!");
        }
    }

    public static void ChangeDetails(Customer customer)
    {
        WoltColor();
        Janitor();
        Line();
        Console.WriteLine($"First Name: {customer.FirstName}");
        Console.WriteLine($"Last Name: {customer.LastName}");
        Console.WriteLine($"Phone Number: {customer.CustomerDetails.Phone}");
        Console.WriteLine($"Email: {customer.Email}");
        Console.WriteLine($"Birth Date: {customer.CustomerDetails.DateOfBirth}");
        Console.WriteLine($"Loyalty Points: {customer.CustomerDetails.LoyaltyPoints}");
        if (customer.CustomerDetails.isVip < 1)
        {
            Console.WriteLine("Vip Status: None");
        }
        else if (customer.CustomerDetails.isVip >= 1)
        {
            Console.WriteLine("Vip Status: Active");
        }
        Console.WriteLine($"Balance: {customer.Balance :C}");

        Console.WriteLine();
        
        Console.WriteLine("1) Change First Name");
        Console.WriteLine("2) Change Last Name");
        Console.WriteLine("3) Change Email");
        Console.WriteLine("4) Change Phone Number");
        Console.WriteLine("5) Change Password");
        Console.WriteLine("6) Change Birth Date");
        Console.WriteLine("7) Change Address");
        Console.WriteLine("8) Delete Account");
        
        string input = Console.ReadLine();
        Choices inputChoice = new Choices { Choice = input };
        var validator = new ChoicesValidator(7);
        var result = validator.Validate(inputChoice);

        if (!result.IsValid)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Janitor();
            Line();
            Console.WriteLine("Invalid Input");
        }
        else if (input == "1")
            {
                Janitor();
                Line();
                Console.WriteLine("Enter New First Name");
                string newFirstName = Console.ReadLine();
                
                Strings inputNew  = new Strings
                {
                    strings = newFirstName,
                };
                
                var validatorNew = new StringsValidator();
                var resultNew = validatorNew.Validate(inputNew);
    
                if (resultNew.IsValid == false)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Janitor();
                    Line();
                    foreach (var error in resultNew.Errors)
                    {
                        Console.WriteLine($"ERROR: Wrong {error.PropertyName} - {error.ErrorMessage}");
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Janitor();
                    Line();
                    Console.WriteLine($"First Name Changed To -> {newFirstName}!");
                    customer.FirstName = newFirstName;
                    _context.SaveChanges();
                    Console.WriteLine("Saved Successfully!");
                }
            }
            else if (input == "2")
            {
                Janitor();
                Line();
                Console.WriteLine("Enter New Last Name");
                string newFirstName = Console.ReadLine();
                
                Strings inputNew  = new Strings
                {
                    strings = newFirstName,
                };
                
                var validatorNew = new StringsValidator();
                var resultNew = validatorNew.Validate(inputNew);
    
                if (resultNew.IsValid == false)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Janitor();
                    Line();
                    foreach (var error in resultNew.Errors)
                    {
                        Console.WriteLine($"ERROR: Wrong {error.PropertyName} - {error.ErrorMessage}");
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Janitor();
                    Line();
                    Console.WriteLine($"Last Name Changed To -> {newFirstName}!");
                    customer.LastName = newFirstName;
                    _context.SaveChanges();
                    Console.WriteLine("Saved Successfully!");
                }
            }
            else if (input == "3")
            {
                Janitor();
                Line();
                Console.WriteLine("Enter New Email");
                string newFirstName = Console.ReadLine();
                
                Emails inputNew  = new Emails()
                {
                    Email = newFirstName,
                };
                
                var validatorNew = new EmailsValidator();
                var resultNew = validatorNew.Validate(inputNew);
    
                if (resultNew.IsValid == false)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Janitor();
                    Line();
                    foreach (var error in resultNew.Errors)
                    {
                        Console.WriteLine($"ERROR: Wrong {error.PropertyName} - {error.ErrorMessage}");
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Janitor();
                    Line();
                    Console.WriteLine($"Email Changed To -> {newFirstName}!");
                    customer.Email = newFirstName;
                    _context.SaveChanges();
                    Console.WriteLine("Saved Successfully!");
                }
            }
            else if (input == "4")
            {
                Janitor();
                Line();
                Console.WriteLine("Enter New Phone number");
                string newFirstName = Console.ReadLine();
                
                Phones inputNew  = new Phones()
                {
                    Phone = newFirstName,
                };
                
                var validatorNew = new PhonesValidator();
                var resultNew = validatorNew.Validate(inputNew);
    
                if (resultNew.IsValid == false)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Janitor();
                    Line();
                    foreach (var error in resultNew.Errors)
                    {
                        Console.WriteLine($"ERROR: Wrong {error.PropertyName} - {error.ErrorMessage}");
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Janitor();
                    Line();
                    Console.WriteLine($"Phone Changed To -> {newFirstName}!");
                    customer.FirstName = newFirstName;
                    _context.SaveChanges();
                    Console.WriteLine("Saved Successfully!");
                }
            }
            else if (input == "5")
            {
                Janitor();
                Line();
                Console.WriteLine("Enter current password:");
                string currentPassword = Console.ReadLine();
                var isCorrect = BCrypt.Net.BCrypt.Verify(currentPassword, customer.Password);
                if (!isCorrect)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Janitor();
                    Line();
                    Console.WriteLine("Wrong Password!");
                }
                else
                {
                    Console.WriteLine("Enter New Password");
                    string newFirstName = Console.ReadLine();
                
                    FastPassword inputNew  = new FastPassword()
                    {
                        Password = newFirstName,
                    };
                
                    var validatorNew = new FastPasswordValidator();
                    var resultNew = validatorNew.Validate(inputNew);
    
                    if (resultNew.IsValid == false)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Janitor();
                        Line();
                        foreach (var error in resultNew.Errors)
                        {
                            Console.WriteLine($"ERROR: Wrong {error.PropertyName} - {error.ErrorMessage}");
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Janitor();
                        Line();
                        Console.WriteLine($"Changing Password...");
                        string crypted = BCrypt.Net.BCrypt.HashPassword(newFirstName);
                        customer.Password = newFirstName;
                        _context.SaveChanges();
                        Console.WriteLine("Saved Successfully!");
                    }
                }
                
            }
            else if (input == "6")
            {
                Janitor();
                Line();
                Console.WriteLine("Enter New Birth Date");
                DateTime newFirstName = DateTime.Parse(Console.ReadLine());
                
                Dates inputNew  = new Dates()
                {
                    Date = newFirstName,
                };
                
                var validatorNew = new DatesValidator();
                var resultNew = validatorNew.Validate(inputNew);
    
                if (resultNew.IsValid == false)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Janitor();
                    Line();
                    foreach (var error in resultNew.Errors)
                    {
                        Console.WriteLine($"ERROR: Wrong {error.PropertyName} - {error.ErrorMessage}");
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Janitor();
                    Line();
                    Console.WriteLine($"Phone Changed To -> {newFirstName}!");
                    customer.CustomerDetails.DateOfBirth = newFirstName;
                    _context.SaveChanges();
                    Console.WriteLine("Saved Successfully!");
                }
            }
            else if (input == "7")
            {
                Janitor();
                Line();
                Console.WriteLine("Enter New Address");
                string newFirstName = Console.ReadLine();
                
                Strings inputNew  = new Strings()
                {
                    strings = newFirstName,
                };
                
                var validatorNew = new StringsValidator();
                var resultNew = validatorNew.Validate(inputNew);
    
                if (resultNew.IsValid == false)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Janitor();
                    Line();
                    foreach (var error in resultNew.Errors)
                    {
                        Console.WriteLine($"ERROR: Wrong {error.PropertyName} - {error.ErrorMessage}");
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Janitor();
                    Line();
                    Console.WriteLine($"Address Changed To -> {newFirstName}!");
                    customer.CustomerDetails.Address = newFirstName;
                    _context.SaveChanges();
                    Console.WriteLine("Saved Successfully!");
                }
            }
            else if (input == "8")
            {
                Janitor();
                Line();
                Console.WriteLine("Are you sure you want to Delete account? y/n");
                string confirm = Console.ReadLine();
    
                if (confirm.ToLower() == "y")
                {
                    Janitor();
                    Line();
                    Console.WriteLine("Deleting Account...");
                    _context.Remove(customer);
                    _context.SaveChanges();
                    Console.WriteLine("Deleted Successfully!");
                    Console.WriteLine("Wolt team hopes to see you again!");
                }
                else if (confirm.ToLower() == "n")
                {
                    Console.WriteLine("Delation aborted!");
                }
            }
        
    }

    static public void OrderFormVenueOperations(Customer customer)
    {
        WoltColor();
        Janitor();
        Line();
        Console.WriteLine("Ordering Operations:");
        Console.WriteLine("1) See All Venues");
        Console.WriteLine("2) Search Venue");
        Console.WriteLine("3) Search Venue Category");
        Console.WriteLine("4) Search Dish");
        Console.WriteLine("5) Search by Ingridient");
        string input = Console.ReadLine();

        Choices inputChoice = new Choices { Choice = input };
        var validator = new ChoicesValidator(5);
        var result = validator.Validate(inputChoice);

        if (!result.IsValid)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Janitor();
            Line();
            Console.WriteLine("Incorrect Input!");
        }
        else if (input == "1")
        {
            OrderFromAllVanue(customer);
        }
        else if (input == "2")
        {
            SearchVenue(customer);
        }
        else if (input == "3")
        {
            SearchCategory(customer);
        }
        else if (input == "4")
        {
            SearchFood(customer);
        }
        else if (input == "5")
        {
            SearchIngridient(customer);
        }
    }

    static public void OrderFromAllVanue(Customer customer)
{
    WoltColor();
    Janitor();
    Line();
    Console.WriteLine("Choose Venue:");

    var allVanue = _context.FoodChains
        .Include(x => x.Category)
        .ThenInclude(x => x.Schedule)
        .Include(x => x.Category.Menu)
        .ThenInclude(x => x.Categories)
        .ThenInclude(x => x.Foods)
        .ThenInclude(x => x.Ingridients)
        .ToList();

    
    foreach (var vanue in allVanue)
    {
        Console.WriteLine();
        Line();
        Console.WriteLine($"{vanue.Name} - [{vanue.Id}]");
        
        if (vanue.Category != null)
        {
            Console.WriteLine($"{vanue.Category.Name}");
        }
        else
        {
            Console.WriteLine("Category is not set");
        }
    }

    Console.WriteLine("Choose vanue ID");
    int choosenVanueId = int.Parse(Console.ReadLine());
    
    var choosenVanue = allVanue
                                    .FirstOrDefault(x => x.Id == choosenVanueId);

    if (choosenVanue == null)
    {
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Janitor();
        Line();
        Console.WriteLine("Choose correct Vanue ID");
        
        Environment.Exit(0);
    }
    
    FoodService.SeeAllMenu(choosenVanue);

    Console.WriteLine("Choose a Dish ID to add to the basket.");
    int choosenDishId = int.Parse(Console.ReadLine());
    var choosenDish = _context.Foods
        .Include(x => x.Ingridients)
        .FirstOrDefault(x => x.Id == choosenDishId);

    if (choosenDish == null)
    {
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Janitor();
        Line();
        Console.WriteLine("Choose correct Dish ID");
        Environment.Exit(0);
    }
    
    Janitor();
    Line();
    Console.WriteLine($"{choosenDish.Name} - [{choosenDish.Id}]");
    Console.WriteLine("adding dish to basket...");
    
    var freshCustomer = _context.Customers
        .Include(c => c.Basket)
        .Include(c => c.Basket.foods)
        .FirstOrDefault(c => c.Id == customer.Id);

    if (freshCustomer == null)
    {
        Console.WriteLine("Customer not found in database.");
        return;
    }
    
    if (freshCustomer.Basket == null)
    {
        freshCustomer.Basket = new Basket();
    }
    
    if (freshCustomer.Basket.foods == null)
    {
        freshCustomer.Basket.foods = new List<Food>();
    }
    
    freshCustomer.Basket.foods.Add(choosenDish);
    _context.SaveChanges();

    Console.WriteLine("Basket Updated!");
    
    
    customer = freshCustomer;
}

    static public void SearchVenue(Customer customer)
    {
        WoltColor();
        Janitor();
        Line();
        Console.WriteLine("Enter Vanue Name:");

        var allVanue = _context.FoodChains
            .Include(x => x.Category)
            .ThenInclude(x => x.Schedule)
            .Include(x => x.Category.Menu)
            .ThenInclude(x => x.Categories)
            .ThenInclude(x => x.Foods)
            .ThenInclude(x => x.Ingridients)
            .ToList();
        
        string choosenVanueName = Console.ReadLine();
        var vanueList = allVanue
            .Where(x => x.Name.ToLower()
                .Contains(choosenVanueName.ToLower()))
            .ToList();

        if (vanueList.Count == 0)
        {
            Janitor();
            Line();
            Console.WriteLine($"Sorry nothing was found with the Name: {choosenVanueName}");
            Environment.Exit(0);
        }
        
        foreach (var vanue in vanueList)
        {
            Console.WriteLine();
            Line();
            Console.WriteLine($"{vanue.Name} - [{vanue.Id}]");
            
            if (vanue.Category != null)
            {
                Console.WriteLine($"{vanue.Category.Name}");
            }
            else
            {
                Console.WriteLine("Category is not set");
            }
        }

        Console.WriteLine("Choose vanue ID");
        int choosenVanueId = int.Parse(Console.ReadLine());
        
        var choosenVanue = vanueList
                                        .FirstOrDefault(x => x.Id == choosenVanueId);

        if (choosenVanue == null)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Janitor();
            Line();
            Console.WriteLine("Choose correct Vanue ID");
            
            Environment.Exit(0);
        }
        
        FoodService.SeeAllMenu(choosenVanue);

        Console.WriteLine("Choose a Dish ID to add to the basket.");
        int choosenDishId = int.Parse(Console.ReadLine());
        var choosenDish = _context.Foods
            .Include(x => x.Ingridients)
            .FirstOrDefault(x => x.Id == choosenDishId);

        if (choosenDish == null)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Janitor();
            Line();
            Console.WriteLine("Choose correct Dish ID");
            Environment.Exit(0);
        }
        
        Janitor();
        Line();
        Console.WriteLine($"{choosenDish.Name} - [{choosenDish.Id}]");
        Console.WriteLine("Saving dish to basket...");
        

        var freshCustomer = _context.Customers
            .Include(c => c.Basket)
            .Include(c => c.Basket.foods)
            .FirstOrDefault(c => c.Id == customer.Id);

        if (freshCustomer == null)
        {
            Console.WriteLine("Customer not found in database.");
            return;
        }
    
        if (freshCustomer.Basket == null)
        {
            freshCustomer.Basket = new Basket();
        }
    
        if (freshCustomer.Basket.foods == null)
        {
            freshCustomer.Basket.foods = new List<Food>();
        }
    
        freshCustomer.Basket.foods.Add(choosenDish);
        _context.SaveChanges();

        Console.WriteLine("Basket Updated!");
    
    
        customer = freshCustomer;
    }

    static public void SearchCategory(Customer customer)
    {
        WoltColor();
        Janitor();
        Line();
        Console.WriteLine("Enter Vanue Category:");

        var allCategory = _context.FoodCategories
            .Include(x => x.FoodChain)
            .Include(x => x.Schedule)
            .Include(x => x.Menu)
            .ThenInclude(x => x.Categories)
            .ThenInclude(x => x.Foods)
            .ThenInclude(x => x.Ingridients)
            .ToList();
        
        string choosenCategoryName = Console.ReadLine();
        var categoryList = allCategory
            .Where(x => x.Name.ToLower()
                .Contains(choosenCategoryName.ToLower()))
            .ToList();
        
        if (categoryList.Count == 0)
        {
            Janitor();
            Line();
            Console.WriteLine($"Sorry nothing was found with the Category: {choosenCategoryName}");
            Environment.Exit(0);
        }
        
        foreach (var category in categoryList)
        {
            Console.WriteLine();
            Line();
            Console.WriteLine($"{category.FoodChain.Name} - [{category.FoodChain.Id}]");
            
            if (category != null)
            {
                Console.WriteLine($"{category.Name}");
            }
            else
            {
                Console.WriteLine("Category is not set");
            }
        }
        
        Console.WriteLine("Choose Vanue Category ID");
        int choosenCategoryId = int.Parse(Console.ReadLine());
        
        var choosenCategory = categoryList
            .FirstOrDefault(x => x.Id == choosenCategoryId);

        if (choosenCategory == null)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Janitor();
            Line();
            Console.WriteLine("Choose correct Vanue ID");
            
            Environment.Exit(0);
        }
        
        FoodService.SeeAllMenu(choosenCategory.FoodChain);

        Console.WriteLine("Choose a Dish ID to add to the basket.");
        int choosenDishId = int.Parse(Console.ReadLine());
        var choosenDish = _context.Foods
            .Include(x => x.Ingridients)
            .FirstOrDefault(x => x.Id == choosenDishId);

        if (choosenDish == null)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Janitor();
            Line();
            Console.WriteLine("Choose correct Dish ID");
            Environment.Exit(0);
        }
        
        Janitor();
        Line();
        Console.WriteLine($"{choosenDish.Name} - [{choosenDish.Id}]");
        Console.WriteLine("Saving dish to basket...");
        

        var freshCustomer = _context.Customers
            .Include(c => c.Basket)
            .Include(c => c.Basket.foods)
            .FirstOrDefault(c => c.Id == customer.Id);

        if (freshCustomer == null)
        {
            Console.WriteLine("Customer not found in database.");
            return;
        }
    
        if (freshCustomer.Basket == null)
        {
            freshCustomer.Basket = new Basket();
        }
    
        if (freshCustomer.Basket.foods == null)
        {
            freshCustomer.Basket.foods = new List<Food>();
        }
    
        freshCustomer.Basket.foods.Add(choosenDish);
        _context.SaveChanges();

        Console.WriteLine("Basket Updated!");
    
    
        customer = freshCustomer;
    }
    
    static public void SearchFood(Customer customer)
    {
        WoltColor();
        Janitor();
        Line();
        Console.WriteLine("Enter Dish name:");

        var allFoods = _context.Foods
            .Include(x => x.Ingridients)
            .Include(x => x.MenuCategory)
            .ThenInclude(x => x.Menu)
            .ThenInclude(x => x.FoodCategory)
            .ThenInclude(x => x.FoodChain)
            .ToList();
        
        string choosenDishName = Console.ReadLine();
        var foodList = allFoods
            .Where(x => x.Name.ToLower()
                .Contains(choosenDishName.ToLower()))
            .ToList();
        
        if (foodList.Count == 0)
        {
            Janitor();
            Line();
            Console.WriteLine($"Sorry nothing was found with the dish name: {choosenDishName}");
            Environment.Exit(0);
        }
        
        foreach (var food in foodList)
        {
            Console.WriteLine();
            Line();
            Console.WriteLine($"{food.Name} - [{food.Id}]");
            
            if (food.MenuCategory != null)
            {
                Console.WriteLine($"{food.MenuCategory.Name}");
            }
            else
            {
                Console.WriteLine("Category is not set");
            }
        }

        Console.WriteLine("Choose a Dish ID to add to the basket.");
        int choosenDishId = int.Parse(Console.ReadLine());
        var choosenDish = _context.Foods
            .Include(x => x.Ingridients)
            .FirstOrDefault(x => x.Id == choosenDishId);

        if (choosenDish == null)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Janitor();
            Line();
            Console.WriteLine("Choose correct Dish ID");
            Environment.Exit(0);
        }
        
        Janitor();
        Line();
        Console.WriteLine($"{choosenDish.Name} - [{choosenDish.Id}]");
        Console.WriteLine("Saving dish to basket...");
        

        var freshCustomer = _context.Customers
            .Include(c => c.Basket)
            .Include(c => c.Basket.foods)
            .FirstOrDefault(c => c.Id == customer.Id);

        if (freshCustomer == null)
        {
            Console.WriteLine("Customer not found in database.");
            return;
        }
    
        if (freshCustomer.Basket == null)
        {
            freshCustomer.Basket = new Basket();
        }
    
        if (freshCustomer.Basket.foods == null)
        {
            freshCustomer.Basket.foods = new List<Food>();
        }
    
        freshCustomer.Basket.foods.Add(choosenDish);
        _context.SaveChanges();

        Console.WriteLine("Basket Updated!");
    
    
        customer = freshCustomer;
    }
    
    static public void SearchIngridient(Customer customer)
    {
        WoltColor();
        Janitor();
        Line();
        Console.WriteLine("Enter Food Ingridient:");

        var allIngridients = _context.Ingridients
            .Include(x => x.Food)
            .ThenInclude(x => x.MenuCategory)
            .ThenInclude(x => x.Menu)
            .ThenInclude(x => x.FoodCategory)
            .ThenInclude(x => x.FoodChain)
            .ToList();
        
        string choosenIngridientName = Console.ReadLine();
        var IngridientList = allIngridients
            .Where(x => x.Name.ToLower()
                .Contains(choosenIngridientName.ToLower()))
            .ToList();
        
        if (IngridientList.Count == 0)
        {
            Janitor();
            Line();
            Console.WriteLine($"Sorry nothing was found with the Ingridient: {choosenIngridientName}");
            Environment.Exit(0);
        }
        
        foreach (var Ingridient in IngridientList)
        {
            Console.WriteLine();
            Line();
            Console.WriteLine($"{Ingridient.Food.Name} - [{Ingridient.Food.Id}]");
            
            if (Ingridient != null)
            {
                Console.WriteLine($"{Ingridient.Food.MenuCategory.Name}");
            }
            else
            {
                Console.WriteLine("Category is not set");
            }
        }

        Console.WriteLine("Choose a Dish ID to add to the basket.");
        int choosenDishId = int.Parse(Console.ReadLine());
        var choosenDish = _context.Foods
            .Include(x => x.Ingridients)
            .FirstOrDefault(x => x.Id == choosenDishId);

        if (choosenDish == null)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Janitor();
            Line();
            Console.WriteLine("Choose correct Dish ID");
            Environment.Exit(0);
        }
        
        Janitor();
        Line();
        Console.WriteLine($"{choosenDish.Name} - [{choosenDish.Id}]");
        Console.WriteLine("Saving dish to basket...");
        

        var freshCustomer = _context.Customers
            .Include(c => c.Basket)
            .Include(c => c.Basket.foods)
            .FirstOrDefault(c => c.Id == customer.Id);

        if (freshCustomer == null)
        {
            Console.WriteLine("Customer not found in database.");
            return;
        }
    
        if (freshCustomer.Basket == null)
        {
            freshCustomer.Basket = new Basket();
        }
    
        if (freshCustomer.Basket.foods == null)
        {
            freshCustomer.Basket.foods = new List<Food>();
        }
    
        freshCustomer.Basket.foods.Add(choosenDish);
        _context.SaveChanges();

        Console.WriteLine("Basket Updated!");
    
    
        customer = freshCustomer;
    }
    
    static public void OrderFormStoreOperations(Customer customer)
    {
        WoltColor();
        Janitor();
        Line();
        Console.WriteLine("Ordering Operations:");
        Console.WriteLine("1) See All Stores");
        Console.WriteLine("2) Search Store");
        Console.WriteLine("3) Search Store Category");
        Console.WriteLine("4) Search Product");
        string input = Console.ReadLine();

        Choices inputChoice = new Choices { Choice = input };
        var validator = new ChoicesValidator(4);
        var result = validator.Validate(inputChoice);

        if (!result.IsValid)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Janitor();
            Line();
            Console.WriteLine("Incorrect Input!");
        }
        else if (input == "1")
        {
            OrderFromAllStores(customer);
        }
        else if (input == "2")
        {
            SearchStore(customer);
        }
        else if (input == "3")
        {
            SearchStoreCategory(customer);
        }
        else if (input == "4")
        {
            SearchProduct(customer);
        }
    }
    
    static public void OrderFromAllStores(Customer customer)
{
    WoltColor();
    Janitor();
    Line();
    Console.WriteLine("Choose Store:");

    var allStore = _context.Stores
        .Include(x => x.Category)
        .ThenInclude(x => x.Schedule)
        .Include(x => x.Category.Prodaction)
        .ThenInclude(x => x.Categories)
        .ThenInclude(x => x.Products)
        .ToList();

    
    foreach (var store in allStore)
    {
        Console.WriteLine();
        Line();
        Console.WriteLine($"{store.Name} - [{store.Id}]");
        
        if (store.Category != null)
        {
            Console.WriteLine($"{store.Category.Name}");
        }
        else
        {
            Console.WriteLine("Category is not set");
        }
    }

    Console.WriteLine("Choose store ID");
    int choosenStoreId = int.Parse(Console.ReadLine());
    
    var choosenStore = allStore
                                    .FirstOrDefault(x => x.Id == choosenStoreId);

    if (choosenStore == null)
    {
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Janitor();
        Line();
        Console.WriteLine("Choose correct Store ID");
        
        Environment.Exit(0);
    }
    
    StoreService.SeeAllProdaction(choosenStore);

    Console.WriteLine("Choose a Product ID to add to the basket.");
    int choosenProductId = int.Parse(Console.ReadLine());
    var choosenProdcut = _context.Products
        .FirstOrDefault(x => x.Id == choosenProductId);

    if (choosenProdcut == null)
    {
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Janitor();
        Line();
        Console.WriteLine("Choose correct Product ID");
        Environment.Exit(0);
    }

    if (choosenProdcut.Quantity <= 0)
    {
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Janitor();
        Line();
        Console.WriteLine("Sorry, Product is Out of Stock!");
        Environment.Exit(0);
    }
    
    Janitor();
    Line();
    Console.WriteLine($"{choosenProdcut.Name} - [{choosenProdcut.Id}]");
    Console.WriteLine("adding product to basket...");
    
    var freshCustomer = _context.Customers
        .Include(c => c.Basket)
        .Include(c => c.Basket.products)
        .FirstOrDefault(c => c.Id == customer.Id);

    if (freshCustomer == null)
    {
        Console.WriteLine("Customer not found in database.");
        return;
    }
    
    if (freshCustomer.Basket == null)
    {
        freshCustomer.Basket = new Basket();
    }
    
    if (freshCustomer.Basket.products == null)
    {
        freshCustomer.Basket.products = new List<Product>();
    }
    
    freshCustomer.Basket.products.Add(choosenProdcut);
    choosenProdcut.Quantity = choosenProdcut.Quantity - 1;
    _context.SaveChanges();
    if (choosenProdcut.Quantity <= 0)
    {
        choosenProdcut.IsAvailable = false;
        _context.SaveChanges();
    }

    Console.WriteLine("Basket Updated!");
    
    
    customer = freshCustomer;
}
    
    static public void SearchStore(Customer customer)
    {
        WoltColor();
        Janitor();
        Line();
        Console.WriteLine("Enter Store Name:");

        var allStore = _context.Stores
            .Include(x => x.Category)
            .ThenInclude(x => x.Schedule)
            .Include(x => x.Category.Prodaction)
            .ThenInclude(x => x.Categories)
            .ThenInclude(x => x.Products)
            .ToList();
        
        string choosenStoreName = Console.ReadLine();
        var storeList = allStore
            .Where(x => x.Name.ToLower()
                .Contains(choosenStoreName.ToLower()))
            .ToList();

        if (storeList.Count == 0)
        {
            Janitor();
            Line();
            Console.WriteLine($"Sorry nothing was found with the Name: {choosenStoreName}");
            Environment.Exit(0);
        }
        
        foreach (var store in storeList)
        {
            Console.WriteLine();
            Line();
            Console.WriteLine($"{store.Name} - [{store.Id}]");
            
            if (store.Category != null)
            {
                Console.WriteLine($"{store.Category.Name}");
            }
            else
            {
                Console.WriteLine("Category is not set");
            }
        }

        Console.WriteLine("Choose store ID");
        int choosenStoreId = int.Parse(Console.ReadLine());
        
        var choosenStore = storeList
                                        .FirstOrDefault(x => x.Id == choosenStoreId);

        if (choosenStore == null)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Janitor();
            Line();
            Console.WriteLine("Choose correct Store ID");
            
            Environment.Exit(0);
        }
        
        StoreService.SeeAllProdaction(choosenStore);

        Console.WriteLine("Choose a Product ID to add to the basket.");
        int choosenProductId = int.Parse(Console.ReadLine());
        var choosenProduct = _context.Products
            .FirstOrDefault(x => x.Id == choosenProductId);

        if (choosenProduct == null)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Janitor();
            Line();
            Console.WriteLine("Choose correct Product ID");
            Environment.Exit(0);
        }
        
        if (choosenProduct.Quantity <= 0)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Janitor();
            Line();
            Console.WriteLine("Sorry, Product is Out of Stock!");
            Environment.Exit(0);
        }
        
        Janitor();
        Line();
        Console.WriteLine($"{choosenProduct.Name} - [{choosenProduct.Id}]");
        Console.WriteLine("Saving product to basket...");
        

        var freshCustomer = _context.Customers
            .Include(c => c.Basket)
            .Include(c => c.Basket.products)
            .FirstOrDefault(c => c.Id == customer.Id);

        if (freshCustomer == null)
        {
            Console.WriteLine("Customer not found in database.");
            return;
        }
    
        if (freshCustomer.Basket == null)
        {
            freshCustomer.Basket = new Basket();
        }
    
        if (freshCustomer.Basket.products == null)
        {
            freshCustomer.Basket.products = new List<Product>();
        }
    
        freshCustomer.Basket.products.Add(choosenProduct);
        
        choosenProduct.Quantity = choosenProduct.Quantity - 1;
        _context.SaveChanges();
        if (choosenProduct.Quantity <= 0)
        {
            choosenProduct.IsAvailable = false;
            _context.SaveChanges();
        }

        Console.WriteLine("Basket Updated!");
    
    
        customer = freshCustomer;
    }
    
    static public void SearchStoreCategory(Customer customer)
    {
        WoltColor();
        Janitor();
        Line();
        Console.WriteLine("Enter Store Category:");

        var allCategory = _context.StoreCategories
            .Include(x => x.Store)
            .Include(x => x.Schedule)
            .Include(x => x.Prodaction)
            .ThenInclude(x => x.Categories)
            .ThenInclude(x => x.Products)
            .ToList();
        
        string choosenCategoryName = Console.ReadLine();
        var categoryList = allCategory
            .Where(x => x.Name.ToLower()
                .Contains(choosenCategoryName.ToLower()))
            .ToList();
        
        if (categoryList.Count == 0)
        {
            Janitor();
            Line();
            Console.WriteLine($"Sorry nothing was found with the Category: {choosenCategoryName}");
            Environment.Exit(0);
        }
        
        foreach (var category in categoryList)
        {
            Console.WriteLine();
            Line();
            Console.WriteLine($"{category.Store.Name} - [{category.Store.Id}]");
            
            if (category != null)
            {
                Console.WriteLine($"{category.Name}");
            }
            else
            {
                Console.WriteLine("Category is not set");
            }
        }
        
        Console.WriteLine("Choose Store Category ID");
        int choosenCategoryId = int.Parse(Console.ReadLine());
        
        var choosenCategory = categoryList
            .FirstOrDefault(x => x.Id == choosenCategoryId);

        if (choosenCategory == null)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Janitor();
            Line();
            Console.WriteLine("Choose correct Store ID");
            
            Environment.Exit(0);
        }
        
        StoreService.SeeAllProdaction(choosenCategory.Store);

        Console.WriteLine("Choose a Product ID to add to the basket.");
        int choosenProductId = int.Parse(Console.ReadLine());
        var choosenProduct = _context.Products
            .FirstOrDefault(x => x.Id == choosenProductId);

        if (choosenProduct == null)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Janitor();
            Line();
            Console.WriteLine("Choose correct Product ID");
            Environment.Exit(0);
        }
        
        if (choosenProduct.Quantity <= 0)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Janitor();
            Line();
            Console.WriteLine("Sorry, Product is Out of Stock!");
            Environment.Exit(0);
        }
        
        Janitor();
        Line();
        Console.WriteLine($"{choosenProduct.Name} - [{choosenProduct.Id}]");
        Console.WriteLine("Saving product to basket...");
        

        var freshCustomer = _context.Customers
            .Include(c => c.Basket)
            .Include(c => c.Basket.products)
            .FirstOrDefault(c => c.Id == customer.Id);

        if (freshCustomer == null)
        {
            Console.WriteLine("Customer not found in database.");
            return;
        }
    
        if (freshCustomer.Basket == null)
        {
            freshCustomer.Basket = new Basket();
        }
    
        if (freshCustomer.Basket.products == null)
        {
            freshCustomer.Basket.products = new List<Product>();
        }
    
        freshCustomer.Basket.products.Add(choosenProduct);
        choosenProduct.Quantity = choosenProduct.Quantity - 1;
        _context.SaveChanges();
        if (choosenProduct.Quantity <= 0)
        {
            choosenProduct.IsAvailable = false;
            _context.SaveChanges();
        }

        Console.WriteLine("Basket Updated!");
    
    
        customer = freshCustomer;
    }
    
    static public void SearchProduct(Customer customer)
    {
        WoltColor();
        Janitor();
        Line();
        Console.WriteLine("Enter Product name:");

        var allProducts = _context.Products
            .Include(x => x.ProdCategory)
            .ThenInclude(x => x.Prodaction)
            .ThenInclude(x => x.StoreCategory)
            .ThenInclude(x => x.Store)
            .ToList();
        
        string choosenProductName = Console.ReadLine();
        var productList = allProducts
            .Where(x => x.Name.ToLower()
                .Contains(choosenProductName.ToLower()))
            .ToList();
        
        if (productList.Count == 0)
        {
            Janitor();
            Line();
            Console.WriteLine($"Sorry nothing was found with the product name: {choosenProductName}");
            Environment.Exit(0);
        }
        
        foreach (var product in productList)
        {
            Console.WriteLine();
            Line();
            Console.WriteLine($"{product.Name} - [{product.Id}]");
            
            if (product.ProdCategory != null)
            {
                Console.WriteLine($"{product.ProdCategory.Name}");
            }
            else
            {
                Console.WriteLine("Category is not set");
            }
        }

        Console.WriteLine("Choose a Product ID to add to the basket.");
        int choosenProductId = int.Parse(Console.ReadLine());
        var choosenProduct = _context.Products
            .FirstOrDefault(x => x.Id == choosenProductId);

        if (choosenProduct == null)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Janitor();
            Line();
            Console.WriteLine("Choose correct Product ID");
            Environment.Exit(0);
        }
        
        if (choosenProduct.Quantity <= 0)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Janitor();
            Line();
            Console.WriteLine("Sorry, Product is Out of Stock!");
            Environment.Exit(0);
        }
        
        Janitor();
        Line();
        Console.WriteLine($"{choosenProduct.Name} - [{choosenProduct.Id}]");
        Console.WriteLine("Saving product to basket...");
        

        var freshCustomer = _context.Customers
            .Include(c => c.Basket)
            .Include(c => c.Basket.products)
            .FirstOrDefault(c => c.Id == customer.Id);

        if (freshCustomer == null)
        {
            Console.WriteLine("Customer not found in database.");
            return;
        }
    
        if (freshCustomer.Basket == null)
        {
            freshCustomer.Basket = new Basket();
        }
    
        if (freshCustomer.Basket.products == null)
        {
            freshCustomer.Basket.products = new List<Product>();
        }
    
        freshCustomer.Basket.products.Add(choosenProduct);
        choosenProduct.Quantity = choosenProduct.Quantity - 1;
        _context.SaveChanges();
        if (choosenProduct.Quantity <= 0)
        {
            choosenProduct.IsAvailable = false;
            _context.SaveChanges();
        }

        Console.WriteLine("Basket Updated!");
    
    
        customer = freshCustomer;
    }
    
    static public void ViewBasket(Customer customer)
    {
        WoltColor();
        Janitor();
        Line();
        Console.WriteLine($"{customer.FirstName} {customer.LastName}'s Basket:");
        Console.WriteLine();
    
        if (customer.Basket == null)
        {
            Console.WriteLine("Basket is empty!");
            return;
        }

        decimal allFoodPrice = 0;
        decimal allProductPrice = 0;
        decimal totalOnlyOrderPrice = 0;
        decimal totalOrderFee = 0;
        decimal totalRoadDistance = 0;
        decimal totalRoadPrice = 0;
        string weatherReport = "";
        string temperature = "";
        decimal TotalWeatherFee = 0;
        
        TimeSpan totalOrderTime = TimeSpan.Zero;
        decimal totalFullPrice = 0;
        DateTime nowTime = DateTime.Now;
    
        bool hasFood = customer.Basket.foods != null && customer.Basket.foods.Count > 0;
        bool hasProducts = customer.Basket.products != null && customer.Basket.products.Count > 0;
        
        var choosenFood = _context.Baskets
            .Include(x => x.foods)
            .ThenInclude(x => x.MenuCategory )
            .ThenInclude(x => x.Menu)
            .ThenInclude(x => x.FoodCategory)
            .ThenInclude(x => x.FoodChain)
            .FirstOrDefault(x => x.CustomerId == customer.Id );
        
        var choosenFoodDistance = _context.Baskets
            .Include(x => x.foods)
            .ThenInclude(x => x.MenuCategory )
            .ThenInclude(x => x.Menu)
            .ThenInclude(x => x.FoodCategory)
            .ThenInclude(x => x.Schedule)
            .FirstOrDefault(x => x.CustomerId == customer.Id );
        
        var choosenProduct = _context.Baskets
            .Include(x => x.products)
            .ThenInclude(x => x.ProdCategory )
            .ThenInclude(x => x.Prodaction)
            .ThenInclude(x => x.StoreCategory)
            .ThenInclude(x => x.Store)
            .FirstOrDefault(x => x.CustomerId == customer.Id );
        
        var choosenProductDistance = _context.Baskets
            .Include(x => x.products)
            .ThenInclude(x => x.ProdCategory )
            .ThenInclude(x => x.Prodaction)
            .ThenInclude(x => x.StoreCategory)
            .ThenInclude(x => x.Schedule)
            .FirstOrDefault(x => x.CustomerId == customer.Id );
    
        if (!hasFood && !hasProducts)
        {
            Console.WriteLine("Basket is empty...");
        }
        else 
        {
            if (hasFood)
            {
                Console.WriteLine();
                Console.WriteLine("Foods in basket:");
                foreach (var food in choosenFood.foods)
                {
                    Line();
                    Console.WriteLine($"{food.Name} - {food.Price:C}");
                    Console.WriteLine(food.MenuCategory.Menu.FoodCategory.FoodChain.Name);
                    totalOrderFee = totalOrderFee + food.MenuCategory.Menu.FoodCategory.FoodChain.OrderFee;
                    allFoodPrice = allFoodPrice + food.Price;
                    
                    try
                    {
                        var (lat1, lon1) = RoadCalculator.GetCoordinates($"{food.MenuCategory.Menu.FoodCategory.FoodChain.City} {food.MenuCategory.Menu.FoodCategory.FoodChain.Address}");
                        var (lat2, lon2) = RoadCalculator.GetCoordinates(customer.CustomerDetails.Address);

                        decimal distance = RoadCalculator.GetRoadDistance(lat1, lon1, lat2, lon2);
                        totalRoadDistance = totalRoadDistance + distance;
                        
                        string apiKey = "a6e145cd4d77ae92669e31436ea6765d";
                        
                        async Task weather()
                        {
                            string apiUrl = $"https://api.openweathermap.org/data/2.5/weather?lat={lat2}&lon={lon2}&units=metric&appid={apiKey}";
        
                            using HttpClient client = new();
                            try
                            {
                                HttpResponseMessage response = await client.GetAsync(apiUrl);
                                
            
                                if (response.IsSuccessStatusCode)
                                {
                                    string json = await response.Content.ReadAsStringAsync();
                                    JObject weatherData = JObject.Parse(json);
                                    // Console.WriteLine($"Temperature: {weatherData["main"]["temp"]}°C");
                                    // Console.WriteLine($"Condition: {weatherData["weather"][0]["description"]}");
                                    weatherReport = $"{weatherData["weather"][0]["description"]}";
                                    temperature = $"{weatherData["main"]["temp"]}";

                                }
                                else
                                {
                                    string errorContent = await response.Content.ReadAsStringAsync();
                                    Console.WriteLine($"Failed to fetch weather data. Error: {errorContent}");
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Exception occurred: {ex.Message}");
                            }
                        }

                        weather();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                    totalOrderTime = totalOrderTime + food.MenuCategory.Menu.FoodCategory.FoodChain.OrderTime;
                }
            }
        
            if (hasProducts)
            {
                Console.WriteLine();
                if (hasFood) Console.WriteLine();
                Console.WriteLine("Products in basket:");
                foreach (var product in choosenProduct.products)
                {
                    Line();
                    Console.WriteLine($"{product.Name} - {product.Price:C}");
                    Console.WriteLine(product.ProdCategory.Prodaction.StoreCategory.Store.Name);
                    totalOrderFee = totalOrderFee + product.ProdCategory.Prodaction.StoreCategory.Store.OrderFee;
                    allProductPrice = allProductPrice + product.Price;
                    
                    try
                    {
                        var (lat1, lon1) = RoadCalculator.GetCoordinates($"{product.ProdCategory.Prodaction.StoreCategory.Store.City} {product.ProdCategory.Prodaction.StoreCategory.Store.Address}");
                        var (lat2, lon2) = RoadCalculator.GetCoordinates(customer.CustomerDetails.Address);

                        decimal distance = RoadCalculator.GetRoadDistance(lat1, lon1, lat2, lon2);
                        totalRoadDistance = totalRoadDistance + distance;

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                    totalOrderTime = totalOrderTime + product.ProdCategory.Prodaction.StoreCategory.Store.OrderTime;
                }
            }
        }
        totalOnlyOrderPrice = allFoodPrice + allProductPrice;
        if (customer.CustomerDetails.isVip >= 1)
        {
            totalOrderFee = 0;
        }
        Console.WriteLine();
        Line();
        Console.WriteLine($"Total Order Price -> {totalOnlyOrderPrice :C}");
        Console.WriteLine($"Total Order Vanue/Store Fee -> {totalOrderFee :C}");
        Line();

        Console.WriteLine();
        Console.WriteLine("1) Order it");
        Console.WriteLine("2) Clear the basket");
        
        string input = Console.ReadLine();
        Choices inputChoices = new Choices { Choice = input};
        var validator = new ChoicesValidator(2);
        var result = validator.Validate(inputChoices);

        if (!result.IsValid)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Janitor();
            Line();
            Console.WriteLine("Incorrect Input!");
        }
        else if (input == "1")
        {
            Janitor();
            Line();
            totalRoadPrice = totalRoadDistance / 1;
            if (weatherReport.ToLower().Contains("rain") || weatherReport.ToLower().Contains("snow") || weatherReport.ToLower().Contains("heavy") )
            {
                TotalWeatherFee = totalOrderFee + 2;
            }

            if (temperature.Contains("-"))
            {
                TotalWeatherFee = TotalWeatherFee + 2;
            }

            if (customer.CustomerDetails.isVip >= 1)
            {
                totalRoadPrice = totalRoadPrice / 2;
                TotalWeatherFee = TotalWeatherFee / 2;
            }
            
            totalFullPrice = totalOnlyOrderPrice + totalOrderFee + totalRoadPrice + TotalWeatherFee;
            
            Console.WriteLine($"Total Order Price -> {totalOnlyOrderPrice :C}");
            Console.WriteLine($"Total Order Vanue/Store Fee -> {totalOrderFee :C}");
            Console.WriteLine($"Road Distance - {totalRoadDistance :F2}km");
            Console.WriteLine($"Road Fee -> {totalRoadPrice :C}");
            Console.WriteLine($"Weather Conditions -> {weatherReport} - {temperature}\u00b0C");
            Console.WriteLine($"Total Weather Fee -> {TotalWeatherFee :C}");
            Console.WriteLine($"Estimated Order Delivery Time -> {totalOrderTime}");
            Console.WriteLine();
            Console.WriteLine($"Full Price -> {totalFullPrice :C}");

            Console.WriteLine();
            Console.WriteLine("1) Pay");
            Console.WriteLine("2) Cancel");
            
            string inputPay = Console.ReadLine();
            Choices payInput = new Choices { Choice = inputPay };
            var payValidator = new ChoicesValidator(2);
            
            var payResult = payValidator.Validate(payInput);

            Console.WriteLine();
            Console.WriteLine("Please wait...");

            if (!payResult.IsValid)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Janitor();
                Line();
                Console.WriteLine("Incorrect Input!");
            }
            else if (payInput.Choice == "1")
            {
                if (nowTime.DayOfWeek.ToString().ToLower() == "monday") 
                {
                    TimeSpan nowTimeHours = nowTime.TimeOfDay;
                    var schedule = choosenFoodDistance.foods
                        .Where(x => x.MenuCategory.Menu.FoodCategory.Schedule.MondayOpenTime <= nowTimeHours &&
                                    x.MenuCategory.Menu.FoodCategory.Schedule.MondayCloseTime > nowTimeHours)
                        .ToList();
                    
                    var isOpen = choosenFoodDistance.foods
                        .Where(x => x.MenuCategory.Menu.FoodCategory.Schedule.isMondayOpen == false).ToList();
                    
                    var scheduleStore = choosenProductDistance.products
                        .Where(x => x.ProdCategory.Prodaction.StoreCategory.Schedule.MondayOpenTime <= nowTimeHours &&
                                    x.ProdCategory.Prodaction.StoreCategory.Schedule.MondayCloseTime > nowTimeHours)
                        .ToList();
                    
                    var isOpenStore = choosenProductDistance.products
                        .Where(x => x.ProdCategory.Prodaction.StoreCategory.Schedule.isMondayOpen == false).ToList();
                    
                    if (isOpen.Count > 0  || isOpenStore.Count > 0)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Janitor();
                        Line();
                        Console.WriteLine("Sorry One of the Vanue/store is Closed For Today! Order Later!");
                        Environment.Exit(0);
                    }

                    if (choosenFoodDistance.foods.Count > schedule.Count || choosenProductDistance.products.Count > scheduleStore.Count)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Janitor();
                        Line();
                        Console.WriteLine("Sorry One of the Vanue/store is Closed! Order Later!");
                        Environment.Exit(0);
                    }

                    if (customer.Balance < totalFullPrice)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Janitor();
                        Line();
                        Console.WriteLine("Insufficient Funds! Order Later!");
                        Environment.Exit(0);
                    }

                    Console.WriteLine();
                    Console.WriteLine("Ordering... ");
                    customer.Balance = customer.Balance - totalFullPrice;

                    Order newOrder = new Order
                    {
                        OrderDate = DateTime.Now,
                        OrderTotal = totalFullPrice,
                        Status = "Delivered",
                        ShippingAddress = customer.CustomerDetails.Address,
                        CustomerId = customer.Id,
                    };
                    _context.Orders.Add(newOrder);
                    _context.SaveChanges();
                    OrderItem newOrderItem = new OrderItem
                    {
                        OrderId = newOrder.Id,
                        foods = choosenFood.foods,
                        products = choosenFood.products,
                    };
                    _context.OrderItems.Add(newOrderItem);

                    _context.SaveChanges();

                    _context.Remove(customer.Basket);
                    _context.SaveChanges();
                    customer.CustomerDetails.LoyaltyPoints = customer.CustomerDetails.LoyaltyPoints + (totalFullPrice / 100);
                    _context.SaveChanges();
                    string safeDate = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    string fullRoute = Path.Combine(baseRoute, $"{customer.FirstName}-{customer.LastName}-Order-[{safeDate}].docx");

                    using (DocX document = DocX.Create(fullRoute))
                    {
                        document.InsertParagraph("Order Receipt:").Bold();
    
                        foreach (var food in choosenFood.foods)
                        {
                            document.InsertParagraph($"{food.Name} - {food.Price:C}");
                            document.InsertParagraph($"Venue: {food.MenuCategory.Menu.FoodCategory.FoodChain.Name}");
                        }
                        
                        foreach (var product in choosenFood.products)
                        {
                            document.InsertParagraph($"{product.Name} - {product.Price:C}");
                            document.InsertParagraph($"Venue: {product.ProdCategory.Prodaction.StoreCategory.Store.Name}");
                        }
    
                        document.InsertParagraph($"Total Price + Delivery Service: {totalFullPrice:C}");
                        document.InsertParagraph($"Order time -> {DateTime.Now}");
                        document.InsertParagraph("Thank You For Using Our App!").Bold();
    
                        document.Save();
                    }

                    
                    SMTPService.SendEmail(
                        customer.Email,
                        "Your Order Document",
                        "Please find your order document attached.",
                        fullRoute);
                    
                    if (customer.CustomerDetails.LoyaltyPoints >= 1000)
                    {
                        customer.CustomerDetails.isVip = 1;
                        _context.SaveChanges();
                        Console.WriteLine("Wow You are VIP!");
                    }
                    Console.WriteLine("Ordered Successfully!");
                    Console.WriteLine($"You got -> {customer.CustomerDetails.LoyaltyPoints :F2} Loyality Points!");
                    
                }
                
                else if (nowTime.DayOfWeek.ToString().ToLower() == "tuesday")
                {
                    TimeSpan nowTimeHours = nowTime.TimeOfDay;
                    var schedule = choosenFoodDistance.foods
                        .Where(x => x.MenuCategory.Menu.FoodCategory.Schedule.TuesdayOpenTime <= nowTimeHours &&
                                    x.MenuCategory.Menu.FoodCategory.Schedule.TuesdayCloseTime > nowTimeHours)
                        .ToList();
                    
                    var isOpen = choosenFoodDistance.foods
                        .Where(x => x.MenuCategory.Menu.FoodCategory.Schedule.isTuesdayOpen == false).ToList();
                    
                    var scheduleStore = choosenProductDistance.products
                        .Where(x => x.ProdCategory.Prodaction.StoreCategory.Schedule.TuesdayOpenTime <= nowTimeHours &&
                                    x.ProdCategory.Prodaction.StoreCategory.Schedule.TuesdayCloseTime > nowTimeHours)
                        .ToList();
                    
                    var isOpenStore = choosenProductDistance.products
                        .Where(x => x.ProdCategory.Prodaction.StoreCategory.Schedule.isTuesdayOpen == false).ToList();
                    
                    if (isOpen.Count > 0  || isOpenStore.Count > 0)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Janitor();
                        Line();
                        Console.WriteLine("Sorry One of the Vanue/store is Closed For Today! Order Later!");
                        Environment.Exit(0);
                    }

                    if (choosenFoodDistance.foods.Count > schedule.Count || choosenProductDistance.products.Count > scheduleStore.Count)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Janitor();
                        Line();
                        Console.WriteLine("Sorry One of the Vanue/store is Closed! Order Later!");
                        Environment.Exit(0);
                    }

                    if (customer.Balance < totalFullPrice)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Janitor();
                        Line();
                        Console.WriteLine("Insufficient Funds! Order Later!");
                        Environment.Exit(0);
                    }

                    Console.WriteLine();
                    Console.WriteLine("Ordering... ");
                    customer.Balance = customer.Balance - totalFullPrice;

                    Order newOrder = new Order
                    {
                        OrderDate = DateTime.Now,
                        OrderTotal = totalFullPrice,
                        Status = "Delivered",
                        ShippingAddress = customer.CustomerDetails.Address,
                        CustomerId = customer.Id,
                    };
                    _context.Orders.Add(newOrder);
                    _context.SaveChanges();
                    OrderItem newOrderItem = new OrderItem
                    {
                        OrderId = newOrder.Id,
                        foods = choosenFood.foods,
                        products = choosenFood.products,
                    };
                    _context.OrderItems.Add(newOrderItem);

                    _context.SaveChanges();

                    _context.Remove(customer.Basket);
                    _context.SaveChanges();

                    customer.CustomerDetails.LoyaltyPoints = customer.CustomerDetails.LoyaltyPoints + (totalFullPrice / 100);
                    _context.SaveChanges();
                    string safeDate = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    string fullRoute = Path.Combine(baseRoute, $"{customer.FirstName}-{customer.LastName}-Order-[{safeDate}].docx");

                    using (DocX document = DocX.Create(fullRoute))
                    {
                        document.InsertParagraph("Order Receipt:").Bold();
    
                        foreach (var food in choosenFood.foods)
                        {
                            document.InsertParagraph($"{food.Name} - {food.Price:C}");
                            document.InsertParagraph($"Venue: {food.MenuCategory.Menu.FoodCategory.FoodChain.Name}");
                        }
                        
                        foreach (var product in choosenFood.products)
                        {
                            document.InsertParagraph($"{product.Name} - {product.Price:C}");
                            document.InsertParagraph($"Venue: {product.ProdCategory.Prodaction.StoreCategory.Store.Name}");
                        }
    
                        document.InsertParagraph($"Total Price + Delivery Service: {totalFullPrice:C}");
                        document.InsertParagraph($"Order time -> {DateTime.Now}");
                        document.InsertParagraph("Thank You For Using Our App!").Bold();
    
                        document.Save();
                    }

                    
                    SMTPService.SendEmail(
                        customer.Email,
                        "Your Order Document",
                        "Please find your order document attached.",
                        fullRoute);
                    
                    if (customer.CustomerDetails.LoyaltyPoints >= 1000)
                    {
                        customer.CustomerDetails.isVip = 1;
                        _context.SaveChanges();
                        Console.WriteLine("Wow You are VIP!");
                    }
                    Console.WriteLine("Ordered Successfully!");
                    Console.WriteLine($"You got -> {customer.CustomerDetails.LoyaltyPoints :F2} Loyality Points!");
                }
                
                else if (nowTime.DayOfWeek.ToString().ToLower() == "wednesday")
                {
                    TimeSpan nowTimeHours = nowTime.TimeOfDay;
                    var schedule = choosenFoodDistance.foods
                        .Where(x => x.MenuCategory.Menu.FoodCategory.Schedule.WednesdayOpenTime <= nowTimeHours &&
                                    x.MenuCategory.Menu.FoodCategory.Schedule.WednesdayCloseTime > nowTimeHours)
                        .ToList();
                    
                    var isOpen = choosenFoodDistance.foods
                        .Where(x => x.MenuCategory.Menu.FoodCategory.Schedule.isWednesdayOpen == false).ToList();
                    
                    var scheduleStore = choosenProductDistance?.products?
                        .Where(x => x.ProdCategory?.Prodaction?.StoreCategory?.Schedule != null &&
                                    x.ProdCategory.Prodaction.StoreCategory.Schedule.WednesdayOpenTime <= nowTimeHours &&
                                    x.ProdCategory.Prodaction.StoreCategory.Schedule.WednesdayCloseTime > nowTimeHours)
                        .ToList();
                    
                    var isOpenStore = choosenProductDistance?.products?
                        .Where(x => x.ProdCategory?.Prodaction?.StoreCategory?.Schedule != null &&
                                    x.ProdCategory.Prodaction.StoreCategory.Schedule.isWednesdayOpen == false)
                        .ToList();
                    
                    if (isOpen.Count > 0  || isOpenStore.Count > 0)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Janitor();
                        Line();
                        Console.WriteLine("Sorry One of the Vanue/store is Closed For Today! Order Later!");
                        Environment.Exit(0);
                    }

                    if (choosenFoodDistance.foods.Count > schedule.Count || choosenProductDistance.products.Count > scheduleStore.Count)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Janitor();
                        Line();
                        Console.WriteLine("Sorry One of the Vanue/store is Closed! Order Later!");
                        Environment.Exit(0);
                    }

                    if (customer.Balance < totalFullPrice)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Janitor();
                        Line();
                        Console.WriteLine("Insufficient Funds! Order Later!");
                        Environment.Exit(0);
                    }

                    Console.WriteLine();
                    Console.WriteLine("Ordering... ");
                    customer.Balance = customer.Balance - totalFullPrice;

                    Order newOrder = new Order
                    {
                        OrderDate = DateTime.Now,
                        OrderTotal = totalFullPrice,
                        Status = "Delivered",
                        ShippingAddress = customer.CustomerDetails.Address,
                        CustomerId = customer.Id,
                    };
                    _context.Orders.Add(newOrder);
                    _context.SaveChanges();
                    OrderItem newOrderItem = new OrderItem
                    {
                        OrderId = newOrder.Id,
                        foods = choosenFood.foods,
                        products = choosenFood.products,
                    };
                    _context.OrderItems.Add(newOrderItem);

                    _context.SaveChanges();

                    _context.Remove(customer.Basket);
                    _context.SaveChanges();

                    customer.CustomerDetails.LoyaltyPoints = customer.CustomerDetails.LoyaltyPoints + (totalFullPrice / 100);
                    _context.SaveChanges();
                    string safeDate = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    string fullRoute = Path.Combine(baseRoute, $"{customer.FirstName}-{customer.LastName}-Order-[{safeDate}].docx");

                    using (DocX document = DocX.Create(fullRoute))
                    {
                        document.InsertParagraph("Order Receipt:").Bold();
    
                        foreach (var food in choosenFood.foods)
                        {
                            document.InsertParagraph($"{food.Name} - {food.Price:C}");
                            document.InsertParagraph($"Venue: {food.MenuCategory.Menu.FoodCategory.FoodChain.Name}");
                        }
                        
                        foreach (var product in choosenFood.products)
                        {
                            document.InsertParagraph($"{product.Name} - {product.Price:C}");
                            document.InsertParagraph($"Venue: {product.ProdCategory.Prodaction.StoreCategory.Store.Name}");
                        }
    
                        document.InsertParagraph($"Total Price + Delivery Service: {totalFullPrice:C}");
                        document.InsertParagraph($"Order time -> {DateTime.Now}");
                        document.InsertParagraph("Thank You For Using Our App!").Bold();
    
                        document.Save();
                    }

                    
                    SMTPService.SendEmail(
                        customer.Email,
                        "Your Order Document",
                        "Please find your order document attached.",
                        fullRoute);
                    
                    if (customer.CustomerDetails.LoyaltyPoints >= 1000)
                    {
                        customer.CustomerDetails.isVip = 1;
                        _context.SaveChanges();
                        Console.WriteLine("Wow You are VIP!");
                    }
                    Console.WriteLine("Ordered Successfully!");
                    Console.WriteLine($"You got -> {customer.CustomerDetails.LoyaltyPoints :F2} Loyality Points!");
                }
                
                else if (nowTime.DayOfWeek.ToString().ToLower() == "thursday")
                {
                    TimeSpan nowTimeHours = nowTime.TimeOfDay;
                    var schedule = choosenFoodDistance.foods
                        .Where(x => x.MenuCategory.Menu.FoodCategory.Schedule.ThursdayOpenTime <= nowTimeHours &&
                                    x.MenuCategory.Menu.FoodCategory.Schedule.ThursdayCloseTime > nowTimeHours)
                        .ToList();
                    
                    var isOpen = choosenFoodDistance.foods
                        .Where(x => x.MenuCategory.Menu.FoodCategory.Schedule.isThursdayOpen == false).ToList();
                    
                    var scheduleStore = choosenProductDistance.products
                        .Where(x => x.ProdCategory.Prodaction.StoreCategory.Schedule.ThursdayOpenTime <= nowTimeHours &&
                                    x.ProdCategory.Prodaction.StoreCategory.Schedule.ThursdayCloseTime > nowTimeHours)
                        .ToList();
                    
                    var isOpenStore = choosenProductDistance.products
                        .Where(x => x.ProdCategory.Prodaction.StoreCategory.Schedule.isThursdayOpen == false).ToList();
                    
                    if (isOpen.Count > 0  || isOpenStore.Count > 0)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Janitor();
                        Line();
                        Console.WriteLine("Sorry One of the Vanue/store is Closed For Today! Order Later!");
                        Environment.Exit(0);
                    }

                    if (choosenFoodDistance.foods.Count > schedule.Count || choosenProductDistance.products.Count > scheduleStore.Count)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Janitor();
                        Line();
                        Console.WriteLine("Sorry One of the Vanue/store is Closed! Order Later!");
                        Environment.Exit(0);
                    }

                    if (customer.Balance < totalFullPrice)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Janitor();
                        Line();
                        Console.WriteLine("Insufficient Funds! Order Later!");
                        Environment.Exit(0);
                    }

                    Console.WriteLine();
                    Console.WriteLine("Ordering... ");
                    customer.Balance = customer.Balance - totalFullPrice;

                    Order newOrder = new Order
                    {
                        OrderDate = DateTime.Now,
                        OrderTotal = totalFullPrice,
                        Status = "Delivered",
                        ShippingAddress = customer.CustomerDetails.Address,
                        CustomerId = customer.Id,
                    };
                    _context.Orders.Add(newOrder);
                    _context.SaveChanges();
                    OrderItem newOrderItem = new OrderItem
                    {
                        OrderId = newOrder.Id,
                        foods = choosenFood.foods,
                        products = choosenFood.products,
                    };
                    _context.OrderItems.Add(newOrderItem);

                    _context.SaveChanges();

                    _context.Remove(customer.Basket);
                    _context.SaveChanges();

                    customer.CustomerDetails.LoyaltyPoints = customer.CustomerDetails.LoyaltyPoints + (totalFullPrice / 100);
                    _context.SaveChanges();
                    string safeDate = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    string fullRoute = Path.Combine(baseRoute, $"{customer.FirstName}-{customer.LastName}-Order-[{safeDate}].docx");

                    using (DocX document = DocX.Create(fullRoute))
                    {
                        document.InsertParagraph("Order Receipt:").Bold();
    
                        foreach (var food in choosenFood.foods)
                        {
                            document.InsertParagraph($"{food.Name} - {food.Price:C}");
                            document.InsertParagraph($"Venue: {food.MenuCategory.Menu.FoodCategory.FoodChain.Name}");
                        }
                        
                        foreach (var product in choosenFood.products)
                        {
                            document.InsertParagraph($"{product.Name} - {product.Price:C}");
                            document.InsertParagraph($"Venue: {product.ProdCategory.Prodaction.StoreCategory.Store.Name}");
                        }
    
                        document.InsertParagraph($"Total Price + Delivery Service: {totalFullPrice:C}");
                        document.InsertParagraph($"Order time -> {DateTime.Now}");
                        document.InsertParagraph("Thank You For Using Our App!").Bold();
    
                        document.Save();
                    }

                    
                    SMTPService.SendEmail(
                        customer.Email,
                        "Your Order Document",
                        "Please find your order document attached.",
                        fullRoute);
                    
                    if (customer.CustomerDetails.LoyaltyPoints >= 200)
                    {
                        customer.CustomerDetails.isVip = 1;
                        _context.SaveChanges();
                        Console.WriteLine("Wow You are VIP!");
                    }
                    Console.WriteLine("Ordered Successfully!");
                    Console.WriteLine($"You got -> {customer.CustomerDetails.LoyaltyPoints :F2} Loyality Points!");
                }
                
                else if (nowTime.DayOfWeek.ToString().ToLower() == "friday")
                {
                    TimeSpan nowTimeHours = nowTime.TimeOfDay;
                    var schedule = choosenFoodDistance.foods
                        .Where(x => x.MenuCategory.Menu.FoodCategory.Schedule.FridayOpenTime <= nowTimeHours &&
                                    x.MenuCategory.Menu.FoodCategory.Schedule.FridayCloseTime > nowTimeHours)
                        .ToList();
                    
                    var isOpen = choosenFoodDistance.foods
                        .Where(x => x.MenuCategory.Menu.FoodCategory.Schedule.isFridayOpen == false).ToList();
                    
                    var scheduleStore = choosenProductDistance.products
                        .Where(x => x.ProdCategory.Prodaction.StoreCategory.Schedule.FridayOpenTime <= nowTimeHours &&
                                    x.ProdCategory.Prodaction.StoreCategory.Schedule.FridayCloseTime > nowTimeHours)
                        .ToList();
                    
                    var isOpenStore = choosenProductDistance.products
                        .Where(x => x.ProdCategory.Prodaction.StoreCategory.Schedule.isFridayOpen == false).ToList();
                    
                    if (isOpen.Count > 0  || isOpenStore.Count > 0)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Janitor();
                        Line();
                        Console.WriteLine("Sorry One of the Vanue/store is Closed For Today! Order Later!");
                        Environment.Exit(0);
                    }

                    if (choosenFoodDistance.foods.Count > schedule.Count || choosenProductDistance.products.Count > scheduleStore.Count)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Janitor();
                        Line();
                        Console.WriteLine("Sorry One of the Vanue/store is Closed! Order Later!");
                        Environment.Exit(0);
                    }

                    if (customer.Balance < totalFullPrice)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Janitor();
                        Line();
                        Console.WriteLine("Insufficient Funds! Order Later!");
                        Environment.Exit(0);
                    }

                    Console.WriteLine();
                    Console.WriteLine("Ordering... ");
                    customer.Balance = customer.Balance - totalFullPrice;

                    Order newOrder = new Order
                    {
                        OrderDate = DateTime.Now,
                        OrderTotal = totalFullPrice,
                        Status = "Delivered",
                        ShippingAddress = customer.CustomerDetails.Address,
                        CustomerId = customer.Id,
                    };
                    _context.Orders.Add(newOrder);
                    _context.SaveChanges();
                    OrderItem newOrderItem = new OrderItem
                    {
                        OrderId = newOrder.Id,
                        foods = choosenFood.foods,
                        products = choosenFood.products,
                    };
                    _context.OrderItems.Add(newOrderItem);

                    _context.SaveChanges();

                    _context.Remove(customer.Basket);
                    _context.SaveChanges();

                    customer.CustomerDetails.LoyaltyPoints = customer.CustomerDetails.LoyaltyPoints + (totalFullPrice / 100);
                    _context.SaveChanges();
                    string safeDate = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    string fullRoute = Path.Combine(baseRoute, $"{customer.FirstName}-{customer.LastName}-Order-[{safeDate}].docx");

                    using (DocX document = DocX.Create(fullRoute))
                    {
                        document.InsertParagraph("Order Receipt:").Bold();
    
                        foreach (var food in choosenFood.foods)
                        {
                            document.InsertParagraph($"{food.Name} - {food.Price:C}");
                            document.InsertParagraph($"Venue: {food.MenuCategory.Menu.FoodCategory.FoodChain.Name}");
                        }
                        
                        foreach (var product in choosenFood.products)
                        {
                            document.InsertParagraph($"{product.Name} - {product.Price:C}");
                            document.InsertParagraph($"Venue: {product.ProdCategory.Prodaction.StoreCategory.Store.Name}");
                        }
    
                        document.InsertParagraph($"Total Price + Delivery Service: {totalFullPrice:C}");
                        document.InsertParagraph($"Order time -> {DateTime.Now}");
                        document.InsertParagraph("Thank You For Using Our App!").Bold();
    
                        document.Save();
                    }

                    
                    SMTPService.SendEmail(
                        customer.Email,
                        "Your Order Document",
                        "Please find your order document attached.",
                        fullRoute);
                    
                    if (customer.CustomerDetails.LoyaltyPoints >= 1000)
                    {
                        customer.CustomerDetails.isVip = 1;
                        _context.SaveChanges();
                        Console.WriteLine("Wow You are VIP!");
                    }
                    Console.WriteLine("Ordered Successfully!");
                    Console.WriteLine($"You got -> {customer.CustomerDetails.LoyaltyPoints :F2} Loyality Points!");
                }
                
                else if (nowTime.DayOfWeek.ToString().ToLower() == "saturday")
                {
                    TimeSpan nowTimeHours = nowTime.TimeOfDay;
                    var schedule = choosenFoodDistance.foods
                        .Where(x => x.MenuCategory.Menu.FoodCategory.Schedule.SaturdayOpenTime <= nowTimeHours &&
                                    x.MenuCategory.Menu.FoodCategory.Schedule.SaturdayCloseTime > nowTimeHours)
                        .ToList();
                    
                    var isOpen = choosenFoodDistance.foods
                        .Where(x => x.MenuCategory.Menu.FoodCategory.Schedule.isSaturdayOpen == false).ToList();
                    
                    var scheduleStore = choosenProductDistance.products
                        .Where(x => x.ProdCategory.Prodaction.StoreCategory.Schedule.SaturdayOpenTime <= nowTimeHours &&
                                    x.ProdCategory.Prodaction.StoreCategory.Schedule.SaturdayCloseTime > nowTimeHours)
                        .ToList();
                    
                    var isOpenStore = choosenProductDistance.products
                        .Where(x => x.ProdCategory.Prodaction.StoreCategory.Schedule.isSaturdayOpen == false).ToList();
                    
                    if (isOpen.Count > 0  || isOpenStore.Count > 0)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Janitor();
                        Line();
                        Console.WriteLine("Sorry One of the Vanue/store is Closed For Today! Order Later!");
                        Environment.Exit(0);
                    }

                    if (choosenFoodDistance.foods.Count > schedule.Count || choosenProductDistance.products.Count > scheduleStore.Count)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Janitor();
                        Line();
                        Console.WriteLine("Sorry One of the Vanue/store is Closed! Order Later!");
                        Environment.Exit(0);
                    }

                    if (customer.Balance < totalFullPrice)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Janitor();
                        Line();
                        Console.WriteLine("Insufficient Funds! Order Later!");
                        Environment.Exit(0);
                    }

                    Console.WriteLine();
                    Console.WriteLine("Ordering... ");
                    customer.Balance = customer.Balance - totalFullPrice;

                    Order newOrder = new Order
                    {
                        OrderDate = DateTime.Now,
                        OrderTotal = totalFullPrice,
                        Status = "Delivered",
                        ShippingAddress = customer.CustomerDetails.Address,
                        CustomerId = customer.Id,
                    };
                    _context.Orders.Add(newOrder);
                    _context.SaveChanges();
                    OrderItem newOrderItem = new OrderItem
                    {
                        OrderId = newOrder.Id,
                        foods = choosenFood.foods,
                        products = choosenFood.products,
                    };
                    _context.OrderItems.Add(newOrderItem);

                    _context.SaveChanges();

                    _context.Remove(customer.Basket);
                    _context.SaveChanges();

                    customer.CustomerDetails.LoyaltyPoints = customer.CustomerDetails.LoyaltyPoints + (totalFullPrice / 100);
                    _context.SaveChanges();
                    string safeDate = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    string fullRoute = Path.Combine(baseRoute, $"{customer.FirstName}-{customer.LastName}-Order-[{safeDate}].docx");

                    using (DocX document = DocX.Create(fullRoute))
                    {
                        document.InsertParagraph("Order Receipt:").Bold();
    
                        foreach (var food in choosenFood.foods)
                        {
                            document.InsertParagraph($"{food.Name} - {food.Price:C}");
                            document.InsertParagraph($"Venue: {food.MenuCategory.Menu.FoodCategory.FoodChain.Name}");
                        }
                        
                        foreach (var product in choosenFood.products)
                        {
                            document.InsertParagraph($"{product.Name} - {product.Price:C}");
                            document.InsertParagraph($"Venue: {product.ProdCategory.Prodaction.StoreCategory.Store.Name}");
                        }
    
                        document.InsertParagraph($"Total Price + Delivery Service: {totalFullPrice:C}");
                        document.InsertParagraph($"Order time -> {DateTime.Now}");
                        document.InsertParagraph("Thank You For Using Our App!").Bold();
    
                        document.Save();
                    }

                    
                    SMTPService.SendEmail(
                        customer.Email,
                        "Your Order Document",
                        "Please find your order document attached.",
                        fullRoute);
                    
                    if (customer.CustomerDetails.LoyaltyPoints >= 1000)
                    {
                        customer.CustomerDetails.isVip = 1;
                        _context.SaveChanges();
                        Console.WriteLine("Wow You are VIP!");
                    }
                    Console.WriteLine("Ordered Successfully!");
                    Console.WriteLine($"You got -> {customer.CustomerDetails.LoyaltyPoints : F2} Loyality Points!");
                }
                
                else if (nowTime.DayOfWeek.ToString().ToLower() == "sunday")
                {
                    TimeSpan nowTimeHours = nowTime.TimeOfDay;
                    var schedule = choosenFoodDistance.foods
                        .Where(x => x.MenuCategory.Menu.FoodCategory.Schedule.SundayOpenTime <= nowTimeHours &&
                                    x.MenuCategory.Menu.FoodCategory.Schedule.SundayCloseTime > nowTimeHours)
                        .ToList();
                    
                    var isOpen = choosenFoodDistance.foods
                        .Where(x => x.MenuCategory.Menu.FoodCategory.Schedule.isSundayOpen == false).ToList();
                    
                    var scheduleStore = choosenProductDistance.products
                        .Where(x => x.ProdCategory.Prodaction.StoreCategory.Schedule.SundayOpenTime <= nowTimeHours &&
                                    x.ProdCategory.Prodaction.StoreCategory.Schedule.SundayCloseTime > nowTimeHours)
                        .ToList();
                    
                    var isOpenStore = choosenProductDistance.products
                        .Where(x => x.ProdCategory.Prodaction.StoreCategory.Schedule.isSundayOpen == false).ToList();
                    
                    if (isOpen.Count > 0  || isOpenStore.Count > 0)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Janitor();
                        Line();
                        Console.WriteLine("Sorry One of the Vanue/store is Closed For Today! Order Later!");
                        Environment.Exit(0);
                    }

                    if (choosenFoodDistance.foods.Count > schedule.Count || choosenProductDistance.products.Count > scheduleStore.Count)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Janitor();
                        Line();
                        Console.WriteLine("Sorry One of the Vanue/store is Closed! Order Later!");
                        Environment.Exit(0);
                    }

                    if (customer.Balance < totalFullPrice)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Janitor();
                        Line();
                        Console.WriteLine("Insufficient Funds! Order Later!");
                        Environment.Exit(0);
                    }

                    Console.WriteLine();
                    Console.WriteLine("Ordering... ");
                    customer.Balance = customer.Balance - totalFullPrice;

                    Order newOrder = new Order
                    {
                        OrderDate = DateTime.Now,
                        OrderTotal = totalFullPrice,
                        Status = "Delivered",
                        ShippingAddress = customer.CustomerDetails.Address,
                        CustomerId = customer.Id,
                    };
                    _context.Orders.Add(newOrder);
                    _context.SaveChanges();
                    OrderItem newOrderItem = new OrderItem
                    {
                        OrderId = newOrder.Id,
                        foods = choosenFood.foods,
                        products = choosenFood.products,
                    };
                    _context.OrderItems.Add(newOrderItem);

                    _context.SaveChanges();

                    _context.Remove(customer.Basket);
                    _context.SaveChanges();

                    customer.CustomerDetails.LoyaltyPoints = customer.CustomerDetails.LoyaltyPoints + (totalFullPrice / 100);
                    _context.SaveChanges();
                    string safeDate = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    string fullRoute = Path.Combine(baseRoute, $"{customer.FirstName}-{customer.LastName}-Order-[{safeDate}].docx");

                    using (DocX document = DocX.Create(fullRoute))
                    {
                        document.InsertParagraph("Order Receipt:").Bold();
    
                        foreach (var food in choosenFood.foods)
                        {
                            document.InsertParagraph($"{food.Name} - {food.Price:C}");
                            document.InsertParagraph($"Venue: {food.MenuCategory.Menu.FoodCategory.FoodChain.Name}");
                        }
                        
                        foreach (var product in choosenFood.products)
                        {
                            document.InsertParagraph($"{product.Name} - {product.Price:C}");
                            document.InsertParagraph($"Venue: {product.ProdCategory.Prodaction.StoreCategory.Store.Name}");
                        }
    
                        document.InsertParagraph($"Total Price + Delivery Service: {totalFullPrice:C}");
                        document.InsertParagraph($"Order time -> {DateTime.Now}");
                        document.InsertParagraph("Thank You For Using Our App!").Bold();
    
                        document.Save();
                    }

                    
                    SMTPService.SendEmail(
                        customer.Email,
                        "Your Order Document",
                        "Please find your order document attached.",
                        fullRoute);
                    
                    if (customer.CustomerDetails.LoyaltyPoints >= 1000)
                    {
                        customer.CustomerDetails.isVip = 1;
                        _context.SaveChanges();
                        Console.WriteLine("Wow You are VIP!");
                    }
                    Console.WriteLine("Ordered Successfully!");
                    Console.WriteLine($"You got -> {customer.CustomerDetails.LoyaltyPoints : F2} Loyality Points!");
                }
            }
            else if (payInput.Choice == "2")
            {
                Console.WriteLine();
                Console.WriteLine("Payment Aborted!");
            }
            
        }
        else if (input == "2")
        {
            Janitor();
            Line();
            Console.WriteLine("Clearing Basket...");
            _context.Remove(customer.Basket);
            _context.SaveChanges();
            Console.WriteLine("Basket is Empty!");
        }
    }

    static public void AddBalance(Customer customer)
    {
        WoltColor();
        Janitor();
        Line();
        Console.WriteLine("Deposit money ($1 - $10,000) : ");
        decimal depositMoney = decimal.Parse(Console.ReadLine());

        if (depositMoney >= 1 && depositMoney <= 10000)
        {
            Console.WriteLine();
            Console.WriteLine("Please wait...");
            customer.Balance = customer.Balance + depositMoney;
            _context.SaveChanges();
            Console.WriteLine("Balance Updated!");
            Console.WriteLine($"Current Balance: {customer.Balance :C}");
        }
        else
        {   
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Janitor();
            Line();
            Console.WriteLine("Incorrect Input!");
        }
    }

    public static void SeePastOrders(Customer customer)
    {
        WoltColor();
        Janitor();
        Line();
        Console.WriteLine("Order History:");
        
        var allOrders = _context.Orders
            .Include(x => x.orderItem)
            .ThenInclude(x => x.foods)
            .Include(x => x.orderItem.products)
            .Where(x => x.CustomerId == customer.Id)
            .ToList();

        foreach (var order in allOrders)
        {
            Console.WriteLine();
            Line();
            Console.WriteLine($"Order - [{order.Id}]");
            Console.WriteLine($"Date: {order.OrderDate}");
            Console.WriteLine($"Total Price: {order.OrderTotal :C}");
            Console.WriteLine($"Status: {order.Status}");
            Console.WriteLine($"Shipping Address: {order.ShippingAddress}");
            Console.WriteLine();
            Console.WriteLine("Order Items:");
            foreach (var foods in order.orderItem.foods)
            {
                Console.WriteLine("-----------------------------------");
                Console.WriteLine($"{foods.Name} - {foods.Price :C}");
            }

            foreach (var products in order.orderItem.products)
            {
                Console.WriteLine("-----------------------------------");
                Console.WriteLine($"{products.Name} - {products.Price :C}");
            }
        }
        
    }
} 