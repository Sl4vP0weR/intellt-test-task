namespace Infrastructure;

public class DependencyInjection(IServiceCollection Services, IConfiguration Configuration)
{   
    public void Add()
    {
        AddMessaging();
    }

    private void AddMessaging()
    {
        const string 
            HostKey = "RABBITMQ_HOST", 
            UsernameKey = "RABBITMQ_USER", 
            PasswordKey = "RABBITMQ_PASSWORD";
        
        Services.AddMassTransit(x =>
        {   
            x.UsingRabbitMq((context, config) =>
            {
                config.Host(Configuration[HostKey], "/", h =>
                {
                    h.Username(Configuration[UsernameKey]);
                    h.Password(Configuration[PasswordKey]);
                });
            });
        });
    }
}