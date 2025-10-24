
namespace TravelChecklistConsoleApp;

class Program
{
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
        throw new NotImplementedException();
    }

    private static void ViewAllTrips()
    {
        throw new NotImplementedException();
    }

    private static void CreateNewTrip()
    {
        throw new NotImplementedException();
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