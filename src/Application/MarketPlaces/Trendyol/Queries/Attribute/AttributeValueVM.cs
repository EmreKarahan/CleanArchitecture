namespace Application.MarketPlaces.Trendyol.Queries.Attribute;

public class AttributeValueVm
{
    public IReadOnlyCollection<AttributeValueItemDto> Lists { get; init; } = Array.Empty<AttributeValueItemDto>();
}