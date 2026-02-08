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

Du hast in diesem Setup **vier relevante EF-Kontexte**:

1. **Business-Daten** (`AppDbContext`) im Projekt `PowerDNSExpert.Data`
2. **Identity-Userdaten** (`ApplicationIdentityDbContext`) im Projekt `IdentityProvider`
3. **IdentityServer-Konfiguration** (`ConfigurationDbContext`) im Projekt `IdentityProvider`
4. **IdentityServer-Persisted Grants** (`PersistedGrantDbContext`) im Projekt `IdentityProvider`

Wenn dir IdentityServer-Tabellen fehlen, fehlen meistens Migrationen für **3 + 4**.

### 1) Migration für `AppDbContext` erstellen

```bash
./scripts/create-migration.sh InitialBusinessSchema
```

### 2) Migration für `ApplicationIdentityDbContext` erstellen

```bash
./scripts/create-identity-migration.sh InitialIdentityUsers
```

### 3) Migration für `ConfigurationDbContext` erstellen

```bash
./scripts/create-idp-config-migration.sh InitialIdentityServerConfiguration
```

### 4) Migration für `PersistedGrantDbContext` erstellen

```bash
./scripts/create-idp-grants-migration.sh InitialIdentityServerPersistedGrants
```

### Migrationen anwenden

```bash
# 1) Business-Daten
dotnet ef database update   --project src/Shared/PowerDNSExpert.Data/PowerDNSExpert.Data.csproj

# 2) Identity-Userdaten
dotnet ef database update   --project src/IdentityProvider/IdentityProvider.csproj   --context ApplicationIdentityDbContext

# 3) IdentityServer-Konfiguration
dotnet ef database update   --project src/IdentityProvider/IdentityProvider.csproj   --context ConfigurationDbContext

# 4) IdentityServer-PersistedGrants
dotnet ef database update   --project src/IdentityProvider/IdentityProvider.csproj   --context PersistedGrantDbContext
```

Danach `dotnet run --project src/IdentityProvider` starten; beim Start werden die Kontexte zusätzlich per `Database.Migrate()` geprüft.
