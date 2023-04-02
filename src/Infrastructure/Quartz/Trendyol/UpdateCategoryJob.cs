using Application.MarketPlaces.Trendyol.Commands;
using Domain.Entities.Trendyol;
using Microsoft.Extensions.Logging;
using Shared.Attributes;

namespace Infrastructure.Quartz.Trendyol;

[ScheduledJob("UpdateCategoryJob", "Trendyol", "UpdateCategoryJobTrigger", "Trendyol", "0 15 4 1/1 * ? *", true)]
public class UpdateCategoryJob //: IJob
{
    readonly ICategoryApiService _categoryApiService;
    readonly IMediator _mediator;
    ILogger<UpdateCategoryJob> _logger;

    public UpdateCategoryJob(
        ICategoryApiService categoryApiService,
        IMediator mediator,
        ILogger<UpdateCategoryJob> logger)
    {
        _categoryApiService = categoryApiService;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        await GetDeepestCategories();
    }

    private async Task<List<TrendyolCategoryResponse>> GetDeepestCategories()
    {
        var result = await _categoryApiService.GetCategoryList();

        if (!result.IsSuccess)
            await Task.FromCanceled(new CancellationToken());


        foreach (var item in result.Data)
        {
            var createCategoryCommand = new CreateCategoryCommand() {Name = item.Name, InternalId = item.Id};
            var categoryId = await _mediator.Send(createCategoryCommand);

            var category = new Category {Id = categoryId, Name = item.Name, InternalId = item.Id};
            await RecursiveQuestionCount(item, category);
        }

        return result.Data;
    }

    private async Task RecursiveQuestionCount(TrendyolCategoryResponse cat, Category category)
    {
        if (cat.SubCategories == null || cat.SubCategories.Count == 0)
        {
            var updateCategoryCommand = new UpdateCategoryCommand
            {
                InternalId = cat.Id, 
                IsDeepest = true
            };
            await _mediator.Send(updateCategoryCommand);
        }

        foreach (var child in cat.SubCategories)
        {
            var subCategory = new Category
            {
                ParentId = category.Id,
                Name = child.Name,
                InternalId = child.Id,
                InternalParentId = child.ParentId,
                SubCategories = new List<Category>()
            };
            //category.SubCategories.Add(subCategrory);

            var createCategoryCommand = new CreateCategoryCommand()
            {
                Name = child.Name,
                InternalId = child.Id,
                ParentId = category.Id == 0 ? null : category.Id,
                InternalParentId = child.ParentId,
            };
            var categoryId = await _mediator.Send(createCategoryCommand);
            subCategory.Id = categoryId;

            await RecursiveQuestionCount(child, subCategory);
        }
    }
}