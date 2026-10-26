CREATE PROCEDURE BuscarProductos
	@Texto VARCHAR(100),
	@PrecioMaximo DECIMAL(10,2)
AS 
BEGIN
	SELECT p.Id, p.Nombre, p.Precio, c.Nombre AS Categoria
	FROM Productos p
	JOIN Categorias c ON c.Id = p.CategoriaId
	WHERE p.Nombre LIKE '%' + @Texto + '%'
	AND p.Precio <= @PrecioMaximo
END