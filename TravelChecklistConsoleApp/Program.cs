
using Microsoft.Data.SqlClient;
using TravelChecklistConsoleApp.Models;

namespace TravelChecklistConsoleApp;

class Program
{
    private static List<Trip> trips = new List<Trip>();
    private static int nextTripId = 1;
    private static int nextItemId = 1;

    // Connecting to the database
    private const string ConnectionString = "Server=localhost,1433;Database=TravelChecklist;User Id=SA;Password=P4ssword;TrustServerCertificate=True;";

    public static void Main(string[] args)
    {
        Console.WriteLine("Travel Checklist - Checklist Manager");
        Console.WriteLine("====================================");

        Run();
    }

    public static void Run()
    {
        // Menu Loop
        while (true)
        {
            ShowMenu();
            string? choice = Console.ReadLine();
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
        using var connection = new SqlConnection(ConnectionString);
        connection.Open();

        string sql = @"
            SELECT t.*,
                   COUNT(ci.Id) AS TotalItems,
                   SUM(CASE WHEN ci.IsPacked = 1 THEN 1 ELSE 0 END) AS PackedItems
            FROM Trips t
            LEFT JOIN ChecklistItems ci ON t.id = ci.TripId
            WHERE t.StartTime BETWEEN @now AND @next24hrs
            GROUP BY t.id, t.Name, t.TripType, t.StartTime, t.EndTime, t.CreatedAt
            ORDER BY t.StartTime";

        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@now", DateTime.Now);
        command.Parameters.AddWithValue("@next24hrs", DateTime.Now.AddHours(24));

        using var reader = command.ExecuteReader();

        Console.WriteLine("\n== Upcoming Trips (Next 24 Hours) ==");
        bool hasUpcoming = false;

        while (reader.Read())
        {
            hasUpcoming = true;
            var name = reader["Name"];
            var tripType = reader["TripType"];
            var startTime = Convert.ToDateTime(reader["StartTime"]);
            var totalItems = Convert.ToInt32(reader["ItemCount"]);
            var packedItems = Convert.ToInt32(reader["PackedCount"]);
            var timeUntilTrip = startTime - DateTime.Now;

            Console.WriteLine($"⏰ {name} ({tripType}) starts in {timeUntilTrip:h\\:mm} hours!");
            Console.WriteLine($"    Packing progress: {packedItems}/{totalItems} items packed");

            // Show unpacked items
            if (packedItems < totalItems)
            {
                DisplayUnpackedItems(Convert.ToInt32(reader["Id"]));
            }
            Console.WriteLine();
        }

        if (!hasUpcoming)
            Console.WriteLine("No upcoming trips in the next 24 hours.\n");
    }

    private static void DisplayUnpackedItems(int v)
    {
        throw new NotImplementedException();
    }

    private static void MarkItemAsPacked()
    {
        Console.Write("\nEnter Trip ID: ");
        if (!int.TryParse(Console.ReadLine(), out int tripId))
        {
            Console.WriteLine("Invalid Trip ID. Please enter a numeric value.\n");
            return;
        }

        // Show items for this trip
        DisplayTripItems(tripId);

        Console.WriteLine("Enter Item ID to mark as packed: ");
        if (!int.TryParse(Console.ReadLine(), out int itemId))
        {
            Console.WriteLine("Invalid Item ID. Please enter a numeric value.\n");
            return;
        }

        // Update the items
        UpdateItemAsPacked(itemId);
    }

    private static void UpdateItemAsPacked(int itemId)
    {
        using var connection = new SqlConnection(ConnectionString);
        connection.Open();

        string sql = @"
            UPDATE ChecklistItems
            SET IsPacked = 1, PackedAt = @packedAt
            WHERE Id = @itemId";

        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@itemId", itemId);
        command.Parameters.AddWithValue("@packedAt", DateTime.Now);

        int rows = command.ExecuteNonQuery();
        Console.WriteLine(rows > 0 ? "Item marked as packed!\n" : "Item not found.\n");
    }

    private static void DisplayTripItems(int tripId)
    {
        using var connection = new SqlConnection(ConnectionString);
        connection.Open();

        string sql = "SELECT * FROM ChecklistItems WHERE TripId = @tripId ORDER BY Id";
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@tripId", tripId);

        using var reader = command.ExecuteReader();

        Console.WriteLine($"\nChecklist Items for Trip {tripId}:");
        bool hasItems = false;

        while (reader.Read())
        {
            hasItems = true;
            var itemId = reader["Id"];
            var name = reader["Name"];
            var isPacked = Convert.ToBoolean(reader["IsPacked"]);
            var category = reader["Category"];
            var status = isPacked ? "[PACKED}" : "[ ]";

            Console.WriteLine($"    {itemId}. {status} {name} ({category})");
        }

        if (!hasItems)
        {
            Console.WriteLine("   No items in checklist.\n");
        }
    }

    private static void AddItemToTrip()
    {
        ViewAllTrips();

        Console.WriteLine("Enter Trip ID to add item to: ");
        if (!int.TryParse(Console.ReadLine(), out int tripId))
        {
            Console.WriteLine("Invalid Trip ID. Please enter a numeric value.\n");
            return;
        }

        // Check if trip exists
        if (!TripExists(tripId))
        {
            Console.WriteLine($"Trip with ID {tripId} does not exist.\n");
            return;
        }

        Console.Write("Item Name: ");
        string? itemName = Console.ReadLine();
        if (string.IsNullOrEmpty(itemName) )
        {
            Console.WriteLine("Item name can't be empty. Operation cancelled.\n");
            return;
        }

        Console.Write("Category (General/Clothing/Electronics/Toiletries/Other): ");
        string? category = Console.ReadLine() ?? "General";

        int itemId = InsertChecklistItem(tripId, itemName, category);
        Console.WriteLine($"Added '{itemName}' to trip with item ID: {itemId}\n");
    }

    private static int InsertChecklistItem(int tripId, string name, string category)
    {
        using var connection = new SqlConnection(ConnectionString);
        connection.Open();

        string sql = @"
            INSERT INTO ChecklistItems (TripId, Name, Category)
            VALUES (@tripId, @name, @category);
            SELECT SCOPE_IDENTITY();";

        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@tripId", tripId);
        command.Parameters.AddWithValue("@name", name);
        command.Parameters.AddWithValue("@category", category);

        return Convert.ToInt32(command.ExecuteScalar());
    }

    private static bool TripExists(int tripId)
    {
        using var connection = new SqlConnection(ConnectionString);
        connection.Open();

        string sql = "SELECT COUNT(1) FROM Trips WHERE Id = @id";
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@id", tripId);

        int count = Convert.ToInt32(command.ExecuteScalar());
        return count > 0;
    }

    private static void ViewAllTrips()
    {
        // Open database connection
        using var connection = new SqlConnection(ConnectionString);
        connection.Open();

        string sql = @"
            SELECT t.*,
                   COUNT(ci.Id) AS ItemCount,
                   SUM(CASE WHEN ci.IsPacked = 1 THEN 1 ELSE 0 END) AS PackedCount
            FROM Trips t
            LEFT JOIN ChecklistItems ci ON t.Id = ci.TripId
            GROUP BY t.Id, t.Name, t.TripType, t.StartTime, t.EndTime, t.CreatedAt
            ORDER BY t.StartTime";
        
        using var command = new SqlCommand(sql, connection);
        using var reader = command.ExecuteReader();

        Console.WriteLine("\n== Your Trips ==");
        bool hasTrips = false;

        while (reader.Read())
        {
            hasTrips = true;
            var tripId = reader["Id"];
            var name = reader["Name"];
            var tripType = reader["TripType"];
            var startTime = Convert.ToDateTime(reader["StartTime"]);
            var itemCount = reader["ItemCount"];
            var packedCount = reader["PackedCount"];
            var timeUntilTrip = startTime - DateTime.Now;

            Console.WriteLine($"""
                Trip {tripId}: {name} ({tripType})
                    Dates: {startTime:dd MMM, yyyy HH:mm}
                    Items: {packedCount}/{itemCount} packed
                    Time until trip: {timeUntilTrip.Days}d {timeUntilTrip.Hours}h

                """);
        }
        if (!hasTrips)
            Console.WriteLine("No trips found.\n");

    }

    private static void CreateNewTrip()
    {
        Console.WriteLine("\n== Create New Trip ==");

        try
        {
            Console.Write("Trip Name: ");
            string? name = Console.ReadLine();
            if (string.IsNullOrEmpty(name))
            {
                Console.WriteLine("Trip name cannot be empty. Operation cancelled.\n");
                return;
            }

            Console.Write("Trip Type (Train/Car/Plane/General): ");
            string? tripType = Console.ReadLine();

            Console.Write("Start Time (yyyy-MM-dd HH:mm): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime startTime))
            {
                Console.WriteLine("Invalid start time format. Operation cancelled.\n");
                return;
            }

            Console.Write("End Time (yyyy-MM-dd HH:mm): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime endTime))
            {
                Console.WriteLine("Invalid end time format. Operation cancelled.\n");
                return;
            }

            // Confirm insertion
            Console.WriteLine($"\nPlease confirm the details:"); 
            Console.WriteLine($"Trip Name: {name}");
            Console.WriteLine($"Trip Type: {tripType}");
            Console.WriteLine($"Start Time: {startTime}");
            Console.WriteLine($"End Time: {endTime}");
            Console.Write("Is this information correct? (Y/N): ");

            string? reply = Console.ReadLine()?.ToUpper().Trim();
            if (reply != "Y")
            {
                Console.WriteLine("Trip creation cancelled.\n");
                return;
            }

            // Insert data into database
            int tripId = InsertTrip(name, tripType, startTime, endTime);
            Console.WriteLine($"New trip created with ID: {tripId}\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating trip: {ex.Message}");
        }
    }

    // Insert trip into database
    private static int InsertTrip(string name, string? tripType, DateTime startTime, DateTime endTime)
    {
        using var connection = new SqlConnection(ConnectionString);
        connection.Open();

        string sql = @"
            INSERT INTO Trips (Name, TripType, StartTime, EndTime)
            VALUES (@name, @tripType, @startTime, @endTime);
            SELECT SCOPE_IDENTITY();";

        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@name", name);
        command.Parameters.AddWithValue("@tripType", tripType);
        command.Parameters.AddWithValue("@startTime", startTime);
        command.Parameters.AddWithValue("@endTime", endTime);

        return Convert.ToInt32(command.ExecuteScalar());
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