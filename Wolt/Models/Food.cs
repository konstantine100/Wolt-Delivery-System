namespace Wolt.Models;

public class Food
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int MenuCategoryId { get; set; }
    public MenuCategory MenuCategory { get; set; }
    public List<Ingridients> Ingridients { get; set; } = new List<Ingridients>();
    
    public List<Basket> baskets { get; set; } = new List<Basket>();
    public List<OrderItem> orders { get; set; } = new List<OrderItem>();
}