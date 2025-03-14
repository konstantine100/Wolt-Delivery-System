namespace Wolt.Models;

public class Order
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal OrderTotal { get; set; }
    public string Status { get; set; }
    public string ShippingAddress { get; set; }
    
    public int CustomerId { get; set; }
    public Customer Customer { get; set; }
    
    public OrderItem orderItem { get; set; }
}