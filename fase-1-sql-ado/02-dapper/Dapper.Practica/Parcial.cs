using Dapper;
using DapperProject.Practica.Models;

namespace DapperProject.Practica;

public static class Parcial
{
    public static async Task EjecutarAsync()
    {
        var producto = await ObtenerProductoPorIdAsync(10);
        string texto = producto is null ? "Producto no encontrado" : $"{producto.Id} | {producto.Nombre} | {producto.Precio}";
        Console.WriteLine(texto);
        Console.WriteLine();

        var productos = await ObtenerProductosPorCategoriaAsync("Electronica");
        foreach (var p in productos)
        {
            Console.WriteLine($"{p.Id} | {p.Nombre} | {p.Precio} | {p.Categoria?.Nombre ?? "Sin Categoria"} (Id {p.Categoria?.Id ?? 0})");
        }
        Console.WriteLine();

        var nuevoId = await CreatCategoriaAsync("Deportes");
        Console.WriteLine($"Nuevo Id: {nuevoId}");
    }

    private static async Task<Producto?> ObtenerProductoPorIdAsync(int id)
    {
        await using var connection = await DbConnectionFactory.OpenConnectionAsync();

        const string sql = """
            SELECT * FROM Productos WHERE Id = @Id
            """;

        return await connection.QueryFirstOrDefaultAsync<Producto>(sql, new {Id = id});
    }

    private static async Task<IEnumerable<Producto>> ObtenerProductosPorCategoriaAsync(string nombreCategoria)
    {
        await using var connection = await DbConnectionFactory.OpenConnectionAsync();

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
        await using var connection = await DbConnectionFactory.OpenConnectionAsync();

        const string sql = """
            INSERT Categorias(Nombre) VALUES(@Nombre);
            SELECT CAST(SCOPE_IDENTITY() AS INT)
            """;

        return await connection.ExecuteScalarAsync<int>(sql, new { nombre = new DbString { Value = nombre, IsAnsi = true, Length = 50 } });
    }
}
