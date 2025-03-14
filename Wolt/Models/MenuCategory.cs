namespace Wolt.Models;

public class MenuCategory
{
    public int Id { get; set; }
    public int MenuId { get; set; }
    public Menu Menu { get; set; }
    
    public string Name { get; set; }
    public string Description { get; set; }
    public List<Food> Foods { get; set; } = new List<Food>();
}