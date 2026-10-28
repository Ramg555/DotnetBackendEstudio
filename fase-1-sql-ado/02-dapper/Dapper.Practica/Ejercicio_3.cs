using Dapper;

namespace DapperProject.Practica;

public static class Ejercicio_3
{
    public static async Task Ejercicio()
    {
        await using var connection = await DbConnectionFactory.OpenConnectionAsync();

        const string sql = """
            SELECT COUNT(*) FROM Productos
            """;

        var cantidad = await connection.ExecuteScalarAsync<int>(sql);

        Console.WriteLine($"Actualmente hay {cantidad} productos registrados");
    }
}
