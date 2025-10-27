
using TravelChecklistConsoleApp.Models;

namespace TravelChecklistConsoleApp;

class Program
{
    private static List<Trip> trips = new List<Trip>();
    private static int nextTripId = 1;
    private static int nextItemId = 1;
    static void Main(string[] args)
    {
        Console.WriteLine("Travel Checklist - Checklist Manager");
        Console.WriteLine("====================================");
        
        // Menu Loop
        while (true)
        {
            ShowMenu();
            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    // Create New Trip
                    CreateNewTrip();
                    break;
                case "2":
                    // View Trips
                    ViewAllTrips();
                    break;
                case "3":
                    // Add Checklist Item to Trip
                    AddItemToTrip();
                    break;
                case "4":
                    // Mark Item as Packed
                    MarkItemAsPacked();
                    break;
                case "5":
                    // Check Reminders for Upcoming Trips
                    CheckReminders();
                    break;
                case "6":
                    Console.WriteLine("Exiting...");
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }

            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadKey();
            Console.Clear();
        }
    }

    private static void CheckReminders()
    {
        var upcomingTrips = trips.Where(t => t.TimeUntilTrip.TotalHours <= 24 && t.TimeUntilTrip.TotalHours > 0).ToList();

        if (upcomingTrips.Count == 0)
        {
            Console.WriteLine("No upcoming trips in the next 24 hours.");
            return;
        }

        Console.WriteLine("\nUpcoming Trips (Next 24 hours):");
        Console.WriteLine("=================================");

        foreach (var trip in upcomingTrips)
        {
            var packedCount = trip.ChecklistItems.Count(i => i.IsPacked);
            var totalCount = trip.ChecklistItems.Count;

            Console.WriteLine($"\n⏰ {trip.Name} starts in {trip.TimeUntilTrip:hh\\:mm} hours!");
            Console.WriteLine($"    Packing progress: {packedCount}/{totalCount} items packed");

            if (packedCount < totalCount)
            {
                var unpackedItems = trip.ChecklistItems.Where(i => i.IsPacked).ToList();
                Console.WriteLine("     Remaining items:");
                foreach (var item in unpackedItems)
                {
                    Console.WriteLine($"    -{item.Name}");
                }
            }
        }
    }

    private static void MarkItemAsPacked()
    {
        ViewAllTrips();

        Console.WriteLine("Enter Trip ID");
        if (int.TryParse(Console.ReadLine(), out int tripId))
        {
            var trip = trips.FirstOrDefault(t => t.Id == tripId);
            if (trip != null && trip.ChecklistItems.Count > 0)
            {
                Console.WriteLine($"\nChecklist for {trip.Name}:");
                foreach (var item in trip.ChecklistItems)
                {
                    var status = item.IsPacked ? "[PACKED]" : "[ ]";
                    Console.WriteLine($"{item.Id}. {status} {item.Name} ({item.Category})");
                }

                Console.WriteLine("Enter Item ID to mark as packed: ");
                if (int.TryParse(Console.ReadLine(), out int itemId))
                {
                    var item = trip.ChecklistItems.FirstOrDefault(t => t.Id == itemId);
                    if (item != null)
                    {
                        item.IsPacked = true;
                        item.PackedAt = DateTime.UtcNow;
                        Console.WriteLine($"Marked '{item.Name}' as packed!");
                    }
                    else
                        Console.WriteLine("Item not found.");
                }
            }
            else
                Console.WriteLine("Trip not found or no items in checklist.");
        }
    }

    private static void AddItemToTrip()
    {
        ViewAllTrips();

        Console.WriteLine("Enter Trip ID to add item to: ");
        if (int.TryParse(Console.ReadLine(), out int tripId))
        {
            var trip = trips.FirstOrDefault(t => t.Id == tripId);
            if (trip != null)
            {
                Console.WriteLine("Item Name: ");
                var itemName = Console.ReadLine();

                Console.WriteLine("Category (General/Clothing/Electronics/Toiletries/Other): ");

                var category = Console.ReadLine() ?? "General";

                var newItem = new ChecklistItem
                {
                    Id = nextItemId++,
                    Name = itemName,
                    Category = category
                };

                trip.ChecklistItems.Add(newItem);
                Console.WriteLine($"Added '{itemName}' to {trip.Name} successfully.");
            }
            else
                Console.WriteLine("Trip not found.");
        }
        else
            Console.WriteLine("Invalid Trip ID.");
    }

    private static void ViewAllTrips()
    {
        if (trips.Count == 0)
        {
            Console.WriteLine("No trips found");
            return;
        }

        Console.WriteLine("\nYour Trips:");
        Console.WriteLine("=============");

        foreach (var trip in trips)
        {
            Console.WriteLine($"{trip.Id}. {trip.Name} ({trip.TripType}) - {trip.StartTime:dd MMM, yyyy} to {trip.EndTime:dd MMM, yyyy}");
            Console.WriteLine($"    Items: {trip.ChecklistItems.Count}, Packed: {trip.ChecklistItems.Count(i => i.IsPacked)}");
            Console.WriteLine($"    Time until trip: {trip.TimeUntilTrip.Days} days, {trip.TimeUntilTrip.Hours} hours\n");
        }
    }

    private static void CreateNewTrip()
    {
        Console.Write("Trip Name: ");
        var name = Console.ReadLine();

        Console.Write("Trip Type (Train/Car/Plane/General): ");
        var tripType = Console.ReadLine();

        Console.Write("Start Time (yyyy-MM-dd HH:mm): ");
        if (DateTime.TryParse(Console.ReadLine(), out var startTime))
        {
            Console.Write("End Time (yyyy-MM-dd HH:mm): ");
            if (DateTime.TryParse(Console.ReadLine(), out var endTime))
            {
                var newTrip = new Trip
                {
                    Id = nextTripId++,
                    Name = name,
                    TripType = tripType,
                    StartTime = startTime,
                    EndTime = endTime
                };

                trips.Add(newTrip);
                Console.WriteLine($"Trip '{name}' of type '{tripType}' created from {startTime} to {endTime}.");
            }
            else
            {
                Console.WriteLine("Invalid end time format.");
            }
        }
        else
        {
            Console.WriteLine("Invalid start time format.");
        }
    }

    public static void ShowMenu()
    {
        Console.WriteLine("\nMain Menu:");
        Console.WriteLine("""
            1. Create New Trip
            2. View Trips
            3. Add Checklist Item to Trip
            4. Mark Item as Packed
            5. Check Reminders for Upcoming Trips
            6. Exit
            """);
        Console.Write("Choose an option: ");
    }

}