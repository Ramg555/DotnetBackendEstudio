using Microsoft.Data.SqlClient;
using System.Data;

namespace AdoNet.Practica;

public class Practica_2
{
    public static async Task Ejercicio()
    {
        await using var connection = await DbConnectionFactory.OpenConnectionAsync();

        await using var command = new SqlCommand("ActualizarPrecio", connection);
        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.Add("@Id", SqlDbType.Int).Value = 20;
        var NuevoPrecioParam = command.Parameters.Add("@NuevoPrecio", SqlDbType.Decimal);
        NuevoPrecioParam.Precision = 10;
        NuevoPrecioParam.Scale = 2;
        NuevoPrecioParam.Value = 4000;

        int resultado = await command.ExecuteNonQueryAsync();

        if (resultado > 0) Console.WriteLine("Precio Actualizado");
        else Console.WriteLine("No existe un producto con ese Id");
    }
}
