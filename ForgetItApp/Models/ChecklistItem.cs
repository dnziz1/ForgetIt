using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForgetItApp.Models;

public class ChecklistItem
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsPacked { get; set; }
    public int TripId { get; set; }
}
