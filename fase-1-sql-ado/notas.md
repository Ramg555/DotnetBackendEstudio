------------------------- ADO.NET -----------------------

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

---------------- DAPPER -------------------

Dapper es un micro-ORM donde escribes por completo el SQL pero te ahorras el GetOrdinal, GetString, GetInt32 o sea todo el mapeo.
Funciona como metodos de extension sobre la conexion

Metodo  | Equivalete a ADO.NET

QueryAsync<T> | ExecuteReader + mapear todas las filas
QueryFirstOrDefaultAsync<T> | ExecuteReader + leer una fila o null
ExecuteScalarAsync<T> | ExecuteScalar ya convertido a tipo
ExecuteAsync | ExecuteNonQuery

Dapper mapea por nombre: la columna Nombre del resultado se asigna a la propiedad Nombre de tu clase.
Dapper recibe los parametros como un objeto anonimo: new {categoriaId = 3} 

Para evitar el SQL injection basta con declarar los parametros correctamente, la forma en la que podria ser vulnerable seria que
se concatene o interpole lo que escribio el usuario.

Dapper envia los string como NVARCHAR(4000) como NVARCHAR tiene mayor precedencia que VARCHAR, 
SQL Server convierte la columna para compararla y eso impide usar el indice(pasa de buscar a recorrer toda la tabla) 
MYSQL = LONGTEXT 
SQL SERVER = NVARCHAR(4000)

Si mapeas a un record posicional donde no coinciden los nombres Dapper no encuentra un constructor cuyo parametros coincidan en nombre y tipo con las columnas del resultado

---------------- SQL Server --------------

Tipos de Datos:

-Numericos

INT: enteros hasta 2,100 millones. Lo normal para Ids
BIGINT: cuando INT puede quedarse corto (tablas con miles de millones de filas)
DECIMAL(p,s): numericos exactos. DECIMAL(10,2) = 10 digitos en total, 2 decimales. Es el tipo para dinero.
FLOAT: numeros aproximados. Para calculos cientificos, nunca por dinero.

-Texto 
VARCHAR(n): texto de longitud variable, 1 byte por caracter.
NVARCHAR(n): texto Unicode, 2 bytes por caracter. Soporta cualquier idioma y emojis.
CHAR(n): longitud fija; rellena con espacios. Util para codigos de tamaño fijo.
VARCHAR(MAX) / NVARCHAR(MAX): textos muy largos.
TEXT Y NTEXT: obsoletos

-Fechas
DATE: solo fecha
DATETIME2: fecha y hora con precision. Preferible a DATETIME,  que es el tipo antiguo.
DATETIMEOFFSET: fecha y hora con zona horaria.

-Otros
BIT: 0 o 1. El booleano de SQL Server
UNIQUEIDENTIFIER: un GUID.

Restricciones(constraints)

Restriccion | Que garantiza

PRIMARY KEY | Idenfitificador unico de cada fila; no nulo
FOREIGN KEY | Que el valor exista en otra tabla
UNIQUE | Que no haya valores repetidos
CHECK | Que el valor cumpla una condicion
DEFAULT | Un valor cuando no se especifica uno
NOT NULL | Que el valor siempre exista

Buena practica: nombrar las restricciones. Si no, SQL Server inventa nombres random, y el dia que tengas que eliminarla o te salga un error, no 
se sabra cual es.

CONSTRAINT PK_Productos PRIMARY KEY (Id)
CONSTRAINT FK_Productos_Categorias FOREIGN KEY (CategoriaId) REFERENCES Categorias(Id)
CONSTRAINT UQ_Categorias_Nombre UNIQUE (Nombre)
CONSTRAINT CK_Productos_Precio CHECK (Precio > 0),
CONSTRAINT DF_Productos_Activo DEFAULT 1 FOR Activo

-Clausulas

Paso | Clausula | Que hace

1 | FROM/JOIN | Obtiene las filas de origen
2 | WHERE | Filtra las filas
3 | GROUP BY | Agrupa
4 | HAVING | Filtra grupos
5 | SELECT | Calcula columnas y alias
6 | DISTINCT | Elimina duplicados
7 | ORDER BY | Ordena
8 | OFFSET/FETCH | Toma una porcion

Operadores de filtro

WHERE Precio > 500 - Donde precio sea mayor a 500
WHERE Estado <> 'Cancelado' - Donde Estado sea distinto de Cancelado
WHERE Precio BETWEEN 200 AND 1500 - Donde precio este entre 200 y 500, incluye ambos extremos
WHERE CategoriaId IN (1,2) - Donde CategoriaId el valor sea 1 o 2
WHERE Nombre LIKE 'Lap%' - Donde nombre empieze con "Lap"
WHERE Nombre LIKE '%gel' - Donde nombre termina con "gel"
WHERE Nombre LIKE '_apiz' - Donde nombre tenga exactamente apiz, "_" = exactamente un caracter

NULL no es un valor, significa desconocido, cualquier comparacion con NULL da desconocido, no verdadero ni falso. 
WHERE solo conserva las filas donde la condicion es verdadera.

WHERE Ciudad <> 'Chihuahua' no devuelve los clientes sin ciudad.
Se tienen que pedir explicitamente = WHERE Ciudad <> 'Chihuahua' OR Ciudad IS NULL

-Precedencia de AND y OR

AND se evalua antes que OR, igual que la multiplicacion antes que la suma. Cuando se utilice ambos es mejor utilizar parentesis.

-Filtrar fechas correctamente

WHERE Fecha >= '2026-03-01' AND Fecha < '2026-04-01'

Cuidado con BETWEEN en columnas con hora: BETWEEN '2026-03-01' AND '2026-03-31' excluye casi todo el dia 31
porque ''2026-03-31 equivale a 2026-03-31 00:00:00

-TOP y paginacion

SELECT TOP (3) Nombre, Precio
FROM Productos
ORDER BY Precio DESC

La paginacion estandar en SQL Server es OFFSET ... FETCH, y exige un ORDER BY:

DECLARE @Pagina INT = 2;
DECLARE @TamanoPagina INT = 5;

SELECT Id, Nombre, Precio
FROM Productos
ORDER BY Nombre, Id
OFFSET (@Pagina - 1) * @TamanoPagina ROWS
FETCH NEXT @TamanoPagina ROWS ONLY;

------------ JOINS --------------

JOIN | Que filas conserva

INNER JOIN | Solo las que tienen pareja en ambas tablas
LEFT JOIN | Todas las de la izquierda; las de la derecha si hay pareja, si no, NULL
RIGHT JOIN | Lo contrario. Casi no se usa: se escribe como LEFT invirtiendo las tablas
FULL JOIN | Todas las de ambos lados, con NULL donde no hay pareja
CROSS JOIN | Cada fila de una con cada fila de la otra (producto cartesiano)

JOIN a secas es INNER JOIN

Cada JOIN se agrega al resultado del anterior:

SELECT p.Id, c.Nombre AS Cliente, pr.Nombre AS Producto, pd.Cantidad
FROM Pedidos p
JOIN Clientes c ON c.Id = p.ClienteId
JOIN PedidoDetalle pd ON p.PedidoId = p.Id
JOIN Productos pr ON pr.Id = pd.ProductoId;
