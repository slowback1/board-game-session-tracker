using Api.SlowbackBgSession.Controllers;
using Database.Common.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.SlowbackBgSession.Attributes;

public class AuthenticatedAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var headers = context.HttpContext.Request.Headers;

        if (headers.Authorization.Count == 0)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var token = headers.Authorization.First().Split(' ')[1];


        var user = new TokenProvider(GetSigningKey(context)).DecodeToken(token);

        if (user is null) context.Result = new UnauthorizedResult();
    }

    private string GetSigningKey(ActionExecutingContext context)
    {
        var controller = (BaseController)context.Controller;

        return controller.JwtConfig.Key;
    }
}