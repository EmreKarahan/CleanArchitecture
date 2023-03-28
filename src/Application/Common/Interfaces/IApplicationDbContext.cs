using Domain.Entities;
using Domain.Entities.NOnbir;
using Microsoft.EntityFrameworkCore;
using Attribute = Domain.Entities.NOnbir.Attribute;

namespace Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<TodoList> TodoLists { get; }

    DbSet<TodoItem> TodoItems { get; }
    DbSet<Category> N11Category { get; }
    DbSet<Attribute> N11Attribute { get; }
    DbSet<AttributeValue> N11AttributeValue { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}

