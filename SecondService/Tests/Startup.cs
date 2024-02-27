namespace Tests;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        var applicationServices = new Application.DependencyInjection(services, null!);
        applicationServices.Add();
        
        services.AddDbContext<IDataBaseContext, DataBaseContext>(dbContext =>
        {
            dbContext.UseInMemoryDatabase(Guid.NewGuid().ToString());
        });
    }
}