using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Dapper.Practica;

public static class DbConnectionFactory
{
    public static async Task<SqlConnection> OpenConnectionAsync()
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
