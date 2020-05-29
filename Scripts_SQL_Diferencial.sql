ALTER TABLE [DiarcoMas].[LOYALTY].[Clientes] ADD ValidadoNosis BIT NULL;

UPDATE [DiarcoMas].[LOYALTY].[Clientes] SET ValidadoNosis = 0;

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [LOYALTY].[sp_Clientes_Insert]
                                                @DNI                    BIGINT
                                                , @Apellido             VARCHAR(100)
                                                , @Nombre               VARCHAR(100)
                                                , @Genero               VARCHAR(1)
                                                , @FechaNacimiento      DATETIME
                                                , @Telefono             VARCHAR(20)
                                                , @Celular              VARCHAR(20)
                                                , @Direccion            VARCHAR(100)
                                                , @Postal               VARCHAR(10)
                                                , @Provincia            INT
                                                , @ProvinciaNombre      VARCHAR(100)
                                                , @Ciudad               INT
                                                , @CiudadNombre         VARCHAR(100)
                                                , @Email                VARCHAR(100)
                                                , @Grupo                INT
                                                , @Tarjeta              VARCHAR(20)
                                                , @HashValidacion       VARCHAR(100) = ''
                                                , @Enviado              INT
                                                , @Validado             BIT = 0
                                                , @ValidadoNosis        BIT = 0
AS

DECLARE @Result TABLE (Codigo CHAR(2), Mensaje VARCHAR(1000))
DECLARE @Log VARCHAR(5000)
DECLARE @mApellido          VARCHAR(100)
DECLARE @mNombre            VARCHAR(100)
DECLARE @mGenero            VARCHAR(1)
DECLARE @mFechaNacimiento   DATETIME
DECLARE @mTelefono          VARCHAR(20)
DECLARE @mCelular           VARCHAR(20)
DECLARE @mDireccion         VARCHAR(100)
DECLARE @mPostal            VARCHAR(10)
DECLARE @mProvincia         INT
DECLARE @mProvinciaNombre   VARCHAR(100)
DECLARE @mCiudad            INT
DECLARE @mCiudadNombre      VARCHAR(100)
DECLARE @mEmail             VARCHAR(100)
DECLARE @mGrupo             INT
DECLARE @mTarjeta           VARCHAR(20)
DECLARE @mHashValidacion    VARCHAR(100)
DECLARE @mEnviado           INT
DECLARE @mValidado          BIT
DECLARE @mValidadoNosis     BIT

SET @DNI = ABS(LTRIM(RTRIM(@DNI)))

SET @Log = 'LOG: Cliente - '
SET @Log = @Log + ' @DNI: ' + CAST(@DNI AS VARCHAR(10))
SET @Log = @Log + ' | @Apellido: ' + @Apellido
SET @Log = @Log + ' | @Nombre: ' + @Nombre
SET @Log = @Log + ' | @Genero: ' + @Genero
SET @Log = @Log + ' | @FechaNacimiento: ' + CAST(@FechaNacimiento AS VARCHAR(20))
SET @Log = @Log + ' | @Telefono: ' + @Telefono
SET @Log = @Log + ' | @Celular: ' + @Celular
SET @Log = @Log + ' | @Direccion: ' + @Direccion
SET @Log = @Log + ' | @Postal: ' + @Postal
SET @Log = @Log + ' | @Provincia: ' + CAST(@Provincia   AS VARCHAR(2))
SET @Log = @Log + ' | @ProvinciaNombre: ' + @ProvinciaNombre
SET @Log = @Log + ' | @Ciudad: ' + CAST(@Ciudad AS VARCHAR(6))
SET @Log = @Log + ' | @CiudadNombre: ' + @CiudadNombre
SET @Log = @Log + ' | @Email: ' + @Email
SET @Log = @Log + ' | @Grupo: ' + CAST(@Grupo AS VARCHAR(4))
SET @Log = @Log + ' | @Tarjeta: ' + @Tarjeta
SET @Log = @Log + ' | @HashValidacion: ' + @HashValidacion
SET @Log = @Log + ' | @Enviado: ' + @Enviado
SET @Log = @Log + ' | @Validado: ' + CAST(@Validado as VARCHAR)
SET @Log = @Log + ' | @ValidadoNosis: ' + CAST(@ValidadoNosis as VARCHAR)
    
EXECUTE [LOYALTY].[sp_Logs_Insert] @Log


-- VALIDACIONES
-- Fecha de Nacimiento
IF ( LEN(@DNI) > 8 )
BEGIN

    SET @Log = 'ERROR: Número de documento inválido'
    EXECUTE [LOYALTY].[sp_Logs_Insert] @Log

    INSERT INTO @Result (Codigo, Mensaje)
    VALUES ('NO', @Log)

    SELECT Codigo, Mensaje FROM @Result
    RETURN
END


-- Fecha de Nacimiento
DECLARE @FECHA DATETIME
SET @FECHA = DATEADD(YY, -18, CAST(GETDATE() AS DATE))
IF ( @FechaNacimiento > @FECHA  )
BEGIN

    SET @Log = 'ERROR: Debe ser mayor de 18 años'
    EXECUTE [LOYALTY].[sp_Logs_Insert] @Log

    INSERT INTO @Result (Codigo, Mensaje)
    VALUES ('NO', @Log)

    SELECT Codigo, Mensaje FROM @Result
    RETURN
END

-- Grupo del Convenio
IF NOT EXISTS ( SELECT * FROM LOYALTY.Grupos WHERE CAST(Codigo AS INT) = @Grupo )
BEGIN
    SET @Log = 'ERROR: Grupo ingresado no válido'
    EXECUTE [LOYALTY].[sp_Logs_Insert] @Log

    INSERT INTO @Result (Codigo, Mensaje)
    VALUES ('NO', @Log)

    SELECT Codigo, Mensaje FROM @Result
    RETURN
END



-- Direccion
IF ( LEN(LTRIM(RTRIM(@Direccion))) < 5 )
BEGIN
    SET @Log = 'ERROR: Direccion inválida'
    EXECUTE [LOYALTY].[sp_Logs_Insert] @Log

    INSERT INTO @Result (Codigo, Mensaje)
    VALUES ('NO', @Log)

    SELECT Codigo, Mensaje FROM @Result
    RETURN
END

-- @ProvinciaNombre
IF ( LEN(LTRIM(RTRIM(@ProvinciaNombre))) < 3 )
BEGIN
    SET @Log = 'ERROR: Provincia inválida'
    EXECUTE [LOYALTY].[sp_Logs_Insert] @Log

    INSERT INTO @Result (Codigo, Mensaje)
    VALUES ('NO', @Log)

    SELECT Codigo, Mensaje FROM @Result
    RETURN
END

-- @CiudadNombre
IF ( LEN(LTRIM(RTRIM(@CiudadNombre))) < 3 )
BEGIN
    SET @Log = 'ERROR: Ciudad inválida'
    EXECUTE [LOYALTY].[sp_Logs_Insert] @Log

    INSERT INTO @Result (Codigo, Mensaje)
    VALUES ('NO', @Log)

    SELECT Codigo, Mensaje FROM @Result
    RETURN
END

IF NOT EXISTS ( SELECT * FROM [LOYALTY].[Tarjetas] WITH(NOLOCK) WHERE [Codigo] = @Tarjeta )
BEGIN
    SET @Log = 'ERROR: Tarjeta inválida'
    EXECUTE [LOYALTY].[sp_Logs_Insert] @Log

    INSERT INTO @Result (Codigo, Mensaje)
    VALUES ('NO', @Log)

    SELECT Codigo, Mensaje FROM @Result
    RETURN
END

IF EXISTS ( SELECT * FROM [LOYALTY].[Clientes] WHERE LTRIM(RTRIM(Tarjeta)) = @Tarjeta ) 
BEGIN
    SET @Log = 'ERROR: Tarjeta ya asociada'
    EXECUTE [LOYALTY].[sp_Logs_Insert] @Log

    INSERT INTO @Result (Codigo, Mensaje)
    VALUES ('NO', @Log)

    SELECT Codigo, Mensaje FROM @Result
    RETURN
END

IF EXISTS ( SELECT * FROM [LOYALTY].[Empleados] WHERE DNI = @DNI )
BEGIN
    SET @Log = 'ERROR: Los empleados no pueden registrarse en la web Diarco+'
    EXECUTE [LOYALTY].[sp_Logs_Insert] @Log

    INSERT INTO @Result (Codigo, Mensaje)
    VALUES ('NO', @Log)

    SELECT Codigo, Mensaje FROM @Result
    RETURN
END


IF NOT EXISTS (SELECT * FROM [LOYALTY].[Clientes] WHERE DNI = @DNI )
BEGIN
    INSERT INTO [LOYALTY].[Clientes]
    (
        DNI
        , Apellido
        , Nombre
        , Genero
        , FechaNacimiento
        , Telefono
        , Celular
        , Direccion
        , Postal
        , Provincia
        , ProvinciaNombre
        , Ciudad
        , CiudadNombre
        , Email
        , Grupo
        , Tarjeta
        , FechaAlta
        , HashValidacion
        , Enviado
        , Validado
        , ValidadoNosis
    )
    VALUES
    (
        @DNI
        , @Apellido
        , @Nombre
        , @Genero
        , @FechaNacimiento
        , @Telefono
        , @Celular
        , @Direccion
        , @Postal
        , @Provincia
        , @ProvinciaNombre
        , @Ciudad
        , @CiudadNombre
        , @Email
        , @Grupo
        , @Tarjeta
        , GETDATE()
        , @HashValidacion
        , @Enviado
        , @Validado
        , @ValidadoNosis
    )

    SET @Log = 'ALTA: Cliente - '
    SET @Log = @Log + ' @DNI: ' + CAST(@DNI AS VARCHAR(10))
    SET @Log = @Log + ' | @Apellido: ' + @Apellido
    SET @Log = @Log + ' | @Nombre: ' + @Nombre
    SET @Log = @Log + ' | @Genero: ' + @Genero
    SET @Log = @Log + ' | @FechaNacimiento: ' + CAST(@FechaNacimiento AS VARCHAR(20))
    SET @Log = @Log + ' | @Telefono: ' + @Telefono
    SET @Log = @Log + ' | @Celular: ' + @Celular
    SET @Log = @Log + ' | @Direccion: ' + @Direccion
    SET @Log = @Log + ' | @Postal: ' + @Postal
    SET @Log = @Log + ' | @Provincia: ' + CAST(@Provincia   AS VARCHAR(2))
    SET @Log = @Log + ' | @ProvinciaNombre: ' + @ProvinciaNombre
    SET @Log = @Log + ' | @Ciudad: ' + CAST(@Ciudad AS VARCHAR(6))
    SET @Log = @Log + ' | @CiudadNombre: ' + @CiudadNombre
    SET @Log = @Log + ' | @Email: ' + @Email
    SET @Log = @Log + ' | @Grupo: ' + CAST(@Grupo AS VARCHAR(4))
    SET @Log = @Log + ' | @Tarjeta: ' + @Tarjeta
    SET @Log = @Log + ' | @HashValidacion: ' + @HashValidacion
    SET @Log = @Log + ' | @Enviado: ' + @Enviado
    SET @Log = @Log + ' | @Validado: ' + CAST(@Validado as VARCHAR)
    SET @Log = @Log + ' | @ValidadoNosis: ' + CAST(@ValidadoNosis as VARCHAR)
    
    EXECUTE [LOYALTY].[sp_Logs_Insert] @Log

END
ELSE
BEGIN
    SELECT
            @mApellido = Apellido
            , @mNombre = Nombre
            , @mGenero = Genero
            , @mFechaNacimiento = FechaNacimiento
            , @mTelefono = Telefono
            , @mCelular = Celular
            , @mDireccion = Direccion
            , @mPostal = Postal
            , @mProvincia = Provincia
            , @mProvinciaNombre = ProvinciaNombre
            , @mCiudad = Ciudad
            , @mCiudadNombre = CiudadNombre
            , @mEmail = Email
            , @mGrupo = Grupo
            , @mTarjeta = Tarjeta
            , @mHashValidacion = HashValidacion
            , @mEnviado = Enviado
            , @mValidado = Validado
            , @mValidadoNosis = ValidadoNosis
    FROM    [LOYALTY].[Clientes] 
    WHERE   DNI = @DNI

    SET @Log = 'MODI-VIEJO: Cliente - '
    SET @Log = @Log + ' @DNI: ' + CAST(@DNI AS VARCHAR(10))
    SET @Log = @Log + ' | @Apellido: ' + ISNULL(@mApellido, '')
    SET @Log = @Log + ' | @Nombre: ' + ISNULL(@mNombre, '')
    SET @Log = @Log + ' | @Genero: ' + ISNULL(@mGenero, '')
    SET @Log = @Log + ' | @FechaNacimiento: ' + CAST(ISNULL(@mFechaNacimiento, '') AS VARCHAR(20))
    SET @Log = @Log + ' | @Telefono: ' + ISNULL(@mTelefono, '')
    SET @Log = @Log + ' | @Celular: ' + ISNULL(@mCelular, '')
    SET @Log = @Log + ' | @Direccion: ' + ISNULL(@mDireccion, '')
    SET @Log = @Log + ' | @Postal: ' + ISNULL(@mPostal, '')
    SET @Log = @Log + ' | @Provincia: ' + CAST(ISNULL(@mProvincia, '')  AS VARCHAR(2))
    SET @Log = @Log + ' | @ProvinciaNombre: ' + ISNULL(@mProvinciaNombre, '')
    SET @Log = @Log + ' | @Ciudad: ' + CAST(ISNULL(@mCiudad, '') AS VARCHAR(6))
    SET @Log = @Log + ' | @CiudadNombre: ' + ISNULL(@mCiudadNombre, '')
    SET @Log = @Log + ' | @Email: ' + ISNULL(@mEmail, '')
    SET @Log = @Log + ' | @Grupo: ' + CAST(ISNULL(@mGrupo, '') AS VARCHAR(4))
    SET @Log = @Log + ' | @Tarjeta: ' + ISNULL(@mTarjeta, '')
    SET @Log = @Log + ' | @HashValidacion: ' + @mHashValidacion
    SET @Log = @Log + ' | @Enviado: ' + @mEnviado
    SET @Log = @Log + ' | @Validado: ' + CAST(@mValidado as VARCHAR)
    SET @Log = @Log + ' | @ValidadoNosis: ' + CAST(@mValidadoNosis as VARCHAR)
    
    EXECUTE [LOYALTY].[sp_Logs_Insert] @Log


    SET @Log = 'MODI-NUEVO: Cliente - '
    SET @Log = @Log + ' @DNI: ' + CAST(@DNI AS VARCHAR(10))
    SET @Log = @Log + ' | @Apellido: ' + ISNULL(@Apellido, '')
    SET @Log = @Log + ' | @Nombre: ' + ISNULL(@Nombre, '')
    SET @Log = @Log + ' | @Genero: ' + ISNULL(@Genero, '')
    SET @Log = @Log + ' | @FechaNacimiento: ' + CAST(ISNULL(@FechaNacimiento, '') AS VARCHAR(20))
    SET @Log = @Log + ' | @Telefono: ' + ISNULL(@Telefono, '')
    SET @Log = @Log + ' | @Celular: ' + ISNULL(@Celular, '')
    SET @Log = @Log + ' | @Direccion: ' + ISNULL(@Direccion, '')
    SET @Log = @Log + ' | @Postal: ' + ISNULL(@Postal, '')
    SET @Log = @Log + ' | @Provincia: ' + CAST(ISNULL(@Provincia, '')   AS VARCHAR(2))
    SET @Log = @Log + ' | @ProvinciaNombre: ' + ISNULL(@ProvinciaNombre, '')
    SET @Log = @Log + ' | @Ciudad: ' + CAST(ISNULL(@Ciudad, '') AS VARCHAR(6))
    SET @Log = @Log + ' | @CiudadNombre: ' + ISNULL(@CiudadNombre, '')
    SET @Log = @Log + ' | @Email: ' + ISNULL(@Email, '')
    SET @Log = @Log + ' | @Grupo: ' + CAST(ISNULL(@Grupo, '') AS VARCHAR(4))
    SET @Log = @Log + ' | @Tarjeta: ' + ISNULL(@Tarjeta, '')
    SET @Log = @Log + ' | @HashValidacion: ' + @HashValidacion
    SET @Log = @Log + ' | @Enviado: ' + @mEnviado
    SET @Log = @Log + ' | @Validado: ' + CAST(@Validado as VARCHAR) 
    SET @Log = @Log + ' | @ValidadoNosis: ' + CAST(@ValidadoNosis as VARCHAR)   

    EXECUTE [LOYALTY].[sp_Logs_Insert] @Log

    SET @Log = 'ERROR: El cliente ya existe, no puede modificarse'
    EXECUTE [LOYALTY].[sp_Logs_Insert] @Log


    INSERT INTO @Result (Codigo, Mensaje)
    VALUES ('NO', @Log)

    SELECT Codigo, Mensaje FROM @Result
    RETURN
    /*
    UPDATE [LOYALTY].[Clientes]
    SET 
        Apellido = @Apellido
        , Nombre = @Nombre
        , Genero = @Genero
        , FechaNacimiento = @FechaNacimiento
        , Telefono = @Telefono
        , Celular = @Celular
        , Direccion = @Direccion
        , Postal = @Postal
        , Provincia = @Provincia
        , ProvinciaNombre = @ProvinciaNombre
        , Ciudad = @Ciudad
        , CiudadNombre = @CiudadNombre
        , Email = @Email
        , Grupo = @Grupo
        , Tarjeta = @Tarjeta
        , FechaModif = GETDATE()
    WHERE   DNI = @DNI
    */



END


INSERT INTO @Result (Codigo, Mensaje)
VALUES ('SI', 'SI')

SELECT Codigo, Mensaje FROM @Result
 


GO
