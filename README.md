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
