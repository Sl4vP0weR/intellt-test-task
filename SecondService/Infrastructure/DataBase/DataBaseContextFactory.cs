namespace Infrastructure.DataBase;

public class DataBaseContextFactory : IDesignTimeDbContextFactory<DataBaseContext>
{   
    public DataBaseContext CreateDbContext(string[] args)
    {
        var builder = Host.CreateDefaultBuilder(args);

        builder.ConfigureHostConfiguration(configuration =>
        {
            configuration.AddUserSecrets(AssemblyMarker.Assembly);
        });
        
        builder.ConfigureServices((context, services) =>
        {
            new DependencyInjection(services, context.Configuration).AddDataBase();
        });

        var host = builder.Build();

        var scope = host.Services.CreateScope();

        return scope.ServiceProvider.GetRequiredService<DataBaseContext>();
    }
}