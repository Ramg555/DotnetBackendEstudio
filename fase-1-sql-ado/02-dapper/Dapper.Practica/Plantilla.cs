using Dapper;

namespace DapperProject.Practica
{
    public static class Plantilla
    {
        public static async Task EjecutarAsync()
        {
            //Abrir la conexion a la BD
            await using var connection = await DbConnectionFactory.OpenConnectionAsync();

            //Crear comando
            const string sql = """
                SELECT Id, Nombre, Precio, Cantidad
                FROM Productos
                WHERE CategoriaId = @CategoriaId
                """;

            var productos = await connection.QueryAsync<Producto>(sql, new { CategoriaId = 3 });

            foreach(var producto in productos)
            {
                Console.WriteLine($"{producto.Id} | {producto.Nombre} | {producto.Precio} | {producto.Cantidad}");
            }
        }

        public record Producto(int Id, string Nombre, decimal Precio, int Cantidad);
    }
}
