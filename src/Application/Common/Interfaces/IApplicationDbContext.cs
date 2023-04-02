using Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<TodoList> TodoLists { get; }

    DbSet<TodoItem> TodoItems { get; }
    DbSet<Domain.Entities.NOnbir.Category> N11Category { get; }
    DbSet<Domain.Entities.NOnbir.Attribute> N11Attribute { get; }
    DbSet<Domain.Entities.NOnbir.AttributeValue> N11AttributeValue { get; }
    DbSet<Domain.Entities.Trendyol.Category> TrendyolCategory { get; }
    DbSet<Domain.Entities.Trendyol.Attribute> TrendyolAttribute { get; }
    DbSet<Domain.Entities.Trendyol.AttributeValue> TrendyolAttributeValue { get; }
    DbSet<Domain.Entities.Trendyol.CategoryToAttribute> TrendyolCategoryToAttribute { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}

