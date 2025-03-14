namespace Wolt.Models;

public class FoodCategory
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int FoodChainId { get; set; }
    public FoodChain FoodChain { get; set; }
    public Schedule Schedule { get; set; }
    public Menu Menu { get; set; }
    
}