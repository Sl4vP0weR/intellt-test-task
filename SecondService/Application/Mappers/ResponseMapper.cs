namespace Application.Mappers;

public class ResponseMapper : AutoMapper.Profile
{
    public ResponseMapper()
    {
        CreateMap<User, UserDTO>().ReverseMap();
    }
}