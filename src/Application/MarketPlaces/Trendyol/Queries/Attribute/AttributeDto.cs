using Application.Common.Mappings;
using Minima.Trendyol.Client.Models.Service.Category.Response;

namespace Application.MarketPlaces.Trendyol.Queries.Attribute;

public class AttributeDto: IMapFrom<TrendyolCategoryAttributeResponse>
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public string? DisplayName { get; set; }
    public List<AttributeDto>? CategoryAttributes { get; set; }
}