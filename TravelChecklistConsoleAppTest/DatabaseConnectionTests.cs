using Microsoft.Data.SqlClient;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TravelChecklistConsoleApp;

namespace TravelChecklistConsoleAppTest;

[TestClass]
public class DatabaseConnectionTests
{
    private const string ConnectionString = "Server=localhost,1433;Database=TravelChecklist;User Id=SA;Password=P4ssword;TrustServerCertificate=True;";

    [TestMethod]
    public void ConnectionStringShouldNotBeNullOrEmpty()
    {
        var connectionString = ConnectionString;

        Assert.IsFalse(string.IsNullOrEmpty(connectionString), "Connection string should not be null or empty");
        Assert.IsTrue(connectionString.Length > 0, "Connection string length should be greater than zero");
    }

    [TestMethod]
    public void ConnectionStringShouldContainRequiredParameters()
    {
        var connectionString = ConnectionString;

        StringAssert.Contains(connectionString, "Server=");
        StringAssert.Contains(connectionString, "Database=");
        StringAssert.Contains(connectionString, "User Id=");
        StringAssert.Contains(connectionString, "Password=");
    }

    [TestMethod]
    public void DatabaseConnectionShouldOpenSuccessfully()
    {
        using var connection = new SqlConnection(ConnectionString);

        try
        {
            connection.Open();
            Assert.AreEqual(System.Data.ConnectionState.Open, connection.State, "Connection should be open");

            // Test if a query can be executed
            using var command = new SqlCommand("SELECT 1", connection);
            var result = command.ExecuteScalar();

            Assert.AreEqual(1, result, "Should be able to execute queries");
        }
        catch (Exception ex)
        {
            Assert.Fail(ex.Message);
        }
    }

    [TestMethod]
    public void ConnectionToStringShouldReturnValidString()
    {
        using var connection = new SqlConnection(ConnectionString);

        var connectionString = connection.ToString();

        Assert.IsFalse(string.IsNullOrEmpty(connectionString));
        Assert.IsTrue(connectionString.Length > 0);
    }
}
