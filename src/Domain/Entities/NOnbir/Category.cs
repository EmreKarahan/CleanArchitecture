using Domain.Common;

namespace Domain.Entities.NOnbir;

public class Category : BaseEntity
{
    
    public long InternalId { get; set; }
    public long? InternalParentId { get; set; }
    public int? ParentId { get; set; }
    public string? Name { get; set; }
    
    public bool HasAttribute { get; set; }
    public bool IsDeepest   { get; set; }

    public bool HasError { get; set; }
    
    // public int Depth => 1 + ParentDepth;
    // public int ParentDepth => Parent?.Depth ?? 0;
    public virtual Category? Parent { get; set; }
    public virtual ICollection<Category>? SubCategories { get; set; }
    public virtual ICollection<Attribute>? Attributes { get; set; }
}