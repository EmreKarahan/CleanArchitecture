using Application.Common.Mappings;
using Minima.Trendyol.Client.Models.Service.Category.Response;

namespace Application.MarketPlaces.Trendyol.Queries.Category;

public record CategoryDto : IMapFrom<TrendyolCategoryResponse>
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int? ParentId { get; set; }

    public List<CategoryDto> SubCategories { get; set; }
}