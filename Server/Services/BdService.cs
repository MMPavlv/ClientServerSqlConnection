using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Server.Options;

namespace Server.Services;

public class BdService
{
    private readonly SqlConnection _connection;

    public BdService(IOptions<BdOptions> options)
    {
        var connectionString = options.Value.ConnectionString;
        _connection = new SqlConnection(connectionString);
    }

    public System.Data.ConnectionState GetConnectionState()
    {
        return _connection.State;
    }

    public System.Data.ConnectionState Connect()
    {
        _connection.Open();
        return _connection.State;
    }

    public System.Data.ConnectionState Close()
    {
        _connection.Close();
        return _connection.State;
    }

    public string GetVersion()
    {
        Console.WriteLine(_connection.State);
        using var command = new SqlCommand("SELECT @@VERSION", _connection);

        var result = command.ExecuteScalar();
        Console.WriteLine(result);

        return result?.ToString() ?? "";
    }
}
