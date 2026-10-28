using Dapper;
using System.Data;

namespace DapperProject.Practica;

public static class Ejercicio_2
{
    public static async Task Ejercicio()
    {
        await using var connection = await DbConnectionFactory.OpenConnectionAsync();

        int filasAfectadas = await connection.ExecuteAsync("ActualizarPrecio", new { Id = 20, NuevoPrecio = 2000m }, commandType: CommandType.StoredProcedure);

        if (filasAfectadas > 0) Console.WriteLine("Precio Actualizado");
        else Console.WriteLine("No existe un Producto con el Id proporcionado");
    }
}
