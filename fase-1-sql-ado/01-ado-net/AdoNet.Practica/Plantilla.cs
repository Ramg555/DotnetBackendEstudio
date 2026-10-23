using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace AdoNet.Practica;

public class Plantilla
{
    public static async Task Plantilla_()
    {
        var config = new ConfigurationBuilder()
        .AddUserSecrets<Program>()
        .Build();

        var connectionString = config.GetConnectionString("Practica");

        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();

        const string sql = "SELECT Id, Nombre, Precio, Cantidad FROM Productos";

        await using var command = new SqlCommand(sql, connection);
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            int id = reader.GetInt32(0);
            string nombre = reader.GetString(1);
            decimal precio = reader.GetDecimal(2);
            int cantidad = reader.GetInt32(3);

            Console.WriteLine($"{id} | {nombre} | {precio} | {cantidad}");
        }
    }
}
