using Microsoft.Data.SqlClient;
using System.Data;

namespace AdoNet.Practica;
public class Ejercicio_1
{
    //A)Productos de una categoria especifica, usando SqlParameter
    public static async Task Ejercicio()
    {
        await using var connection = await DbConnectionFactory.OpenConnectionAsync();
        int categoriaId = 3;

        const string sql = "SELECT Id, Nombre, Precio, Cantidad FROM Productos WHERE CategoriaId = @CategoriaId";

        await using var command = new SqlCommand(sql, connection);
        command.Parameters.Add("@CategoriaId", SqlDbType.Int).Value = categoriaId;
        await using var reader = await command.ExecuteReaderAsync();

        Console.WriteLine($"Productos de la categoria {categoriaId}");
        Console.WriteLine("");
        Console.WriteLine("Id | Nombre | Precio | Cantidad");
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
