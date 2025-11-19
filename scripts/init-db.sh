#!/bin/bash
set -e

echo "Waiting for PostgreSQL to be ready..."

# Esperar a que PostgreSQL esté disponible
until PGPASSWORD=$POSTGRES_PASSWORD psql -h "postgres" -U "$POSTGRES_USER" -d "$POSTGRES_DB" -c '\q'; do
  >&2 echo "PostgreSQL is unavailable - sleeping"
  sleep 1
done

>&2 echo "PostgreSQL is up - executing migrations"

# Ejecutar migraciones de Entity Framework
cd /src/Arkano.Transactions.Api
dotnet ef database update --connection "Host=postgres;Database=transactiondb;Username=postgres;Password=postgres"

>&2 echo "Migrations applied successfully"

# Iniciar la aplicación
exec dotnet Arkano.Transactions.Api.dll