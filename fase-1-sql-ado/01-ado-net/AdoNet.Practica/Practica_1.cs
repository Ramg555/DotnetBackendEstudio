using Microsoft.Data.SqlClient;
using System.Data;

namespace AdoNet.Practica;

public class Practica_1
{
    public static async Task Ejercicio()
    {

        List<Producto> productos = [];

        await using var connection = await DbConnectionFactory.OpenConnectionAsync();

        await using var command = new SqlCommand("BuscarProductos", connection);
        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.Add("@Texto", SqlDbType.VarChar, 100).Value = "Lapaz";
        var precioMaximoParam = command.Parameters.Add("@PrecioMaximo", SqlDbType.Decimal);
        precioMaximoParam.Precision = 10;
        precioMaximoParam.Scale = 2;
        precioMaximoParam.Value = 20000;

        await using var reader = await command.ExecuteReaderAsync();

        var idPosition = reader.GetOrdinal("Id");
        var nombrePosition = reader.GetOrdinal("Nombre");
        var precioPosition = reader.GetOrdinal("Precio");
        var categoriaPosition = reader.GetOrdinal("Categoria");

        while(await reader.ReadAsync())
        {
            int id = reader.GetInt32(idPosition);
            string nombre = reader.GetString(nombrePosition);
            decimal precio = reader.GetDecimal(precioPosition);
            string categoria = reader.GetString(categoriaPosition);

            Producto producto = new (id, nombre, precio, categoria);
            productos.Add(producto);
        }

        if (productos.Count == 0) Console.WriteLine("No se encontraron productos");
        else
        {
            Console.WriteLine("Resultado de busqueda");
            Console.WriteLine();
            Console.WriteLine("ID | NOMBRE | PRECIO | CATEGORIA");
            Console.WriteLine();
            foreach (var producto in productos)
            {
                Console.WriteLine($"{producto.Id} | {producto.Nombre} | {producto.Precio} | {producto.Categoria}");
            }
        }
    }

    private record Producto(int Id, string Nombre, decimal Precio, string Categoria);
}
