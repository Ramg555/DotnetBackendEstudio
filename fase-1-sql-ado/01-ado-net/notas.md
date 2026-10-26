ADO.NET

ADO.NET es la forma nativa de .NET para hablar con una base de datos. EF Core esta construido encima de ADO.NET

SqlConnection = La conexion a la base de datos
SqlCommand = El SQL o stored procedure a ejecutar
SqlParameter = Los valores que le pasas al comando
SqlDataReader = El lector de resultados, fila por fila

3 formas de ejecutar un comando:

ExecuteReaderAsync() = devuelve filas (un SELECT) devuelve un reader para recorrer el resultado de la consulta fila por fila
ExecuteScalarAsync() = devuelve un solo valor (un COUNT, un SUM, el Id recien insertado)
ExecuteNonQueryAsync() = no devuelve filas, solo cuantas fueron afectadas (INSERT, UPDATE, DELETE)

Flujo = Abrir conexion -> crear comando -> agregar parametros -> ejecutar -> leer -> cerrar.

ExecuteScalarAsync() puede devolver: 
	DBNull.Value: La consulta si trajo una fila pero el valor de esa filla es null entonces devuelve un DBNull.
	null: Si la consulta no devolvio ninguna fila.

Solucion: 
	decimal total = resultado is null or DBNull ? 0 : Convert.ToDecimal(resultado);

Todo lo que es IDisposable en ADO.NET va con await using

var nuevoIdParam = command.Parameters.Add("@NuevoId", SqlDbType.Int);
nuevoIdParam.Direction = ParameterDirection.Output; 
-- Despues de ejecutar el comando, el valor esta en nuevoIdParam.Value

-Al hacer dispose a la conexion no se cierra fisicamente, la conexion real con SQL Server sigue abierta en el pool lista para reutilizarse. abrir una conexion de cero es caro
el pool evista ese costo si se vuelve a abrir.

-Con sqlParameter se evita la SQL Injection dado que el valor viaja separado del texto SQL y la bd nunca lo interpreta como codigo

-Razones por las cuales se podria elegir ADO.Net en vez de Entity Framework:
	-Rendimeinto en consultas criticas
	-Sistemas donde la logica vive en Store Procedures
	-Operaciones masivas
	-Control total del SQL generado