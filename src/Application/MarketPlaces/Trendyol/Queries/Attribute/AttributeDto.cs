using Application.Common.Mappings;
namespace Application.MarketPlaces.Trendyol.Queries.Attribute;

public class AttributeDto : IMapFrom<Minima.Trendyol.Client.Models.Service.Category.Response.Attribute>
{
    public int Id { get; set; }
    
    public string Name { get; set; }
}