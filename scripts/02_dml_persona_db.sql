/* =========================================================
   personapi-dotnet - Script DML (datos de prueba)
   Ejecutar después de 01_ddl_persona_db.sql
   ========================================================= */

USE persona_db;
GO

INSERT INTO dbo.profesion (id, nom, des) VALUES
(1, 'Ingeniería de Sistemas', 'Diseño y desarrollo de sistemas de información y software'),
(2, 'Medicina',               'Prevención, diagnóstico y tratamiento de enfermedades'),
(3, 'Derecho',                'Estudio y aplicación de normas jurídicas'),
(4, 'Contaduría Pública',     'Gestión contable, tributaria y financiera');
GO

INSERT INTO dbo.persona (cc, nombre, apellido, genero, edad) VALUES
(1010101010, 'Laura',   'Gómez',     'F', 24),
(1020202020, 'Andrés',  'Martínez',  'M', 31),
(1030303030, 'Camila',  'Rodríguez', 'F', 28),
(1040404040, 'Juan',    'Pérez',     'M', 45),
(1050505050, 'Valeria', 'Torres',    'F', NULL);
GO

INSERT INTO dbo.estudios (id_prof, cc_per, fecha, univer) VALUES
(1, 1010101010, '2023-12-15', 'Pontificia Universidad Javeriana'),
(1, 1020202020, '2018-06-20', 'Universidad Nacional de Colombia'),
(4, 1020202020, '2021-11-30', 'Universidad de los Andes'),
(2, 1030303030, '2022-07-08', 'Universidad del Rosario'),
(3, 1040404040, '2005-12-01', 'Universidad Externado de Colombia');
GO

INSERT INTO dbo.telefono (num, oper, duenio) VALUES
('3001234567', 'Claro',    1010101010),
('3159876543', 'Movistar', 1020202020),
('3204567890', 'Tigo',     1020202020),
('3012223344', 'Claro',    1030303030),
('6015551234', 'ETB',      1040404040);
GO

/* Verificación */
SELECT 'profesion' AS tabla, COUNT(*) AS registros FROM dbo.profesion
UNION ALL SELECT 'persona',  COUNT(*) FROM dbo.persona
UNION ALL SELECT 'estudios', COUNT(*) FROM dbo.estudios
UNION ALL SELECT 'telefono', COUNT(*) FROM dbo.telefono;
GO
