namespace Domain.Validators;

public class UserCreationRequest_Validator : AbstractValidator<CreateUser>
{
    private readonly Regex
        NameRegex = new (@"^[А-ЯA-Z][а-яa-z]{0,49}(?:\-[А-ЯA-Z][а-яa-z]{0,49})?$"), // RU or EN
        PhoneRegex= new (@"^\+\d{4,15}$"); // E.164
    
    public UserCreationRequest_Validator() 
    {
        RuleFor(x => x.Name).NotEmpty().Matches(NameRegex);
        RuleFor(x => x.Surname).NotEmpty().Matches(NameRegex);
        RuleFor(x => x.Patronymic)
            .Must(name => string.IsNullOrEmpty(name) || NameRegex.IsMatch(name))
            .WithMessage("Patronymic must be empty or match the name regex.");
        RuleFor(x => x.PhoneNumber).NotEmpty().Matches(PhoneRegex);
        RuleFor(x => x.Mail).NotEmpty().EmailAddress();
    }
}
