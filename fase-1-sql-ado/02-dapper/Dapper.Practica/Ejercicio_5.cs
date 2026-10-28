using Dapper;

namespace DapperProject.Practica;

public static class Ejercicio_5
{
    public static async Task Ejercicio()
    {
        await using var connection = await DbConnectionFactory.OpenConnectionAsync();

        string sql = """
            SELECT p.Id, p.Nombre, p.Precio, c.Id, c.Nombre FROM Productos p
            JOIN Categorias c on c.Id = p.CategoriaId
            """;

        var productos = await connection.QueryAsync<Producto, Categoria, Producto>(sql,
                (producto, categoria) =>
                {
                    producto.Categoria = categoria;
                    return producto;
                },
                splitOn: "Id"
            );

        Console.WriteLine("NOMBRE | PRECIO | CATEGORIA (ID)");
        Console.WriteLine();

        foreach (var producto in productos)
        {
            Console.WriteLine($"{producto.Nombre} | {producto.Precio} | {producto.Categoria?.Nombre ?? "Sin Categoria"} | (Id {producto.Categoria?.Id})");
        }
    }
}

public class Categoria
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
}

public class Producto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public Categoria? Categoria { get; set; }
}
