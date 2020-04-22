USE [DIARCOP]
GO
/****** Object:  StoredProcedure [LOYALTY].[sp_Clientes_Activar]    Script Date: 20/7/2019 15:14:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create PROCEDURE [LOYALTY].[sp_Clientes_Activar]
												@HashValidacion		VARCHAR(100)


AS

DECLARE @Result TABLE (Codigo CHAR(2), Mensaje VARCHAR(1000), Apellido VARCHAR(100), Nombre VARCHAR(100), Tarjeta varchar(20), Genero varchar(1), Email varchar(100))
DECLARE @Log VARCHAR(5000)
DECLARE @DNI BIGINT
DECLARE @Apellido VARCHAR(100)
DECLARE @Nombre VARCHAR(100)
DECLARE @Tarjeta VARCHAR(20)
DECLARE @Genero  VARCHAR(1)
DECLARE @Email VARCHAR(100)
DECLARE @Validado BIT

SET @Log = 'LOG: Cliente - '
SET @Log = @Log + ' @TOKEN: ' + @HashValidacion
	
EXECUTE [LOYALTY].[sp_Logs_Insert] @Log

SELECT @DNI = DNI,
	   @Apellido = Apellido,
	   @Nombre = Nombre,
	   @Validado = Validado,
	   @Tarjeta = Tarjeta,
	   @Genero = Genero,
	   @Email = Email
  FROM LOYALTY.Clientes
 WHERE HashValidacion = @HashValidacion

 if @@ROWCOUNT = 1
	BEGIN
		if @Validado = 1 
			BEGIN
				SET @Log = 'El token ya expiró.'
				EXECUTE [LOYALTY].[sp_Logs_Insert] @Log

				INSERT INTO @Result (Codigo, Mensaje)
				VALUES ('NO', @Log)
			END
		else
			begin
				UPDATE LOYALTY.Clientes
				   SET Validado = 1
				 WHERE DNI = @DNI

				 INSERT INTO @Result (Codigo, Mensaje, Apellido, Nombre, Tarjeta, Genero, Email)
				 VALUES ('SI', 'SI', @Apellido, @Nombre, @Tarjeta, @Genero, @Email)
			END
	END
 ELSE
	BEGIN
 		SET @Log = 'El token no existe o ya expiró.'
		EXECUTE [LOYALTY].[sp_Logs_Insert] @Log

		INSERT INTO @Result (Codigo, Mensaje)
		VALUES ('NO', @Log)
	END

	SELECT Codigo, Mensaje, Apellido, Nombre, Tarjeta, Genero, Email
	  FROM @Result

	RETURN

