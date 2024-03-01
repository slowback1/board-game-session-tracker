using API.Config;
using API.Utils;
using Database.Common.DTOs;
using Database.Common.Storers;
using Database.Common.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Controllers;

public abstract class BaseController : Controller
{
    public readonly JwtConfig JwtConfig;
    protected readonly IDataStorer Storer;

    public BaseController(IConfiguration config)
    {
        JwtConfig = config.GetSection("Jwt").Get<JwtConfig>()!;

        Storer = LoadFromConfigToGetDataStorer(config);
    }

    protected UserDTO? AuthenticatedUser { get; private set; }

    private IDataStorer LoadFromConfigToGetDataStorer(IConfiguration config)
    {
        var supabaseConfig = config.GetSection("Supabase").Get<SupabaseConfig>()!;
        var databaseConfig = config.GetSection("Database").Get<DatabaseConfig>()!;
        var dataStorerConfig = config.GetSection("DataStorer").Get<DataStorerConfig>()!;

        var factory = new DataStorerFactory(dataStorerConfig);
        factory.DatabaseConfig = databaseConfig;
        factory.SupabaseConfig = supabaseConfig;

        return factory.GetDataStorerFromConfig();
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        GetAuthenticatedUser();
        ValidateInputs(context);

        base.OnActionExecuting(context);
    }

    private void ValidateInputs(ActionExecutingContext context)
    {
        var values = context.ActionArguments.Values;

        var validationErrors = new List<string>();

        foreach (var value in values) validationErrors.AddRange(Validator.ObjectValidator.ValidateObject(value));

        if (validationErrors.Count > 0)
            context.Result = Ok(new ApiResponse<object> { Errors = validationErrors });
    }

    private void GetAuthenticatedUser()
    {
        var headers = HttpContext.Request.Headers;

        if (!headers.Authorization.Any())
            return;

        var authHeader = headers.Authorization.First();

        var token = authHeader.Split(' ')[1];

        var provider = new TokenProvider(JwtConfig.Key);

        AuthenticatedUser = provider.DecodeToken(token);
    }
}