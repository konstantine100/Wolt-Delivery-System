﻿namespace Wolt.Models;

public class Customer
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    
    public decimal Balance { get; set; }
    
    public CustomerDetails CustomerDetails { get; set; }
    
    public Basket Basket { get; set; }
    public List<Order> Orders { get; set; } = new List<Order>();

    
}