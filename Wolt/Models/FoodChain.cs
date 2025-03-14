namespace Wolt.Models;

public class FoodChain
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string City { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    public decimal Rating { get; set; }
    public decimal ParticipantScore { get; set; }
    public decimal ParticipantNumber { get; set; }
    public decimal OrderFee { get; set; }
    public TimeSpan OrderTime { get; set; }
    public int OwnerId { get; set; }
    public Owner Owner { get; set; }
    public FoodCategory Category { get; set; }
}