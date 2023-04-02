using Domain.Common;

namespace Domain.Entities.Trendyol;

public class CategoryToAttribute: BaseEntity
{
    public int CategoryId { get; set; }
    public int AttributeId { get; set; }
    
    public virtual Category Category { get; set; } = null!;
    public virtual Attribute Attribute { get; set; } = null!;
    public virtual ICollection<AttributeValue> AttributeValues { get; set; }
}