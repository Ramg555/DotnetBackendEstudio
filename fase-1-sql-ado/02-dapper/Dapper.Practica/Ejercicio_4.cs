using System.Data;

namespace Dapper.Practica;

public static class Ejercicio_4
{
    public static async Task Ejercicio()
    {
        await using var connection = await DbConnectionFactory.OpenConnectionAsync();

        var parametros = new DynamicParameters();
        parametros.Add("@Nombre", "Saco");
        parametros.Add("@Precio", 1200);
        parametros.Add("@Cantidad", 5);
        parametros.Add("@CategoriaId", 2);
        parametros.Add("@NuevoId", dbType: DbType.Int32, direction: ParameterDirection.Output);
        await connection.ExecuteAsync("InsertarProducto", parametros ,commandType: CommandType.StoredProcedure);

        int nuevoId = parametros.Get<int>("@NuevoId");
        Console.WriteLine($"Producto creado con Id {nuevoId}");
    }
}
