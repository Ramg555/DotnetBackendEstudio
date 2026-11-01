-- A)
SELECT Id, Nombre, Precio, Activo, FechaAlta 
FROM Productos 
WHERE Activo = 1 
AND Precio BETWEEN 200 AND 1500
ORDER BY Precio DESC, Nombre DESC;

--B)
SELECT * FROM Clientes WHERE Ciudad IS NULL;

--C)
SELECT * FROM Clientes WHERE Nombre LIKE '%ez';

--D)
SELECT * FROM Pedidos WHERE Fecha >= '2026-03-01' AND Fecha < '2026-04-01'

--E)
SELECT * FROM Pedidos WHERE Estado <> 'Entregado' AND Estado <> 'Cancelado';

--F)
SELECT Ciudad 
From Clientes 
WHERE Ciudad IS NOT NULL 
GROUP BY Ciudad 
ORDER BY Ciudad;

--G)
SELECT TOP(3) Nombre, Precio 
FROM Productos 
ORDER BY Precio DESC;

--H)
DECLARE @PageSize INT = 5;
DECLARE @PageNumber INT = 2;

SELECT * 
FROM PRODUCTOS 
ORDER BY Nombre
OFFSET (@PageNumber - 1) * @PageSize ROWS
FETCH NEXT @PageSize ROWS ONLY;

--I)
SELECT * FROM Productos
WHERE (CategoriaId = 1 OR CategoriaId = 2) AND Stock < 20;

SELECT * FROM Productos
WHERE CategoriaId = 1 OR CategoriaId = 2 AND Stock < 20;
--Trae 7 porque se ejecuta primero el AND (CategoriaId = 2 AND Stock < 20) se trae los productos que cumplan esa condicion
--Y despues cae el OR trayendo tambien los de CategoriaId 1