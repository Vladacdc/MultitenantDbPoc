namespace MultitenantDbPoc.Services;

public class TenantService : ITenantService
{
    private readonly IHttpContextAccessor _contextAccessor;

    public TenantService(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public string GetTenantId()
    {
        // if (_contextAccessor.HttpContext?.Request.Headers.TryGetValue("Tenant", out var tenantId) ?? false)
        // {
        //     return tenantId.FirstOrDefault().ToString();
        // }

        return "OtherTenant";
    }
}