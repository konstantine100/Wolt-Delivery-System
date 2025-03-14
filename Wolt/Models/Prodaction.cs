namespace Wolt.Models;

public class Prodaction
{
    public int Id { get; set; }
    public string Description { get; set; }
    public List<ProdCategory> Categories { get; set; } =  new List<ProdCategory>();
    public int StoreCategoryId { get; set; }
    public StoreCategory StoreCategory { get; set; }
}