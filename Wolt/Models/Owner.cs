namespace Wolt.Models;

public class Owner
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Password { get; set; }
    public DateTime DateOfBirth { get; set; }
    public List<FoodChain> FoodChains { get; set; } = new List<FoodChain>();
    public List<Store> Stores { get; set; } =  new List<Store>();
}