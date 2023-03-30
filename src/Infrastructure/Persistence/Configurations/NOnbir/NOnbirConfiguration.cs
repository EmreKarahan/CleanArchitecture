using Domain.Entities.NOnbir;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Attribute = Domain.Entities.NOnbir.Attribute;

namespace Infrastructure.Persistence.Configurations.NOnbir;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Category", "NOnbir");
        builder.HasKey(x => x.Id);
        builder.Property(t => t.Name)
            .HasMaxLength(400)
            .IsRequired();

        builder.HasOne(x => x.Parent)
            .WithMany(x => x.SubCategories)
            .HasForeignKey(x => x.ParentId)
            .IsRequired(false);
        //.OnDelete(DeleteBehavior.Restrict);
    }
}

public class AttributeConfiguration : IEntityTypeConfiguration<Attribute>
{
    public void Configure(EntityTypeBuilder<Attribute> builder)
    {
        builder.ToTable("Attribute", "NOnbir");
        builder.HasKey(x => x.Id);
        builder.Property(t => t.Name)
            .HasMaxLength(400)
            .IsRequired();

        builder.Property(t => t.Name)
            .HasMaxLength(400)
            .IsRequired(false);

        builder.HasOne(x => x.Category)
            .WithMany(x => x.Attributes)
            .HasForeignKey(x => x.CategoryId)
            .IsRequired(false);
        //.OnDelete(DeleteBehavior.Restrict);
    }
}

public class AttributeValueConfiguration : IEntityTypeConfiguration<AttributeValue>
{
    public void Configure(EntityTypeBuilder<AttributeValue> builder)
    {
        builder.ToTable("AttributeValue", "NOnbir");
        builder.HasKey(x => x.Id);
        builder.Property(t => t.Name)
            .HasMaxLength(400)
            .IsRequired();

        builder.Property(t => t.DependedName)
            .HasMaxLength(400)
            .IsRequired(false);

        // builder.Property(t => t.InternalId)
        //     .IsRequired(false);

        builder.HasOne(x => x.Attribute)
            .WithMany(x => x.AttributeValues)
            .HasForeignKey(x => x.AttributeId)
            .IsRequired(false);
        //.OnDelete(DeleteBehavior.Restrict);
    }
}