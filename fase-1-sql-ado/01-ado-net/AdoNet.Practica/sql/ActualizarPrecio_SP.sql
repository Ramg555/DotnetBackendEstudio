CREATE PROCEDURE ActualizarPrecio
	@Id INT,
	@NuevoPrecio DECIMAL(10,2)
AS
BEGIN
	UPDATE Productos
	SET Precio = @NuevoPrecio
	WHERE Id = @Id
END