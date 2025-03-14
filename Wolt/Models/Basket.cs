namespace Wolt.Models;

public class Basket
{
    public int Id { get; set; }
    public List<Food> foods { get; set; } = new List<Food>();
    public List<Product> products { get; set; } = new List<Product>();
    public int CustomerId { get; set; }
    public Customer Customer { get; set; }
}