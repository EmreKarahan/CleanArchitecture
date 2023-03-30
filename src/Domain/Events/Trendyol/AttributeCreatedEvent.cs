using Domain.Common;
using Attribute = Domain.Entities.Trendyol.Attribute;

namespace Domain.Events.Trendyol;

public class AttributeCreatedEvent : BaseEvent
{
    public AttributeCreatedEvent(Attribute attribute)
    {
        Attribute = attribute;
    }

    public Attribute Attribute { get; }
}
