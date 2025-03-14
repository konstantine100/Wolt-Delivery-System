using Microsoft.EntityFrameworkCore;
using Wolt.Data;
using Wolt.Models;
using Wolt.Validator;
using Wolt.SMTP;
using BCrypt.Net;
using System.Globalization;
using System.Text;
namespace Wolt.Services;

public class OwnerService
{
    public static string baseRoute = @"C:\\Users\\kmami\\RiderProjects\\Wolt\\Logs";
    public static string ownerLog = "owner.txt";
    
    static public DataContext _context = new DataContext();
    static public void Line()
    {
        Console.WriteLine("==============================================");
    }
    static public void Janitor()
    {
        Console.Clear();
    }
    
    static public void AddOwner()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Janitor();
        Line();
        Console.WriteLine("Create Owner Account");
        
        Console.WriteLine("\r");
        Console.WriteLine("Owner First Name:");
        string firstName = Console.ReadLine();
        
        Console.WriteLine("\r");
        Console.WriteLine("Owner Last Name:");
        string lastName = Console.ReadLine();
        
        Console.WriteLine("\r");
        Console.WriteLine("Owner Phone Number:");
        string phoneNumber = Console.ReadLine();
        
        Console.WriteLine("\r");
        Console.WriteLine("Owner Email:");
        string email = Console.ReadLine();
        
        Console.WriteLine("\r");
        Console.WriteLine("Owner Password:");
        string password = Console.ReadLine();
        
        Console.WriteLine("\r");
        Console.WriteLine("Owner Birth Date:");
        DateTime birthDate = DateTime.Parse(Console.ReadLine());
        
        Line();
        Console.WriteLine("Please Wait...");
        
        Owner newOwner = new Owner
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Password = password,
            DateOfBirth = birthDate,
            Phone = phoneNumber,
        };
        
        var validator = new OwnerValidator();
        
        var result = validator.Validate(newOwner);
    
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
                newOwner.Password = BCrypt.Net.BCrypt.HashPassword(password);
                _context.Owners.Add(newOwner);
                _context.SaveChanges();
                string fullRoute = Path.Combine(baseRoute, ownerLog);

                using (StreamWriter wr = new StreamWriter(fullRoute, true))
                {
                    wr.WriteLine($"[LOG] Customer added -> [{newOwner.FirstName} {newOwner.LastName}] - {DateTime.Now}");
                }
                Console.WriteLine($"Owner {newOwner.FirstName} {newOwner.LastName} Saved Successfully!");
            }
            
            
        }
    }
    
    static public void LogInOwner()
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
        var choosenOwner = _context.Owners.FirstOrDefault(x => x.Email == email);
        if (choosenOwner != null)
        {
            bool isCorrect = BCrypt.Net.BCrypt.Verify(password, choosenOwner.Password);
    
            if (isCorrect)
            {
                OwnerOperations(choosenOwner);
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
                <p>Hello {choosenOwner.FirstName} {choosenOwner.LastName},</p>
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
                            choosenOwner.Password = hashPassword;
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
    static public void OwnerOperations(Owner owner)
    {
        Janitor();
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Line();
        Console.WriteLine($"Welcome to Wolt {owner.FirstName} {owner.LastName}!");
        Console.WriteLine("\r");
        Console.WriteLine("Choose option:");
        Console.WriteLine("1. Add Food Venue");
        Console.WriteLine("2. Add Store");
        Console.WriteLine("3. See Owned Food Venues");
        Console.WriteLine("4. See Owned Stores");
        Console.WriteLine("5. Account Information");
        Console.WriteLine("6. Exit");
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
            FoodService.AddFoodVanue(owner);
        }
        else if (choice.Choice == "2")
        {
            StoreService.AddStore(owner);
        }
        else if (choice.Choice == "3")
        {
            FoodService.AddDetails(owner);
        }
        else if (choice.Choice == "4")
        {
            StoreService.AddDetails(owner);
        }
        else if (choice.Choice == "5")
        {
            ChangeOwner(owner);
        }
        else if (choice.Choice == "6")
        {
            Console.WriteLine("Logging Off...");
            Environment.Exit(0);
        }
    }
    static public void ChangeOwner(Owner owner)
    {
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Janitor();
        Line();
        Console.WriteLine($"Owner {owner.FirstName} {owner.LastName}'s Account:");
        Console.WriteLine("\r");
        Console.WriteLine($"First Name: {owner.FirstName} ");
        Console.WriteLine($"Last Name: {owner.LastName} ");
        Console.WriteLine($"Phone Number: {owner.Phone}");
        Console.WriteLine($"Email: {owner.Email}");
        Console.WriteLine($"Birth Date: {owner.DateOfBirth}");
        Console.WriteLine("\r");
        Console.WriteLine("1) Change Info");
        
        string choice = Console.ReadLine();
        if (choice == "1")
        {
            Janitor();
            Line();
            Console.WriteLine("1) Change First Name");
            Console.WriteLine("2) Change Last Name");
            Console.WriteLine("3) Change Email ");
            Console.WriteLine("4) Change Phone Number");
            Console.WriteLine("5) Change Password");
            Console.WriteLine("6) Change Birth Date ");
            Console.WriteLine("7) Delete Account");
            string input = Console.ReadLine();
            Choices inputChoices = new Choices
            {
                Choice = input,
            };
            var validator = new ChoicesValidator(7);
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
                    owner.FirstName = newFirstName;
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
                    owner.LastName = newFirstName;
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
                    owner.Email = newFirstName;
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
                    owner.FirstName = newFirstName;
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
                var isCorrect = BCrypt.Net.BCrypt.Verify(currentPassword, owner.Password);
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
                        owner.Password = newFirstName;
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
                    owner.DateOfBirth = newFirstName;
                    _context.SaveChanges();
                    Console.WriteLine("Saved Successfully!");
                }
            }
            else if (input == "7")
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
                    _context.Remove(owner);
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
        else
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Janitor();
            Line();
            Console.WriteLine("Wrong Input!");
        }
        
    }
}