namespace Domain.DTO;

public record UserQuery(
    IQueryable<UserDTO> Users,
    int Left
);