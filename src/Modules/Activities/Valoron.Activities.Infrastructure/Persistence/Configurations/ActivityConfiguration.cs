using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Valoron.Activities.Domain;

namespace Valoron.Activities.Infrastructure.Persistence.Configurations;

public class ActivityConfiguration : IEntityTypeConfiguration<Activity>
{
    public void Configure(EntityTypeBuilder<Activity> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.OwnsOne(a => a.Category, category =>
        {
            category.Property(c => c.Code).HasColumnName("Category_Code");
            category.Property(c => c.Name).HasColumnName("Category_Name");
        });

        builder.OwnsOne(a => a.Difficulty, difficulty =>
        {
            difficulty.Property(d => d.Value).HasColumnName("Difficulty_Value");
        });

        builder.OwnsOne(a => a.Measurement, measurement =>
        {
            measurement.Property(m => m.Unit).HasColumnName("Measurement_Unit");
            measurement.Property(m => m.TargetValue).HasColumnName("Measurement_Target");
            measurement.Property(m => m.CurrentValue).HasColumnName("Measurement_Current");
        });

        builder.Property(a => a.ResourceId).IsRequired(false);
    }
}
