using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForgetItApp.Models;
/// <summary>
/// Represents a trip with its details and associated checklist items
/// so that users can manage their packing and preparations. 
/// </summary>

public class Trip
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime DepartureDateTime { get; set; }
    public List<ChecklistItem> Items { get; set; } = new();
}
