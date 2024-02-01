using Api.SlowbackBgSession.Config;
using Database.Common.Storers;
using Database.Supabase;
using Microsoft.AspNetCore.Mvc;

namespace Api.SlowbackBgSession.Controllers;

public abstract class BaseController : ControllerBase
{
    protected readonly IDataStorer Storer;

    public BaseController(IConfiguration config)
    {
        var supabaseConfig = config.GetSection("Supabase").Get<SupabaseConfig>()!;

        Storer = new SupabaseStorer(supabaseConfig.Url, supabaseConfig.Key);
    }
}