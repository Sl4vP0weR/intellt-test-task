namespace Application;

public class DependencyInjection(IServiceCollection Services, IConfiguration? Configuration)
{
    public void Add()
    {
        AddMediator();
        
        AddMapping();

        AddValidation();
    }

    public void AddMediator()
    {
        Services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblyContaining<AssemblyMarker>();
        });
    }

    public void AddValidation()
    {
        Services.AddValidatorsFromAssemblies(
        [
            Domain.AssemblyMarker.Assembly, 
            AssemblyMarker.Assembly
        ]);
        Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    }

    public void AddMapping()
    {
        Services.AddAutoMapper(config =>
        {
            config.AddMaps(AssemblyMarker.Assembly);
        });
    }
}