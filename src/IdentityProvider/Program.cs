using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using IdentityProvider;
using IdentityProvider.Config;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Missing DefaultConnection connection string.");

var migrationsAssembly = typeof(Program).Assembly.GetName().Name;

builder.Services.AddDbContext<ApplicationIdentityDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
    .AddDefaultTokenProviders();

builder.Services
    .AddIdentityServer(options =>
    {
        options.EmitStaticAudienceClaim = true;
    })
    .AddAspNetIdentity<ApplicationUser>()
    .AddConfigurationStore(options =>
    {
        options.ConfigureDbContext = db => db.UseNpgsql(connectionString,
            sql => sql.MigrationsAssembly(migrationsAssembly));
    })
    .AddOperationalStore(options =>
    {
        options.ConfigureDbContext = db => db.UseNpgsql(connectionString,
            sql => sql.MigrationsAssembly(migrationsAssembly));
        options.EnableTokenCleanup = true;
        options.TokenCleanupInterval = 3600;
    })
    .AddDeveloperSigningCredential();

builder.Services.AddRazorPages();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var sp = scope.ServiceProvider;
    var configDbContext = sp.GetRequiredService<ConfigurationDbContext>();
    configDbContext.Database.Migrate();

    if (!configDbContext.Clients.Any())
    {
        configDbContext.Clients.AddRange(IdentityServerConfig.Clients.Select(x => x.ToEntity()));
        configDbContext.IdentityResources.AddRange(IdentityServerConfig.IdentityResources.Select(x => x.ToEntity()));
        configDbContext.ApiScopes.AddRange(IdentityServerConfig.ApiScopes.Select(x => x.ToEntity()));
        configDbContext.ApiResources.AddRange(IdentityServerConfig.ApiResources.Select(x => x.ToEntity()));
        configDbContext.SaveChanges();
    }

    var persistedGrantDbContext = sp.GetRequiredService<PersistedGrantDbContext>();
    persistedGrantDbContext.Database.Migrate();

    var identityDbContext = sp.GetRequiredService<ApplicationIdentityDbContext>();
    identityDbContext.Database.Migrate();

    var userManager = sp.GetRequiredService<UserManager<ApplicationUser>>();
    if (await userManager.FindByEmailAsync("admin@powerdns.local") is null)
    {
        var user = new ApplicationUser
        {
            UserName = "admin@powerdns.local",
            Email = "admin@powerdns.local",
            EmailConfirmed = true
        };

        await userManager.CreateAsync(user, "PowerDns123!");
    }
}

app.UseStaticFiles();
app.UseRouting();
app.UseIdentityServer();
app.UseAuthorization();

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));
app.MapRazorPages();

app.Run();
