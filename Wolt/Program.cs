using Microsoft.EntityFrameworkCore;
using Wolt.Data;
using Wolt.Models;
using Wolt.Validator;
using Wolt.SMTP;
using BCrypt.Net;
using System.Globalization;
using System.Text;
using Wolt.Services;


void WoltApp()
{
    Console.WriteLine();
    Console.ForegroundColor = ConsoleColor.DarkGreen;
    Console.WriteLine("Start App? y/n");
    
    string startUp = Console.ReadLine();

    if (startUp.ToLower() == "n")
    {
        Console.WriteLine();
        Console.WriteLine("Exiting...");
        Environment.Exit(0);
    }
    else if (startUp.ToLower() == "y")
    {
        CustomerService.WoltColor();
        CustomerService.Janitor();
        CustomerService.Line();
        Console.WriteLine("Welcome to the Wolt!");
        Console.WriteLine("Enter What to do!");
        Console.WriteLine();
        Console.WriteLine("1) Create Owner");
        Console.WriteLine("2) Log in to Owner");
        Console.WriteLine("3) Create Customer");
        Console.WriteLine("4) Log in to Customer");

        string theMainInput = Console.ReadLine();
        
        Choices theChoice = new Choices { Choice = theMainInput};
        var theValidator = new ChoicesValidator(4);
        
        var theResult = theValidator.Validate(theChoice);

        if (!theResult.IsValid)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            CustomerService.Janitor();
            CustomerService.Line();
            foreach (var error in theResult.Errors)
            {
                Console.WriteLine($"ERROR: Wrong {error.PropertyName} - {error.ErrorMessage}");
            }
        }
        else if (theMainInput == "1")
        {
            OwnerService.AddOwner();
        }
        else if (theMainInput == "2")
        {
            OwnerService.LogInOwner();
        }
        else if (theMainInput == "3")
        {
            CustomerService.AddCustomer();
        }
        else if (theMainInput == "4")
        {
            CustomerService.LogInCustomer();
        }
    }

    else
    {
        Console.ForegroundColor = ConsoleColor.DarkRed;
        CustomerService.Janitor();
        CustomerService.Line();
        Console.WriteLine("Wrong Input!");
    }
}

while (true)
{
    WoltApp();
}
