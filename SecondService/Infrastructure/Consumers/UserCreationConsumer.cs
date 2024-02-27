namespace Infrastructure.Consumers;

public class UserCreationConsumer(
    ILogger<UserCreationConsumer> Logger, 
    IMapper Mapper,
    IMediator Mediator
) : IConsumer<CreateUser>
{    
    public Task Consume(ConsumeContext<CreateUser> context)
    {
        var request = context.Message;
        Logger.LogInformation("Bus received Create User request: {Request}", request.ToString());
        
        return Mediator.Send(request, context.CancellationToken);
    }
}