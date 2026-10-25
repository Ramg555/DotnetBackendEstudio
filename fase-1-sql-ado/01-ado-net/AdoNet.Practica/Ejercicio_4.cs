using Microsoft.Data.SqlClient;
using System.Data;

namespace AdoNet.Practica;

public class Ejercicio_4
{
    public static async Task Ejercicio()
    {
        //Abrir conexion a la BD
        await using var connection = await DbConnectionFactory.OpenConnectionAsync();

        //Crear Comando
        await using var command = new SqlCommand("InsertarProducto", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        //Parametros
        command.Parameters.Add("@Nombre", SqlDbType.VarChar, 100).Value = "Gorra";
        var precioParam = command.Parameters.Add("@Precio", SqlDbType.Decimal);
        precioParam.Precision = 10;
        precioParam.Scale = 2;
        precioParam.Value = 800;
        command.Parameters.Add("@Cantidad", SqlDbType.Int).Value = 10;
        command.Parameters.Add("@CategoriaId", SqlDbType.Int).Value = 2;
        var nuevoIdParam = command.Parameters.Add("@NuevoId", SqlDbType.Int);
        nuevoIdParam.Direction = ParameterDirection.Output;

        //Ejecutar Comando
        await command.ExecuteNonQueryAsync();

        Console.WriteLine($"Id del producto creado: {(int)nuevoIdParam.Value}");
    }
}
