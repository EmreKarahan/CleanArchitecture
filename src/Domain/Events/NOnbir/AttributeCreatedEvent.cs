using Domain.Common;
using Attribute = Domain.Entities.NOnbir.Attribute;

namespace Domain.Events.NOnbir;

public class AttributeCreatedEvent : BaseEvent
{
    public AttributeCreatedEvent(Attribute attribute)
    {
        Attribute = attribute;
    }

    public Attribute Attribute { get; }
}
