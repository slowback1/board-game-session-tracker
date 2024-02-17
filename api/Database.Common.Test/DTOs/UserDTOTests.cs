using Database.Common.DTOs;
using TestUtilities;
using Validator.Attributes;

namespace Database.Common.Test.DTOs;

public class UserDTOTests
{
    [Test]
    public void CreateUserDTOHasMatchingPropertyValidation()
    {
        typeof(CreateUserDTO)
            .HasAttribute<MatchingPropertyAttribute>();
    }

    [Test]
    public void CreateUserDTOHasRequiredUsername()
    {
        typeof(CreateUserDTO)
            .HasProperty(nameof(CreateUserDTO.Username))
            .PropertyHasAttribute<RequiredAttribute>();
    }

    [Test]
    public void CreateUserDTOHasRequiredPassword()
    {
        typeof(CreateUserDTO)
            .HasProperty(nameof(CreateUserDTO.Password))
            .PropertyHasAttribute<RequiredAttribute>();
    }

    [Test]
    public void LoginDTOHasRequiredUsername()
    {
        typeof(LoginDTO)
            .HasProperty(nameof(LoginDTO.Username))
            .PropertyHasAttribute<RequiredAttribute>();
    }

    [Test]
    public void LoginDTOHasRequiredPassword()
    {
        typeof(LoginDTO)
            .HasProperty(nameof(LoginDTO.Password))
            .PropertyHasAttribute<RequiredAttribute>();
    }
}