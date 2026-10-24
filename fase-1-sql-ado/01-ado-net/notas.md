ADO.NET

ADO.NET es la forma nativa de .NET para hablar con una base de datos. EF Core esta construido encima de ADO.NET

SqlConnection = La conexion a la base de datos
SqlCommand = El SQL o stored procedure a ejecutar
SqlParameter = Los valores que le pasas al comando
SqlDataReader = El lector de resultados, fila por fila

3 formas de ejecutar un comando:

ExecuteReaderAsync() = devuelve filas (un SELECT)
ExecuteScalarAsync() = devuelve un solo valor (un COUNT, un SUM, el Id recien insertado)
ExecuteNonQueryAsync() = no devuelve filas, solo cuantas fueron afectadas (INSERT, UPDATE, DELETE)

Flujo = Abrir conexion -> crear comando -> agregar parametros -> ejecutar -> leer -> cerrar.

ExecuteScalarAsync() puede devolver: 
	DBNull.Value: La consulta si trajo una fila pero el valor de esa filla es null entonces devuelve un DBNull.
	null: Si la consulta no devolvio ninguna fila.

Solucion: 
	decimal total = resultado is null or DBNull ? 0 : Convert.ToDecimal(resultado);

Todo lo que es IDisposable en ADO.NET va con await using