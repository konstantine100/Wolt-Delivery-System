namespace Wolt.Models;

public class StoreCategory
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int StoreId { get; set; }
    public Store Store { get; set; }
    public StoreSchedule Schedule { get; set; }
    public Prodaction Prodaction { get; set; }
}