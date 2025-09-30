using System.ComponentModel.DataAnnotations;

namespace PersonalisedTravelChecklist.Models
{
    public class Trip
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string TripType { get; set; } = string.Empty;

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        public string? Description { get; set; }

        public string? Location { get; set; }

        public List<ChecklistItem> ChecklistItems { get; set; } = new();

        public bool IsActive => DateTime.Now <= EndTime;

        public bool IsUpcoming => DateTime.Now < StartTime;
    }
}