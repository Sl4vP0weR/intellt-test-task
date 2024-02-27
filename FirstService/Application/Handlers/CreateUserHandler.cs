namespace Application.Handlers;

public class CreateUserHandler(IBus Bus) : IRequestHandler<CreateUser>
{
    public async Task Handle(CreateUser request, CancellationToken token)
    {
        var endpoint = await Bus.GetSendEndpoint(new("queue:"+typeof(CreateUser).FullName));
        await endpoint.Send(request, token);
    }
}