using System.ComponentModel.DataAnnotations;

namespace PersonalisedTravelChecklist.Models
{
    public class ChecklistItem
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public bool IsChecked { get; set; }

        public DateTime? CheckedTime { get; set; }

        public int TripId { get; set; }
        public Trip Trip { get; set; } = null!;

        public int? ReminderMinutes { get; set; }

        public string? Location { get; set; }

        public bool IsImportant { get; set; }
    }
}