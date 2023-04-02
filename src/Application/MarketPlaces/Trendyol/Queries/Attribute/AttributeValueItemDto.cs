using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities.Trendyol;

namespace Application.MarketPlaces.Trendyol.Queries.Attribute;

public class AttributeValueItemDto : IMapFrom<AttributeValue>
{
    public int InternalId { get; set; }
    public string Name { get; set; }
    public int AttributeId { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<AttributeValue, AttributeValueItemDto>();
    }
}