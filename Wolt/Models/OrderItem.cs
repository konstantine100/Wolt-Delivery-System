namespace Wolt.Models;

public class OrderItem
{
    public int Id { get; set; }
    public List<Food> foods { get; set; } = new List<Food>();
    public List<Product> products { get; set; } = new List<Product>();
    
    public int OrderId { get; set; }
    public Order Order { get; set; }
}