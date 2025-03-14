namespace Wolt.Models;

public class Menu
{
    public int Id { get; set; }
    public string Description { get; set; }
    public List<MenuCategory> Categories { get; set; } =  new List<MenuCategory>();
    public int FoodCategoryId { get; set; }
    public FoodCategory FoodCategory { get; set; }
    
}