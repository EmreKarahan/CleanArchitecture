using Domain.Entities.Trendyol;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Attribute = Domain.Entities.Trendyol.Attribute;

namespace Infrastructure.Persistence.Configurations.Trendyol;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Category", "Trendyol");
        builder.HasKey(x => x.Id);
        builder.Property(t => t.Name)
            .HasMaxLength(400)
            .IsRequired();

        builder.HasOne(x => x.Parent)
            .WithMany(x => x.SubCategories)
            .HasForeignKey(x => x.ParentId)
            .IsRequired(false);
        //.OnDelete(DeleteBehavior.Restrict);

        builder.Navigation(x => x.Parent)
            .IsRequired(false);
    }
}

public class AttributeConfiguration : IEntityTypeConfiguration<Attribute>
{
    public void Configure(EntityTypeBuilder<Attribute> builder)
    {
        builder.ToTable("Attribute", "Trendyol");
        builder.HasKey(x => x.Id);
        builder.Property(t => t.Name)
            .HasMaxLength(400)
            .IsRequired();

        builder.Property(t => t.DisplayName)
            .HasMaxLength(400)
            .IsRequired(false);


        // builder.HasOne(x => x.Category)
        //     .WsForeignKey(x => x.CategoryId)
        //                   .IsRequired(false);ithMany(x => x.Attributes)
        //     .HasForeignKey(x => x.CategoryId)
        //     .IsRequired(false);
        //.OnDelete(DeleteBehavior.Restrict);
    }
}

public class AttributeValueConfiguration : IEntityTypeConfiguration<AttributeValue>
{
    public void Configure(EntityTypeBuilder<AttributeValue> builder)
    {
        builder.ToTable("AttributeValue", "Trendyol");
        builder.HasKey(x => x.Id);
        builder.Property(t => t.Name)
            .HasMaxLength(400)
            .IsRequired();

        builder.HasOne(x => x.CategoryToAttribute)
            .WithMany(x => x.AttributeValues)
            .HasForeignKey(x => x.CategoryAttributeId)
            .IsRequired(false);
        

        // builder.HasOne(x => x.Attribute)
        //     .WithMany(x => x.AttributeValues)
        //     .HasForeignKey(x => x.AttributeId)
        //     .IsRequired(false);
        //.OnDelete(DeleteBehavior.Restrict);
    }
}

public class CategoryToAttributeConfiguration : IEntityTypeConfiguration<CategoryToAttribute>
{
    public void Configure(EntityTypeBuilder<CategoryToAttribute> builder)
    {
        builder.ToTable("Category_To_Attribute", "Trendyol");
        builder.HasKey(x => x.Id);


        builder.HasOne(bc => bc.Attribute)
            .WithMany(b => b.CategoryToAttributes)
            .HasForeignKey(bc => bc.AttributeId);  
        builder.HasOne(bc => bc.Category)
            .WithMany(c => c.CategoryToAttributes)
            .HasForeignKey(bc => bc.CategoryId);
        
        //.OnDelete(DeleteBehavior.Restrict);
    }
}