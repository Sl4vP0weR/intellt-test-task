namespace Application.Handlers;

public class QueryOrganizationUsersHandler(
    IMapper Mapper,
    IDataBaseContext DataBase
) : IRequestHandler<QueryOrganizationUsers, UserQuery>
{
    public async Task<UserQuery> Handle(QueryOrganizationUsers request, CancellationToken token)
    {
        var organization = await DataBase.Organizations.FindAsync([request.OrganizationId], token);
        ArgumentNullException.ThrowIfNull(organization);
        
        var users = DataBase.Users.Include(x => x.Organization)
            .Where(x => x.Organization == organization);
        
        var count = await users.CountAsync(token);

        var from = request.From;
        var to = Math.Min(request.To, count - 1);
        var range = to - from + 1;
        
        var usersDto = users
            .Skip(from).Take(range)
            .ProjectTo<UserDTO>(Mapper.ConfigurationProvider);
        
        UserQuery query = new(usersDto, count - range - to);
        return query;
    }
}