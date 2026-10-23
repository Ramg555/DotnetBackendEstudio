using Microsoft.Data.SqlClient;

namespace AdoNet.Practica;

public class Ejercicio_2
{
    //B)Cuantos Productos hay en total, usando ExecuteScalarAsync()
    public static async Task Ejercicio()
    {
        await using var connection = await ConnectionDatabase.Connection();
        const string sql = "SELECT COUNT(*) FROM Productos";

        await using var command = new SqlCommand(sql, connection);
        var resultado = await command.ExecuteScalarAsync();

        var cantidad = Convert.ToInt32(resultado);

        Console.WriteLine($"Existe una cantidad de {cantidad} productos");
    }
}
