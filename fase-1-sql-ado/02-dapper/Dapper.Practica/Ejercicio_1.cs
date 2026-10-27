using System.Data;

namespace Dapper.Practica;

public static class Ejercicio_1
{
    public static async Task Ejercicio()
    {
        await using var connection = await DbConnectionFactory.OpenConnectionAsync();

        var productos = await connection.QueryAsync<Producto>("BuscarProductos", new { Texto = "Lap", PrecioMaximo = 15000}, commandType: CommandType.StoredProcedure);

        foreach(var producto in productos)
        {
            Console.WriteLine($"{producto.Id} | {producto.Nombre} | {producto.Precio} | {producto.Categoria}");
        }
    }

    private record Producto(int Id, string Nombre, decimal Precio, string Categoria);
}
