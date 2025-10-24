using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelChecklistConsoleApp.Models;

public class ChecklistItem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsPacked { get; set; } = false;
    public string Category { get; set; } = "General";
    public DateTime? PackedAt { get; set; }
}
