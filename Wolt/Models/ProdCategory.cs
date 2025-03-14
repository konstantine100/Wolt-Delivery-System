using System.ComponentModel.DataAnnotations.Schema;

namespace Wolt.Models;

public class ProdCategory
{
    public int Id { get; set; }
    public int ProdactionId { get; set; }
    public Prodaction Prodaction { get; set; }
    
    public string Name { get; set; }
    public string Description { get; set; }
    public List<Product> Products { get; set; } = new List<Product>();
}