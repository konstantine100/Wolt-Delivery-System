namespace Wolt.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public bool IsAvailable  { get; set; }
    public int ProdCategoryId { get; set; }
    public ProdCategory ProdCategory { get; set; }
    public List<Basket> baskets { get; set; } = new List<Basket>();
    public List<OrderItem> orders { get; set; } = new List<OrderItem>();
}