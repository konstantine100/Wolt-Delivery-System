namespace Wolt.Models;

public class StoreSchedule
{
    public int Id { get; set; }
 
    public bool isMondayOpen { get; set; }
    public bool isTuesdayOpen { get; set; }
    public bool isWednesdayOpen { get; set; }
    public bool isThursdayOpen { get; set; }
    public bool isFridayOpen { get; set; }
    public bool isSaturdayOpen { get; set; }
    public bool isSundayOpen { get; set; }
    
    public TimeSpan MondayOpenTime { get; set; }
    public TimeSpan TuesdayOpenTime { get; set; }
    public TimeSpan WednesdayOpenTime { get; set; }
    public TimeSpan ThursdayOpenTime { get; set; }
    public TimeSpan FridayOpenTime { get; set; }
    public TimeSpan SaturdayOpenTime { get; set; }
    public TimeSpan SundayOpenTime { get; set; }
    
    public TimeSpan MondayCloseTime { get; set; }
    public TimeSpan TuesdayCloseTime { get; set; }
    public TimeSpan WednesdayCloseTime { get; set; }
    public TimeSpan ThursdayCloseTime { get; set; }
    public TimeSpan FridayCloseTime { get; set; }
    public TimeSpan SaturdayCloseTime { get; set; }
    public TimeSpan SundayCloseTime { get; set; }
    
    public bool isMondayAllDay { get; set; }
    public bool isTuesdayAllDay { get; set; }
    public bool isWednesdayAllDay { get; set; }
    public bool isThursdayAllDay { get; set; }
    public bool isFridayAllDay { get; set; }
    public bool isSaturdayAllDay { get; set; }
    public bool isSundayAllDay { get; set; }
    
    public int StoreCategoryId { get; set; }
    public StoreCategory StoreCategory { get; set; }
}