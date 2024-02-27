namespace Domain.Requests;

public record CreateUser(
    string Name, 
    string Surname, 
    string? Patronymic, 
    string PhoneNumber, 
    string Mail
) : IRequest;