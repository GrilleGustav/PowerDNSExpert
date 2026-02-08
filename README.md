# PowerDNSExpert Starter Architecture

Dieses Repository enthält ein vollständiges Starter-Setup aus:

- **Identity Provider** mit **Duende IdentityServer** (`https://localhost:5001`)
- **Catalog API** mit .NET Minimal API (`http://localhost:5003`)
- **Orders API** mit .NET Minimal API (`http://localhost:5004`)
- **Gemeinsame Datenbank** via **PostgreSQL + EF Core**
- **Frontend** mit **Angular v21 + TailwindCSS** (`http://localhost:4200`)

## Architektur

Alle Backend-Projekte verwenden dieselbe PostgreSQL-Instanz (`powerdns_expert`) und laufen auf unterschiedlichen Ports.

## Schnellstart

1. PostgreSQL starten:
   ```bash
   docker compose up -d
   ```
2. Identity Provider starten:
   ```bash
   dotnet run --project src/IdentityProvider
   ```
3. Catalog API starten:
   ```bash
   dotnet run --project src/CatalogApi
   ```
4. Orders API starten:
   ```bash
   dotnet run --project src/OrdersApi
   ```
5. Frontend starten:
   ```bash
   cd frontend/powerdns-expert
   npm install
   npm start
   ```

## Hinweise

- Standard-Benutzer wird beim Start des IDP erzeugt:
  - Benutzer: `admin@powerdns.local`
  - Passwort: `PowerDns123!`
- Connection String ist in allen Backends auf dieselbe DB gesetzt.

## EF Core Migrations erstellen

Du hast in diesem Setup **zwei Migrations-Bereiche**:

1. **Business-Daten** (`AppDbContext`) im Projekt `PowerDNSExpert.Data`
2. **IdentityServer/Identity-Daten** (Konfig + Persisted Grants + Users) im Projekt `IdentityProvider`

### 1) Migration für `AppDbContext` erstellen

```bash
./scripts/create-migration.sh InitialBusinessSchema
```

Oder direkt mit `dotnet ef`:

```bash
dotnet ef migrations add InitialBusinessSchema \
  --project src/Shared/PowerDNSExpert.Data/PowerDNSExpert.Data.csproj \
  --output-dir Migrations
```

### 2) Migration für `ApplicationIdentityDbContext` erstellen

```bash
./scripts/create-identity-migration.sh InitialIdentitySchema
```

Oder direkt mit `dotnet ef`:

```bash
dotnet ef migrations add InitialIdentitySchema \
  --project src/IdentityProvider/IdentityProvider.csproj \
  --context ApplicationIdentityDbContext \
  --output-dir Migrations/ApplicationIdentity
```

> Für die Duende-`ConfigurationDbContext` und `PersistedGrantDbContext` werden i.d.R. ebenfalls eigene Migrationen erzeugt, falls noch nicht vorhanden.

### Migrationen anwenden

```bash
# Business-Daten
dotnet ef database update \
  --project src/Shared/PowerDNSExpert.Data/PowerDNSExpert.Data.csproj

# Identity-Daten
dotnet ef database update \
  --project src/IdentityProvider/IdentityProvider.csproj \
  --context ApplicationIdentityDbContext
```

Das `IdentityProvider`-Projekt ruft beim Start außerdem `Database.Migrate()` für seine Kontexte auf.

