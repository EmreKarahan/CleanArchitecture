using Application.Common.Caching;
using AutoMapper;
using MediatR;
using Minima.Trendyol.Client.Models.Service.Category.Response;
using Minima.Trendyol.Client.Services;

namespace Application.MarketPlaces.Trendyol.Queries.Attribute;

public class GetAttributesFromApiQuery : IRequest<AttributeDto?>
{
    public GetAttributesFromApiQuery(int categoryId)
    {
        CategoryId = categoryId;
    }
    public int CategoryId { get; }
}

public class GetAttributesFromApiQueryHandler : IRequestHandler<GetAttributesFromApiQuery, AttributeDto?>
{
    private readonly IMapper _mapper;
    readonly ICategoryApiService _categoryApiService;
    readonly ICachingManager _cachingManager;

    public GetAttributesFromApiQueryHandler(
        IMapper mapper,
        ICategoryApiService categoryApiService,
        ICachingManager cachingManager)
    {
        _mapper = mapper;
        _categoryApiService = categoryApiService;
        _cachingManager = cachingManager;
    }

    public async Task<AttributeDto?> Handle(GetAttributesFromApiQuery request, CancellationToken cancellationToken)
    {
        var attributesResponse =
            await _categoryApiService.GetCategoryAttributeByCategoryIdAsync(request.CategoryId);
        
        var data = await _cachingManager.GetValueOrCreateAsync("attributes", 10000,
            async () =>
            {
                var attributesResponse =
                    await _categoryApiService.GetCategoryAttributeByCategoryIdAsync(request.CategoryId);
                if (!attributesResponse.IsSuccess)
                    return null;
                return _mapper.Map<TrendyolCategoryAttributeResponse, AttributeDto>(attributesResponse.Data);
            });
        return data;
    }
}