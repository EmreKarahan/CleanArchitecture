using Domain.Common;

namespace Domain.Entities.Trendyol;

public class AttributeValue : BaseEntity
{
    public int InternalId { get; set; }
    public string Name { get; set; }
    //public int AttributeId { get; set; }
    public int CategoryAttributeId { get; set; }
    

    public virtual CategoryToAttribute CategoryToAttribute { get; set; }
}