namespace MultitenantDbPoc.Options;

public class TenantDbConfiguration
{
    public string Tenant { get; set; }

    public string ConnectionString { get; set; }
}