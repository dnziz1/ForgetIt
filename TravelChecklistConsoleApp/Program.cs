
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
        throw new NotImplementedException();
    }

    private static void MarkItemAsPacked()
    {
        throw new NotImplementedException();
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
                // Here you would normally save the trip to a database or in-memory list
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