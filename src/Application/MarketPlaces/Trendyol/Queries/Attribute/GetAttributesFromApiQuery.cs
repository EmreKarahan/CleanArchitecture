using Application.Common.Caching;
using AutoMapper;
using MediatR;
using Minima.Trendyol.Client.Models.Service.Category.Response;
using Minima.Trendyol.Client.Services;

namespace Application.MarketPlaces.Trendyol.Queries.Attribute;

public class GetAttributesFromApiQuery : IRequest<AttributeResponseDto?>
{
    public GetAttributesFromApiQuery(int categoryId)
    {
        CategoryId = categoryId;
    }
    public int CategoryId { get; }
}

public class GetAttributesFromApiQueryHandler : IRequestHandler<GetAttributesFromApiQuery, AttributeResponseDto?>
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

    public async Task<AttributeResponseDto?> Handle(GetAttributesFromApiQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = $"trendyol_attributes_{request.CategoryId}";
        
        var data = await _cachingManager.GetValueOrCreateAsync(cacheKey, 10000,
            async () =>
            {
                var attributesResponse =
                    await _categoryApiService.GetCategoryAttributeByCategoryIdAsync(request.CategoryId);
                if (!attributesResponse.IsSuccess)
                    return null;
                return _mapper.Map<TrendyolCategoryAttributeResponse, AttributeResponseDto>(attributesResponse.Data);
            });
        return data;
    }
}