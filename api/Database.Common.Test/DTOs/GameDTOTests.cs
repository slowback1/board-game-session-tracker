using Database.Common.DTOs;
using TestUtilities;
using Validator.Attributes;

namespace Database.Common.Test.DTOs;

public class GameDTOTests
{
    [Test]
    public void CreateGameRequestHasRequiredAndMinLengthName()
    {
        typeof(CreateGameRequest)
            .HasProperty(nameof(CreateGameRequest.GameName))
            .PropertyHasAttribute<RequiredAttribute>()
            .PropertyHasAttribute<LengthAttribute>();
    }
}