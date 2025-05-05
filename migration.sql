-- Criar a tabela clients
CREATE TABLE IF NOT EXISTS clients (
    "Id" SERIAL PRIMARY KEY,
    "Name" TEXT NOT NULL,
    "Lastname" TEXT NOT NULL,
    "Address" TEXT NOT NULL,
    "Age" INTEGER
);

-- Tabela EFMigrationsHistory para o Entity Framework Core
CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

-- Inserir registro de migração inicial
INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250505000000_InitialMigration', '9.0.0')
ON CONFLICT DO NOTHING;