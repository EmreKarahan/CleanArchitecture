using Domain.Entities.NOnbir;
using Shared.Attributes;
using ICategoryApiService = Minima.MarketPlace.NOnbir.Services.ICategoryApiService;

namespace Infrastructure.Quartz.NOnbir;

[ScheduledJob("UpdateTopCategoryJob", "N11", "UpdateTopCategoryJobTrigger", "N11", "0 /1 * ? * *")]
public class UpdateTopCategoryJob : IJob
{
    readonly ICategoryApiService _categoryApiService;
    readonly IRepository<Category> _categoryRepository;

    public UpdateTopCategoryJob(
        ICategoryApiService categoryApiService, 
        IRepository<Category> categoryRepository)
    {
        _categoryApiService = categoryApiService;
        _categoryRepository = categoryRepository;
    }
    
    public  async Task Execute(IJobExecutionContext context)
    {
        UpdateTopCategories().GetAwaiter().GetResult();
    }

    private async Task UpdateTopCategories()
    {
        var response = _categoryApiService.GetTopLevelCategories();
        if (response.Result.Status != "success")
            return;
        
        foreach (var topLevelCategory in response.CategoryList)
        {
            if (topLevelCategory is null)
                continue;
            
            Category serviceCategory = await _categoryRepository.GetByAsync(x => x.InternalId == topLevelCategory.Id);
            if (serviceCategory is null)
            {
                serviceCategory = new Category
                {
                    Name = topLevelCategory.Name,
                    InternalId = topLevelCategory.Id
                };
                await _categoryRepository.InsertAsync(serviceCategory);
            }
            else
            {
                serviceCategory.Name = topLevelCategory.Name;
                _categoryRepository.Update(serviceCategory);
            }
        }
    }


}