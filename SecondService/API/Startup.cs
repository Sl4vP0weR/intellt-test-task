var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;
var environment = builder.Environment;

var inDevelopment = environment.IsDevelopment();

// Add services to the container.

services.AddControllers();

configuration.AddEnvironmentVariables();
configuration.AddUserSecrets(Domain.AssemblyMarker.Assembly);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

new Application.DependencyInjection(services, configuration).Add();
new Infrastructure.DependencyInjection(services, configuration).Add();

services.AddResponseCompression(opt =>
{
    opt.EnableForHttps = true;
});

AddLoggging();

builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapControllers();

app.UseResponseCompression();

await PrepareDataBase(app);

await app.RunAsync();

async Task PrepareDataBase(WebApplication webApplication)
{
    await using(var scope = webApplication.Services.CreateAsyncScope())
    {
        var provider = scope.ServiceProvider;
    
        var db = provider.GetRequiredService<IDataBaseContext>();
        await db.MigrateAsync();
        
        if(inDevelopment)
            await db.TrySeedAsync();
    }
}

void AddLoggging()
{
    var loggerBuilder = new LoggerConfiguration()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
        .Enrich.FromLogContext()
        .Enrich.WithExceptionDetails()
        .WriteTo.Debug()
        .WriteTo.Console()
        .ReadFrom.Configuration(configuration);

    if (!builder.Environment.IsDevelopment())
    {
        loggerBuilder
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning);
    }

    Log.Logger = loggerBuilder.CreateLogger();
}