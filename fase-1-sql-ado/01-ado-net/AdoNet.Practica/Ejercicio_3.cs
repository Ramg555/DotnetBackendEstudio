using Microsoft.Data.SqlClient;
using System.Data;

namespace AdoNet.Practica;

public class Ejercicio_3
{
    //Ejecutar un Store Procedure con parametros
    public static async Task Ejercicio()
    {
        decimal precioMinimo = 100;
        int categoriaId = 2;
        List<ProductoConCategoria> productos = [];

        await using var connection = await DbConnectionFactory.OpenConnectionAsync();
        await using var command = new SqlCommand("ObtenerProductosPorPrecio", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        //Parametros
        var precioParam = command.Parameters.Add("@PrecioMinimo", SqlDbType.Decimal);
        precioParam.Precision = 10;
        precioParam.Scale = 1;
        precioParam.Value = precioMinimo;
        command.Parameters.Add("@CategoriaId", SqlDbType.Int).Value = categoriaId;

        await using var reader = await command.ExecuteReaderAsync();

        int ordId = reader.GetOrdinal("Id");
        int ordNombre = reader.GetOrdinal("Nombre");
        int ordPrecio = reader.GetOrdinal("Precio");
        int ordCategoria = reader.GetOrdinal("Categoria");

        while (await reader.ReadAsync())
        {
            ProductoConCategoria producto = new
                (
                    reader.GetInt32(ordId),
                    reader.GetString(ordNombre),
                    reader.GetDecimal(ordPrecio),
                    reader.GetString(ordCategoria)
                );

            productos.Add(producto);
        }
        Console.WriteLine("Id | Nombre | Precio | Categoria");
        Console.WriteLine();

        foreach (var producto in productos)
        {
            Console.WriteLine($"{producto.Id} | {producto.Nombre} | {producto.Precio} | {producto.Categoria}");
        }
    }

    private record ProductoConCategoria(int Id, string Nombre, decimal Precio, string Categoria);
}
