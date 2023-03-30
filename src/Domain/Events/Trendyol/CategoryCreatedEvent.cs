using Domain.Common;
using Domain.Entities.Trendyol;


namespace Domain.Events.Trendyol;

public class CategoryCreatedEvent : BaseEvent
{
    public CategoryCreatedEvent(Category category)
    {
        Category = category;
    }

    public Category Category { get; }
}
