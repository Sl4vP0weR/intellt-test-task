namespace Domain.Requests;

public record QueryOrganizationUsers(
    Guid OrganizationId,
    int From,
    int To
) : IRequest<UserQuery>;