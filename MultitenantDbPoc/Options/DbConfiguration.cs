namespace MultitenantDbPoc.Options;

public class DbConfiguration
{
    public string DefaultConnectionString { get; set; }
    public Dictionary<string, string> TenantConnectionStrings { get; set; } = new();
}