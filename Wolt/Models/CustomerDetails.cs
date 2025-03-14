namespace Wolt.Models;

public class CustomerDetails
{
    public int Id { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    public DateTime DateOfBirth { get; set; }
    public decimal LoyaltyPoints { get; set; }
    public decimal isVip { get; set; }
    
    public int CustomerId { get; set; }
    public Customer Customer { get; set; }
}