using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultitenantDbPoc.Models;

namespace MultitenantDbPoc.Persistence.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.TenantId).IsUnique(false);

        builder.Property(x => x.Name).IsRequired();
        builder.Property(x => x.Description).IsRequired(false);
    }
}