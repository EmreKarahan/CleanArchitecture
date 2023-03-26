using Domain.Common;

namespace Domain.Entities.NOnbir;

public class AttributeValue : BaseEntity
{
    public int AttributeId { get; set; }
    // public long InternalId { get; set; }
    public string Name { get; set; }
    public string DependedName { get; set; }
    
    public virtual Attribute Attribute { get; set; }
}