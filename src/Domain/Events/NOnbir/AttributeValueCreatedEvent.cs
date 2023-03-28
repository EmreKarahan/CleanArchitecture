using Domain.Common;
using Domain.Entities.NOnbir;
using Attribute = Domain.Entities.NOnbir.Attribute;

namespace Domain.Events.NOnbir;

public class AttributeValueCreatedEvent : BaseEvent
{
    public AttributeValueCreatedEvent(AttributeValue attributeValue)
    {
        AttributeValue = attributeValue;
    }

    public AttributeValue AttributeValue { get; }
}
