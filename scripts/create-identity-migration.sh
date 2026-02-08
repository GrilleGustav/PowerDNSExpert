#!/usr/bin/env bash
set -euo pipefail

MIGRATION_NAME="${1:-}"
if [[ -z "${MIGRATION_NAME}" ]]; then
  echo "Usage: $0 <MigrationName>"
  exit 1
fi

export POWERDNS_DB_CONNECTION="${POWERDNS_DB_CONNECTION:-Host=localhost;Port=5432;Database=powerdns_expert;Username=postgres;Password=postgres}"

dotnet ef migrations add "${MIGRATION_NAME}" \
  --project src/IdentityProvider/IdentityProvider.csproj \
  --context ApplicationIdentityDbContext \
  --output-dir Migrations/ApplicationIdentity

echo "Migration '${MIGRATION_NAME}' created in src/IdentityProvider/Migrations/ApplicationIdentity"
