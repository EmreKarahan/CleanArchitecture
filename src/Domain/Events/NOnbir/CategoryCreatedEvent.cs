using Domain.Common;
using Domain.Entities.NOnbir;

namespace Domain.Events.NOnbir;

public class CategoryCreatedEvent : BaseEvent
{
    public CategoryCreatedEvent(Category category)
    {
        Category = category;
    }

    public Category Category { get; }
}
