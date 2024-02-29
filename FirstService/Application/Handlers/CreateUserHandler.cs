namespace Application.Handlers;

public class CreateUserHandler(IBus Bus, ILogger<CreateUserHandler> Logger) : IRequestHandler<CreateUser>
{
    public async Task Handle(CreateUser request, CancellationToken token)
    {
        await Bus.Publish(request, token);
        Logger.LogInformation("Bus published Create User request: {Request}", request.ToString());
    }
}