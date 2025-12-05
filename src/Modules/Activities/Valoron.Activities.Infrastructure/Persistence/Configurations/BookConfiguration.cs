using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Valoron.Activities.Domain;

namespace Valoron.Activities.Infrastructure.Persistence.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(b => b.Author)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(b => b.Status)
            .HasConversion<string>();

        builder.Property(b => b.UserId).IsRequired();
        builder.OwnsMany(b => b.ReadingSessions, a =>
        {
            a.ToTable("ReadingSessions");
            a.WithOwner().HasForeignKey("BookId");
            a.HasKey("Id"); // EF Core shadow ID
            a.Property(r => r.Duration);
            a.Property(r => r.Date);
            a.Property(r => r.PagesRead);
        });
    }
}
