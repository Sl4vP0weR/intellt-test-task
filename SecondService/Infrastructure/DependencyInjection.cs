namespace Infrastructure;

public class DependencyInjection(IServiceCollection Services, IConfiguration? Configuration)
{   
    public void Add()
    {
        AddDataBase();

        AddMediator();
        
        AddMessaging();
    }
    
    private void AddMediator()
    {
        Services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblyContaining<AssemblyMarker>();
        });
    }

    public void AddDataBase()
    {
        ArgumentNullException.ThrowIfNull(Configuration);
        
        Services.AddDbContext<IDataBaseContext, DataBaseContext>(dbContext =>
        {
            const string section = DataBaseContext.ConnectionString;
            var connectionString = Configuration[section];
        
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException(connectionString,
                    $"Connection string for database required, configuration section '{section}' was empty.");
            
            dbContext.UseNpgsql(connectionString);
        });
    }

    public void AddMessaging()
    {
        ArgumentNullException.ThrowIfNull(Configuration);
        
        const string 
            HostKey = "RABBITMQ_HOST", 
            UsernameKey = "RABBITMQ_USER", 
            PasswordKey = "RABBITMQ_PASSWORD";
        
        Services.AddMassTransit(busConfig =>
        {
            busConfig.AddConsumers(AssemblyMarker.Assembly);
            
            busConfig.UsingRabbitMq((context, config) =>
            {
                config.Host(Configuration[HostKey], "/", h =>
                {
                    h.Username(Configuration[UsernameKey]);
                    h.Password(Configuration[PasswordKey]);
                });
                
                config.ConfigureEndpoints(context);
            });
        });
    }
}
