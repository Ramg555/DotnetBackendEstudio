using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace AdoNet.Practica;

public class ConnectionDatabase
{
    public static async Task<SqlConnection?> Connection()
    {
        var config = new ConfigurationBuilder()
        .AddUserSecrets<Program>()
        .Build();

        var connectionString = config.GetConnectionString("Practica") ?? throw new InvalidOperationException("Falta la connectionString");
        var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();

        return connection;
    }
}
