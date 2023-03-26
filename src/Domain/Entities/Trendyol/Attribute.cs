using Domain.Common;

namespace Domain.Entities.Trendyol;

public class Attribute : BaseEntity
{
    public long InternalId { get; set; }
    public string Name { get; set; }
    public bool Required { get; set; }
    public bool AllowCustom { get; set; }
    public bool Varianter { get; set; }
    public bool Slicer { get; set; }
    public int CategoryId { get; set; }

    public virtual Category Category { get; set; }
    public virtual List<AttributeValue> AttributeValues { get; set; }
    public string? DisplayName { get; set; }
}