namespace Application.Handlers;

public class AssignUserHandler(
    IDataBaseContext DataBase    
) : IRequestHandler<AssignUser>
{
    public async Task Handle(AssignUser request, CancellationToken token)
    {
        var user = await DataBase.Users.FindAsync([request.UserId], token);
        ArgumentNullException.ThrowIfNull(user);
        
        var organization = await DataBase.Organizations.FindAsync([request.OrganizationId], token);
        ArgumentNullException.ThrowIfNull(organization);
        
        user.Organization = organization;
        await DataBase.SaveChangesAsync(token);
    }
}