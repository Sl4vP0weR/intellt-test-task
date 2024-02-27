namespace Domain.Requests;

public record AssignUser(
    Guid UserId,
    Guid OrganizationId
) : IRequest;