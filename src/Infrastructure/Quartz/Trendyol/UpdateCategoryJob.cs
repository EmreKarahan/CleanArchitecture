using Domain.Entities.Trendyol;
using Shared.Attributes;

namespace Infrastructure.Quartz.Trendyol;

[ScheduledJob("UpdateCategoryJob", "Trendyol", "UpdateCategoryJobTrigger", "Trendyol", "0 /1 * ? * *")]
public class UpdateCategoryJob : IJob
{
    readonly ICategoryApiService _categoryApiService;
    readonly IRepository<Category> _categoryRepository;

    public UpdateCategoryJob(
        ICategoryApiService categoryApiService, 
        IRepository<Category> categoryRepository)
    {
        _categoryApiService = categoryApiService;
        _categoryRepository = categoryRepository;
    }
    
    public async Task Execute(IJobExecutionContext context)
    {
        await GetDeepestCategories();
    }

    public async Task<List<TrendyolCategoryResponse>> GetDeepestCategories()
    {
        var result = await _categoryApiService.GetCategoryList();

        if (result.IsSuccess)
        {
            foreach (var item in result.Data)
            {
                var category = new Category
                {
                    Name = item.Name, InternalId = item.Id, SubCategories = new List<Category>()
                };
                RecursiveQuestionCount(item, category);
                try
                {
                    var exist = await _categoryRepository.GetByAsync(f => f.InternalId == item.Id);
                    if (exist != null)
                        continue;
                    
                    await _categoryRepository.InsertAsync(category);
                    
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

            }
            return result.Data;
        }
        
        return new List<TrendyolCategoryResponse>();
    }

    void RecursiveQuestionCount(TrendyolCategoryResponse cat, Category category)
    {
        //count += Questions.Count(); // this cat's questions.
        foreach (var child in cat.SubCategories)
        {
            var subCategrory = new Category
            {
                Name = child.Name,
                InternalId = child.Id,
                InternalParentId = child.ParentId,
                SubCategories = new List<Category>()
            };
            category.SubCategories.Add(subCategrory);
            RecursiveQuestionCount(child, subCategrory); // will add each child and so it goes...
        }
    }


}