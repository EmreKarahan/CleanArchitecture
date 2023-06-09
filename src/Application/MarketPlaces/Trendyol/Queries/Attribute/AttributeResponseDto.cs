using Application.Common.Mappings;
using Minima.Trendyol.Client.Models.Service.Category.Response;


namespace Application.MarketPlaces.Trendyol.Queries.Attribute;

public class AttributeResponseDto: IMapFrom<TrendyolCategoryAttributeResponse>
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public string? DisplayName { get; set; }
    public List<CategoryAttributeDto>? CategoryAttributes { get; set; }
}