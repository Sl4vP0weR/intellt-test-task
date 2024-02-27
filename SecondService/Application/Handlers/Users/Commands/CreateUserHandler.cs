namespace Application.Handlers;

public class CreateUserHandler(
    IDataBaseContext DataBase    
) : IRequestHandler<CreateUser>
{
    public async Task Handle(CreateUser request, CancellationToken token)
    {
        var user = await DataBase.Users.FirstOrDefaultAsync(x => 
            x.PhoneNumber == request.PhoneNumber || 
            x.Mail == request.Mail, token);
        
        if(user is not null) return;
        
        user = new()
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Surname = request.Surname,
            Patronymic = request.Patronymic,
            Mail = request.Mail,
            PhoneNumber = request.PhoneNumber.Trim('+')
        };

        await DataBase.Users.AddAsync(user, token);
        
        await DataBase.SaveChangesAsync(token);
    }
}