using Api.SlowbackBgSession.Config;
using Database.Common.DTOs;
using Database.Common.Storers;
using Database.Common.User;
using Database.Supabase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.SlowbackBgSession.Controllers;

public abstract class BaseController : Controller
{
    public readonly JwtConfig JwtConfig;
    protected readonly IDataStorer Storer;

    public BaseController(IConfiguration config)
    {
        var supabaseConfig = config.GetSection("Supabase").Get<SupabaseConfig>()!;
        JwtConfig = config.GetSection("Jwt").Get<JwtConfig>()!;

        Storer = new SupabaseStorer(supabaseConfig.Url, supabaseConfig.Key);
    }

    protected UserDTO? AuthenticatedUser { get; private set; }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        GetAuthenticatedUser();
        base.OnActionExecuting(context);
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