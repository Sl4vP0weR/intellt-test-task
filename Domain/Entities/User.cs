namespace Domain.Entities;

public class User
{
    public Guid Id { get; set; }

    public Organization? Organization { get; set; }
    
    public string Name { get; set; }
    public string Surname { get; set; }
    public string? Patronymic { get; set; }
    public string PhoneNumber { get; set; } 
    public string Mail { get; set; }
}