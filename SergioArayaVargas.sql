Use [recdes5]
GO

CREATE TABLE tst.Usuariotest(
    IdUsuario INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    Usuario VARCHAR(50) NOT NULL,
    Nombre VARCHAR(50) NOT NULL,
    Apellido1 VARCHAR(50) NULL,
    Apellido2 VARCHAR(50) NULL,
    Habilitado BIT NOT NULL
);

INSERT INTO tst.Usuariotest
    (
   	 Usuario,
   	 Nombre,
   	 Apellido1,
   	 Apellido2,
   	 Habilitado
    )
VALUES
    (
   	 'Usuario 01',
   	 'Sergio',
   	 'Araya',
   	 'Vargas',
   	 1    
    ),
    (
   	 'Usuario 02',
   	 'Ruben',
   	 'Porras',
   	 'Sanchez',
   	 1    
    )

CREATE PROCEDURE [tst].[sp_InsertAndUpdateUsuariotest]
    (
   	 @p_IdUsuario INT,
   	 @p_Usuario varchar(50),
   	 @p_Nombre varchar(50),
   	 @p_Apellido1 varchar(50),
   	 @p_Apellido2 varchar(50),
   	 @p_Habilitado bit,
   	 @OperationResult INT OUTPUT
    )
AS
BEGIN

    SET NOCOUNT ON;

    BEGIN TRY
   	 IF(@p_IdUsuario < 0)
   		 BEGIN
   			 INSERT INTO tst.Usuariotest
   				 (
   					 Usuario,
   					 Nombre,
   					 Apellido1,
   					 Apellido2,
   					 Habilitado
   				 )
   			 VALUES
   				 (
   					 @p_Usuario,
   					 @p_Nombre,
   					 @p_Apellido1,
   					 @p_Apellido2,
   					 @p_Habilitado
   				 );
   		 END
   	 ELSE
   		 BEGIN

   			 UPDATE tst.Usuariotest SET
   				 Nombre = @p_Nombre,
   				 Apellido1 = @p_Apellido1,
   				 Apellido2 = @p_Apellido2,
   				 Habilitado = @p_Habilitado
   			 WHERE IdUsuario = @p_IdUsuario

   		 END
   	 SET @OperationResult = 1;

    END TRY

    BEGIN CATCH

   	 ROLLBACK
   	 PRINT ERROR_MESSAGE()
   	 SET @OperationResult = 0;

    END CATCH

    SELECT @OperationResult AS result

END
GO

CREATE PROCEDURE [tst].[sp_UpdateUsuariotest]
AS 
BEGIN
SET NOCOUNT ON;

DECLARE CURSOR_HABILITADO CURSOR FOR
     SELECT IdUsuario
    FROM TST.Usuariostest
    WHERE TST.fn_countLetterByUser(IdUsuario) = 2;
OPEN CURSOR_HABILITADO;
FETCH FROM CURSOR_HABILITADO;
while(@@FETCH_STATUS)=0
begin
UPDATE TST.Usuariostest
SET Habilitado = 0 
WHERE CURRENT OF CURSOR_HABILITADO;
FETCH CURSOR_HABILITADO;
end
CLOSE CURSOR_HABILITADO;
DEALLOCATE CURSOR_HABILITADO;
END
GO

ALTER FUNCTION [tst].[fn_countLetterByUser]
(
	@IdUsuario int
)
RETURNS INT  
AS
BEGIN
	DECLARE @CONTADOR INT
	SELECT  @CONTADOR = (LEN(Usuario) - LEN(REPLACE(Usuario, 'a', '')))
	FROM   TST.Usuariostest
	where IdUsuario = @IdUsuario

	RETURN @CONTADOR
END	
GO
