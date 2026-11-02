--A)
SELECT p.Id, p.Fecha, p.Estado, c.Nombre AS Cliente, e.Nombre AS NombreEmpleado
FROM Pedidos p 
JOIN Clientes c ON c.Id = P.ClienteId
JOIN Empleados e ON e.Id = p.EmpleadoId;

--B)
SELECT c.Id, c.Nombre AS Categoria, p.Nombre AS Producto
FROM Categorias c
LEFT JOIN Productos p ON p.CategoriaId = c.Id; 

--C)
SELECT c.Nombre AS Cliente 
FROM Clientes c
LEFT JOIN Pedidos p ON p.ClienteId = c.Id
WHERE p.Id IS NULL;

--D)
SELECT p.Nombre AS Producto
FROM Productos p
LEFT JOIN PedidoDetalle pd ON pd.ProductoId = p.Id
WHERE pd.ProductoId IS NULL
