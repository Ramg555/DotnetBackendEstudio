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

1.Supongo que para hacerlo de una forma mas comoda y facil de hacer y entender.
2.Existe una vulerabilidad ahi no? Creo que SQL Inyection