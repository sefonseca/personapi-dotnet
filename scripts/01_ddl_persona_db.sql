/* =========================================================
   personapi-dotnet - Script DDL
   Motor: SQL Server 2025 Express
   Crea la base de datos persona_db (propietario sa) y sus tablas
   ========================================================= */

USE master;
GO

IF DB_ID('persona_db') IS NULL
    CREATE DATABASE persona_db;
GO

ALTER AUTHORIZATION ON DATABASE::persona_db TO sa;
GO

USE persona_db;
GO

/* Borrado en orden inverso a las dependencias (permite re-ejecutar el script) */
IF OBJECT_ID('dbo.telefono', 'U')  IS NOT NULL DROP TABLE dbo.telefono;
IF OBJECT_ID('dbo.estudios', 'U')  IS NOT NULL DROP TABLE dbo.estudios;
IF OBJECT_ID('dbo.persona', 'U')   IS NOT NULL DROP TABLE dbo.persona;
IF OBJECT_ID('dbo.profesion', 'U') IS NOT NULL DROP TABLE dbo.profesion;
GO

/* ---------- profesion ---------- */
CREATE TABLE dbo.profesion (
    id   INT          NOT NULL,
    nom  VARCHAR(90)  NOT NULL,
    des  VARCHAR(MAX) NULL,          -- TEXT en el modelo; TEXT está obsoleto en SQL Server
    CONSTRAINT PK_profesion PRIMARY KEY (id)
);
GO

/* ---------- persona ---------- */
CREATE TABLE dbo.persona (
    cc        INT         NOT NULL,
    nombre    VARCHAR(45) NOT NULL,
    apellido  VARCHAR(45) NOT NULL,
    genero    CHAR(1)     NOT NULL,  -- ENUM('M','F') del modelo, implementado con CHECK
    edad      INT         NULL,
    CONSTRAINT PK_persona PRIMARY KEY (cc),
    CONSTRAINT CK_persona_genero CHECK (genero IN ('M', 'F'))
);
GO

/* ---------- estudios (relación N:M persona-profesion) ---------- */
CREATE TABLE dbo.estudios (
    id_prof  INT         NOT NULL,
    cc_per   INT         NOT NULL,
    fecha    DATE        NULL,
    univer   VARCHAR(50) NULL,
    CONSTRAINT PK_estudios PRIMARY KEY (id_prof, cc_per),
    CONSTRAINT estudio_persona_fk FOREIGN KEY (cc_per)
        REFERENCES dbo.persona (cc),
    CONSTRAINT estudio_profesion_fk FOREIGN KEY (id_prof)
        REFERENCES dbo.profesion (id)
);
GO

/* ---------- telefono ---------- */
CREATE TABLE dbo.telefono (
    num     VARCHAR(15) NOT NULL,
    oper    VARCHAR(45) NOT NULL,
    duenio  INT         NOT NULL,
    CONSTRAINT PK_telefono PRIMARY KEY (num),
    CONSTRAINT telefono_persona_fk FOREIGN KEY (duenio)
        REFERENCES dbo.persona (cc)
);
GO
