using Application.Common.Caching;
using AutoMapper;
using MediatR;
using Minima.Trendyol.Client.Models;
using Minima.Trendyol.Client.Models.Service.Category.Response;
using Minima.Trendyol.Client.Services;

namespace Application.MarketPlaces.Trendyol.Queries.Category;

public record GetCategoriesFromApiQuery : IRequest<List<CategoryDto>?>
{
    public int ListId { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetTodoItemsWithPaginationQueryHandler : IRequestHandler<GetCategoriesFromApiQuery, List<CategoryDto>?>
{
    private readonly IMapper _mapper;
    readonly ICategoryApiService _categoryApiService;
    readonly ICachingManager _cachingManager;

    public GetTodoItemsWithPaginationQueryHandler(
        IMapper mapper,
        ICategoryApiService categoryApiService,
        ICachingManager cachingManager)
    {
        _mapper = mapper;
        _categoryApiService = categoryApiService;
        _cachingManager = cachingManager;
    }

    public async Task<List<CategoryDto>?> Handle(GetCategoriesFromApiQuery request, CancellationToken cancellationToken)
    {
        var data = await _cachingManager.GetValueOrCreateAsync("categories", 10000,
            async () =>
            {
                var categoriesBaseResponse = await _categoryApiService.GetCategoryList(new TrendyolBaseRequest());
                if (!categoriesBaseResponse.IsSuccess)
                    return null;
                return _mapper.Map<List<TrendyolCategoryResponse>, List<CategoryDto>>(categoriesBaseResponse.Data);
            });
        return data;
    }
}