namespace MultitenantDbPoc.Models;

public class Book
{
    public Guid Id { get; set; }

    public string TenantId { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }
}