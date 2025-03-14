namespace Wolt.Models;

public class Ingridients
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Calories { get; set; }
    public decimal AdditionalPrice { get; set; }
    public int FoodId { get; set; }
    public Food Food { get; set; }
}