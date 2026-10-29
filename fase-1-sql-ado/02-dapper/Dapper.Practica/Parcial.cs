using Dapper;
using DapperProject.Practica.Models;
using Microsoft.Data.SqlClient;

namespace DapperProject.Practica;

public static class Parcial
{
    private static readonly Task<SqlConnection> sqlConnection = DbConnectionFactory.OpenConnectionAsync();
    public static async Task EjecutarAsync()
    {
        var producto = await ObtenerProductoPorIdAsync(10);
        string texto = producto is null ? "Producto no encontrado" : $"{producto.Id} | {producto.Nombre} | {producto.Precio}";
        Console.WriteLine(texto);
        Console.WriteLine();

        var productos = await ObtenerProductosPorCategoriaAsync("Electronica");
        foreach(var p in productos)
        {
            Console.WriteLine();
        }
        var nuevoId = await CreatCategoriaAsync("Deportes");
    }

    private static async Task<Producto?> ObtenerProductoPorIdAsync(int id)
    {
        await using var connection = await sqlConnection;

        const string sql = """
            SELECT * FROM Productos WHERE Id = @Id
            """;

        return await connection.QueryFirstOrDefaultAsync<Producto>(sql, new {Id = id});
    }

    private static async Task<IEnumerable<Producto>> ObtenerProductosPorCategoriaAsync(string nombreCategoria)
    {
        await using var connection = await sqlConnection;

        const string sql = """
            SELECT p.Id, p.Nombre, p.Precio, c.Id, c.Nombre FROM Productos p
            JOIN Categorias c ON c.Id = p.CategoriaId
            WHERE c.Nombre = @NombreCategoria
            """;

        return await connection.QueryAsync<Producto, Categoria, Producto>(sql,
            (producto, categoria) =>
            {
                producto.Categoria = categoria;
                return producto;
            },
            new { nombreCategoria = new DbString { Value = nombreCategoria, IsAnsi = true, Length = 50 } },
            splitOn: "Id"
            );
    }

    private static async Task<int> CreatCategoriaAsync(string nombre)
    {

    }
}
