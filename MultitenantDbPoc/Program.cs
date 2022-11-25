using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MultitenantDbPoc.Options;
using MultitenantDbPoc.Persistence;
using MultitenantDbPoc.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// builder.Configuration.AddAzureKeyVault(new Uri(builder.Configuration["Azure:KeyVaultUri"]),
//     new DefaultAzureCredential(new DefaultAzureCredentialOptions
//     {
//         ManagedIdentityClientId = builder.Configuration["Azure:ManagedIdentityClientId"],
//     }),
//     new AzureKeyVaultConfigurationOptions
//     {
//         //ReloadInterval = TimeSpan.FromMinutes(1)
//     });

// builder.Configuration.AddAzureAppConfiguration(options =>
// {
//     var defaultAzureCredentials = new DefaultAzureCredential(
//         new DefaultAzureCredentialOptions
//         {
//             ManagedIdentityClientId = builder.Configuration["Azure:ManagedIdentityClientId"]
//         });
//
//     options.Connect(new Uri(builder.Configuration["Azure:AppConfigurationUri"]), defaultAzureCredentials);
//     options.ConfigureKeyVault(_ =>
//     {
//         _.Register(new SecretClient(new Uri(builder.Configuration["Azure:KeyVaultUri"]), defaultAzureCredentials));
//     });
//
//     //options.ConfigureRefresh(_ => _.SetCacheExpiration(TimeSpan.FromHours(1`)));
// });

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ITenantService, TenantService>();
builder.Services.Configure<DbConfiguration>(options => builder.Configuration.GetSection(nameof(DbConfiguration)).Bind(options));
builder.Services.AddDbContextFactory<ApplicationDbContext>((provider, optionsBuilder) =>
{
    // get tenant id
    var tenantService = provider.GetRequiredService<ITenantService>();

    // (optional) Get connection string name
    // We need to have strict schema for every tenant to store their configurations
    var dbConfiguration = provider.GetRequiredService<IOptionsSnapshot<DbConfiguration>>();
    if (dbConfiguration.Value.TenantConnectionStrings.TryGetValue(tenantService.GetTenantId(),
            out var tenantConnectionString))
    {
        optionsBuilder.UseSqlServer(
            string.IsNullOrWhiteSpace(tenantConnectionString)
                ? dbConfiguration.Value.DefaultConnectionString
                : tenantConnectionString);
    }

    // use connection string to create db context
    optionsBuilder.UseSqlServer(dbConfiguration.Value.DefaultConnectionString);

}, ServiceLifetime.Scoped);

var app = builder.Build();

MigrateTenantDatabases(app.Services, true);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

void MigrateTenantDatabases(IServiceProvider serviceProvider, bool migrateIfNoDb = false)
{
    using var scope = serviceProvider.CreateScope();
    // get list of all available connections strings for db context
    var optionsSnapshot = scope.ServiceProvider.GetRequiredService<IOptionsSnapshot<DbConfiguration>>();

    MigrateDb(optionsSnapshot.Value.DefaultConnectionString, migrateIfNoDb);
    foreach (var tenantConnectionString in optionsSnapshot.Value.TenantConnectionStrings)
    {
        MigrateDb(tenantConnectionString.Value, migrateIfNoDb);
    }
}

void MigrateDb(string connectionString, bool migrateIfNoDb = false)
{
    var dbContextOptionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
        .UseSqlServer(connectionString);

    using var dbContext = new ApplicationDbContext(dbContextOptionsBuilder.Options);

    if (dbContext.Database.CanConnect() || migrateIfNoDb)
    {
        dbContext.Database.Migrate();
    }
    else
    {
        throw new Exception("can't connect to db");
    }
}