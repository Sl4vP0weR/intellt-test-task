namespace Tests;

public class UserTests(IMediator Mediator, IDataBaseContext DataBase)
{
    [Theory]
    [InlineData("Никита", "Парамонов", "Сергеевич", "+71234578962", "greenorine@yandex.ru")]
    [InlineData("We-Wee", "Master", "Artemovich", "+71354795354", "dungeonmaster@mail.ru")]
    [InlineData("Bingisu", "Staine", null, "+75544444454", "bingisus@ab.tr")]
    public async Task UserCreation_ValidData(string name, string surname, string? patronymic, string phone, string mail)
    {
        CreateUser request = new(name, surname, patronymic, phone, mail);
        await Mediator.Send(request);
    }
    
    [Theory]
    [InlineData("никита", "парамонов", "сергеевич", "+71234578962", "greenorine@yandex.ru")] // lower case names
    [InlineData("We-wee", "Master", "Artemovich", "+71354795354", "dungeonmaster@mail.ru")] // lower case after hyphen
    [InlineData("Bingisu", "Staine", null, "+75544444454", "qwerty")] // invalid email
    [InlineData("Abraham", "Staine", null, null, "abra@ab.tr")] // invalid number
    public async Task UserCreation_InvalidData_Throws(string name, string surname, string? patronymic, string phone, string mail)
    {
        await Assert.ThrowsAsync<ValidationException>(() => 
            UserCreation_ValidData(name, surname, patronymic, phone, mail));
    }
    
    [Fact]
    public async Task UserAssignment()
    {
        await DataBase.TrySeedAsync();

        var user = await DataBase.Users.FirstAsync();
        var organization = await DataBase.Organizations.FirstAsync();
        
        var request = new AssignUser(user.Id, organization.Id);
        await Mediator.Send(request);

        user = await DataBase.Users.Include(x => x.Organization).FirstAsync();
        
        Assert.NotNull(user.Organization);
    }

    [Fact]
    public async Task UsersQuery_ByOrganization()
    {
        await UserAssignment();
        
        var organization = await DataBase.Organizations.FirstAsync();
        
        var request = new QueryOrganizationUsers(organization.Id, 0, int.MaxValue);
        var response = await Mediator.Send(request);
        
        Assert.Equal(0, response.Left);
        
        Assert.Collection(response.Users, _ => { }); // only one user assigned to the organization
    }
}
