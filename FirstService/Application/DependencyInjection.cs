namespace Application;

public class DependencyInjection(IServiceCollection Services, IConfiguration Configuration)
{
    public void Add()
    {
        AddMediator();
        
        AddMapping();

        AddValidation();
    }
    
    private void AddMediator()
    {
        Services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblyContaining<AssemblyMarker>();
        });
    }

    private void AddValidation()
    {
        Services.AddValidatorsFromAssemblies(
        [
            Domain.AssemblyMarker.Assembly, 
            AssemblyMarker.Assembly
        ]);
        Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    }

    private void AddMapping()
    {
        Services.AddAutoMapper(config =>
        {
            config.AddMaps(AssemblyMarker.Assembly);
        });
    }
}