-- Script para sincronizar el estado de migraciones con la base de datos existente
-- Ejecutar este script en PostgreSQL

-- Crear la tabla de migraciones si no existe
CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

-- Marcar la migración inicial como aplicada (solo si no existe el registro)
INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
SELECT '20251117014118_InitialCreate', '8.0.22'
WHERE NOT EXISTS (
    SELECT 1 FROM "__EFMigrationsHistory" 
    WHERE "MigrationId" = '20251117014118_InitialCreate'
);

-- Marcar la migración de timestamp como aplicada (solo si no existe el registro)  
INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
SELECT '20251119021345_UpdateCreatedAtToTimestampWithTimeZone', '8.0.22'
WHERE NOT EXISTS (
    SELECT 1 FROM "__EFMigrationsHistory" 
    WHERE "MigrationId" = '20251119021345_UpdateCreatedAtToTimestampWithTimeZone'
);

-- Verificar que las migraciones están registradas
SELECT * FROM "__EFMigrationsHistory" ORDER BY "MigrationId";