using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Options;

namespace IdentityProvider;

public class PersistedGrantDbContextFactory : IDesignTimeDbContextFactory<PersistedGrantDbContext>
{
    public PersistedGrantDbContext CreateDbContext(string[] args)
    {
        var connectionString =
            Environment.GetEnvironmentVariable("POWERDNS_DB_CONNECTION")
            ?? "Host=localhost;Port=5432;Database=powerdns_expert;Username=postgres;Password=postgres";

        var migrationsAssembly = typeof(Program).Assembly.GetName().Name;

        var optionsBuilder = new DbContextOptionsBuilder<PersistedGrantDbContext>();
        optionsBuilder.UseNpgsql(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));

        return new PersistedGrantDbContext(
            optionsBuilder.Options,
            Options.Create(new OperationalStoreOptions()));
    }
}
