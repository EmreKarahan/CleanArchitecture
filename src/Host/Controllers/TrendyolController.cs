using Application.MarketPlaces.Trendyol.Queries.Attribute;
using Application.MarketPlaces.Trendyol.Queries.Category;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers;

public class TrendyolController : ApiControllerBase
{
    [HttpGet("categories")]
    public async Task<List<CategoryDto>?> Get()
    {
        return await Mediator.Send(new GetCategoriesFromApiQuery());
    }
    
    [HttpGet("attributes")]
    public async Task<AttributeResponseDto?> Get(int categoryId)
    {
        return await Mediator.Send(new GetAttributesFromApiQuery(categoryId));
    }

}