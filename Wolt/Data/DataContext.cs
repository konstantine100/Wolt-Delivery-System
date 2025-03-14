using Microsoft.EntityFrameworkCore;
using Wolt.Models;

namespace Wolt.Data;

public class DataContext : DbContext
{
    public DbSet<Basket> Baskets { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<CustomerDetails> CustomerDetails { get; set; }
    public DbSet<Food> Foods { get; set; }
    public DbSet<FoodCategory> FoodCategories { get; set; }
    public DbSet<FoodChain> FoodChains { get; set; }
    public DbSet<Ingridients> Ingridients { get; set; }
    public DbSet<Menu> Menus { get; set; }
    public DbSet<MenuCategory> MenuCategories { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Owner> Owners { get; set; }
    public DbSet<Prodaction> Prodactions { get; set; }
    public DbSet<ProdCategory> ProdCategories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Schedule> Schedules { get; set; }
    public DbSet<Store> Stores { get; set; }
    public DbSet<StoreCategory> StoreCategories { get; set; }
    public DbSet<StoreSchedule> StoreSchedules { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Data Source=(localdb)\ProjectModels;Initial Catalog=Wolt;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
    }
    
    
}