using Domain.Common;
using Domain.Entities.Trendyol;

namespace Domain.Events.Trendyol;

public class AttributeValueCreatedEvent : BaseEvent
{
    public AttributeValueCreatedEvent(AttributeValue attributeValue)
    {
        AttributeValue = attributeValue;
    }

    public AttributeValue AttributeValue { get; }
}
