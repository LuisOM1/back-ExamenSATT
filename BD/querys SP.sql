

CREATE DATABASE RRHH


CREATE TABLE TA_AREA(
	id_area int primary key identity,
	name_area varchar(50)
)

Select * from TA_AREA

CREATE TABLE TA_EMPL(
	id_empl int primary key identity,
	id_area int references TA_AREA(id_area),
	nomb_empl varchar(50),
	ape_empl varchar(50),
	emai_empl varchar(50),
	suel_empl decimal (10,2),
	fech_ingr date,
	esta char(1)
)

select * from TA_EMPL

insert into TA_AREA(name_area) values
       ('Administracion'),
	   ('Sistemas')

insert into TA_EMPL(id_area, nomb_empl, ape_empl, emai_empl, suel_empl,
                    fech_ingr, esta)  values
		(1, 'Katherine', 'Espinoza Diaz', 'kespinoza@seisi.com', 4500,
		'02/10/1995', 'A')

select * from TA_EMPL


CREATE PROCEDURE SP_REGI_EMPL
    @id_area INT,
    @nomb_empl VARCHAR(50),
    @ape_empl VARCHAR(50),
    @emai_empl VARCHAR(50),
    @suel_empl DECIMAL(10,2),
    @fech_ingr DATE
AS
BEGIN
    -- Mejora el rendimiento al no enviar mensajes de "(1 filas afectadas)"
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;
        INSERT INTO TA_EMPL (id_area, nomb_empl, ape_empl, emai_empl, suel_empl, fech_ingr, esta)
        VALUES (@id_area, @nomb_empl, @ape_empl, @emai_empl, @suel_empl, @fech_ingr, 'A');
        -- Confirmar la transacción
        COMMIT TRANSACTION;
        PRINT 'Empleado registrado exitosamente.';
    END TRY

    BEGIN CATCH
        -- Si ocurre un error, deshacemos cualquier cambio
        ROLLBACK TRANSACTION;
        -- Captura detalles del error
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();

        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END;
GO

EXEC SP_REGI_EMPL
    @id_area = 1, 
    @nomb_empl = 'Juan', 
    @ape_empl = 'Pérez', 
    @emai_empl = 'jperez@seisi.com', 
    @suel_empl = 2500.00, 
    @fech_ingr = '10/03/1990';


CREATE PROCEDURE SP_LIST_ALL_EMPL
AS
BEGIN
    -- Mejora el rendimiento eliminando mensajes de conteo
    SET NOCOUNT ON;

    BEGIN TRY
        -- evitar SELECT *
		-- WITH (NOLOCK) permite que la consulta no se detenga si 
		-- otra persona esta insertando empleados en este momento
        SELECT 
            E.id_empl,
			E.id_area,
            E.nomb_empl,
            E.ape_empl,
            E.emai_empl,
            E.suel_empl,
            E.fech_ingr,
            E.esta,
            A.name_area
        FROM TA_EMPL AS E WITH (NOLOCK)
        INNER JOIN TA_AREA AS A WITH (NOLOCK) ON E.id_area = A.id_area
		WHERE E.esta = 'A'
        ORDER BY E.id_empl ASC;
    END TRY

    BEGIN CATCH
        -- Manejo de errores en ambiente productivo
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();

        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END;
GO

EXECUTE SP_LIST_ALL_EMPL;


CREATE PROCEDURE SP_LIST_EMPL
    @id_empl INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        IF (@id_empl IS NULL OR @id_empl <= 0)
        BEGIN
            PRINT('El ID de empleado proporcionado no es válido.');
            RETURN;
        END
        -- Consulta con JOIN y NOLOCK para máxima eficiencia
        SELECT 
            E.id_empl,
            E.id_area,
            A.name_area,
            E.nomb_empl,
            E.ape_empl,
            E.emai_empl,
            E.suel_empl,
            E.fech_ingr,
            E.esta
        FROM TA_EMPL AS E WITH (NOLOCK)
        INNER JOIN TA_AREA AS A WITH (NOLOCK) ON E.id_area = A.id_area
        WHERE E.id_empl = @id_empl AND E.esta = 'A';

        -- Verifica si se encontró el registro
        IF @@ROWCOUNT = 0
        BEGIN
            PRINT 'No se encontró ningún empleado con el ID proporcionado.';
        END
    END TRY

    BEGIN CATCH
        DECLARE @Msg NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @Sev INT = ERROR_SEVERITY();
        DECLARE @Sta INT = ERROR_STATE();
        RAISERROR(@Msg, @Sev, @Sta);
    END CATCH
END;
GO

EXEC SP_LIST_EMPL @id_empl = 2;

CREATE PROCEDURE SP_EDIT_EMPL
    @id_empl INT,
    @id_area INT,
    @nomb_empl VARCHAR(50),
    @ape_empl VARCHAR(50),
    @emai_empl VARCHAR(50),
    @suel_empl DECIMAL(10,2),
    @fech_ingr DATE
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Valida si el empleado existe
        IF NOT EXISTS (SELECT 1 FROM TA_EMPL WHERE id_empl = @id_empl)
        BEGIN
            PRINT('El empleado con el ID especificado no existe.');
            RETURN;
        END

        -- Iniciar transacción
        BEGIN TRANSACTION;
        UPDATE TA_EMPL
        SET 
            id_area = @id_area,
            nomb_empl = @nomb_empl,
            ape_empl = @ape_empl,
            emai_empl = @emai_empl,
            suel_empl = @suel_empl,
            fech_ingr = @fech_ingr
        WHERE id_empl = @id_empl;
        -- Confirmar cambios
        COMMIT TRANSACTION;
        PRINT 'Empleado actualizado correctamente.';
    END TRY

    BEGIN CATCH
        ROLLBACK TRANSACTION;
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();
        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END;
GO

CREATE PROCEDURE SP_ELIM_EMPL
    @id_empl INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;
        
		UPDATE TA_EMPL
        SET esta = 'I'
        WHERE id_empl = @id_empl;

        COMMIT TRANSACTION;
        PRINT 'Empleado eliminado correctamente.';
    END TRY

    BEGIN CATCH
        ROLLBACK TRANSACTION;
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();
        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END;
GO









