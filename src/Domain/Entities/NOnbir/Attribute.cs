using Domain.Common;

namespace Domain.Entities.NOnbir;

public class Attribute : BaseEntity
{
    public long InternalId { get; set; }
    public bool Mandatory { get; set; }
    public bool MultipleSelect { get; set; }
    public string Name { get; set; }
    public double Priority { get; set; }

    public int CategoryId { get; set; }
    public virtual Category Category { get; set; }
    public virtual ICollection<AttributeValue> AttributeValues { get; set; }
}