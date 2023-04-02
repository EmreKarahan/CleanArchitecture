using Application.Common.Mappings;
using Minima.Trendyol.Client.Models.Service.Category.Response;

namespace Application.MarketPlaces.Trendyol.Queries.Attribute;

public class CategoryAttributeDto : IMapFrom<CategoryAttribute>
{
    public bool AllowCustom { get; set; }
    public AttributeDto Attribute { get; set; }
    public AttributeDto[] AttributeValues { get; set; }
    public int CategoryId { get; set; }
    public bool CategoryAttributeRequired { get; set; }
    public bool Varianter { get; set; }
    public bool Slicer { get; set; }
}