using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelChecklistConsoleApp.Models;

public class Trip
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string TripType { get; set; } = "General";
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public List<ChecklistItem> ChecklistItems { get; set; } = new List<ChecklistItem>();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public TimeSpan TimeUntilTrip => StartTime - DateTime.UtcNow;
    public bool IsUpcoming => TimeUntilTrip.TotalHours <= 24; // Example property to indicate if the trip is within the next 24 hours
}
