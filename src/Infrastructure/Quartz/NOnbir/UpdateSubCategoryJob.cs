using Domain.Entities.NOnbir;
using Minima.MarketPlace.NOnbir.Models.ReturnType.Category.Requests;
using Minima.MarketPlace.NOnbir.Models.Service.Category.Types;
using Shared.Attributes;
using ICategoryApiService = Minima.MarketPlace.NOnbir.Services.ICategoryApiService;

namespace Infrastructure.Quartz.NOnbir;

[ScheduledJob("UpdateSubCategoryJob", "N11", "UpdateSubCategoryJobTigger", "N11", "0 0/10 * 1/1 * ? *")]
public class UpdateSubCategoryJob : IJob, IDisposable
{
    readonly ICategoryApiService _categoryApiService;
    readonly IRepository<Category> _categoryRepository;

    // boolean variable to ensure dispose
    // method executes only once
    private bool disposedValue;
    public UpdateSubCategoryJob(
        ICategoryApiService categoryApiService,
        IRepository<Category> categoryRepository)
    {
        _categoryApiService = categoryApiService;
        _categoryRepository = categoryRepository;
    }
    
    public async Task Execute(IJobExecutionContext context)
    {
        string instName = context.JobDetail.Key.Name;
        JobDataMap dataMap = context.JobDetail.JobDataMap;

        long parentCategoryId = dataMap.GetLongValue("parentCategoryId");
        Console.WriteLine("Instance {0} of DumbJob says: {1}", instName, parentCategoryId);
        GetSubCategories(parentCategoryId);
    }

    private void GetSubCategories(long parentCategoryId)
    {
        var topLevelCategory = _categoryRepository.GetBy(p => p.InternalId == parentCategoryId);
        if (topLevelCategory == null)
            return;
        
        GetSubCategoryList(topLevelCategory.InternalId, topLevelCategory);
    }


    private void GetSubCategoryList(long parentCategoryId, Category category)
    {
        try
        {
            Category subCategory2 = null;
            Console.WriteLine(category.Name);
            category.SubCategories = new List<Category>();
            // if (!category.HasError)
            // {
            //     Console.WriteLine($"{category.Name} es geçildi.");
            // }
            
            var subCategories = _categoryApiService.GetSubCategories(new GetSubCategoriesRequestReturn
            {
                CategoryId = parentCategoryId
            });
            Thread.Sleep(1000);

            if (subCategories != null && subCategories.Result.Status == "success")
            {
                if (subCategories?.Category?.FirstOrDefault()?.SubCategoryList is null)
                {
                    var categoryTemp = _categoryRepository.GetBy(i => i.InternalId == category.InternalId);
                    categoryTemp.IsDeepest = true;
                    categoryTemp.HasError = false;
                    _categoryRepository.Update(categoryTemp);
                }
                else
                {
                    var subCategoryList = subCategories?.Category?.FirstOrDefault()?.SubCategoryList.ToList();

                    if (subCategoryList != null && subCategoryList.Count > 0)
                    {
                        try
                        {
                            foreach (SubCategory subCategory in subCategoryList)
                            {
                                subCategory2 = new Category
                                {
                                    Name = subCategory.Name,
                                    InternalId = subCategory.Id,
                                    InternalParentId = category.InternalId,
                                };
                                //
                                // var subCategoryParent =
                                //     _categoryRepository.GetBy(p => p.InternalParentId == category.InternalId);

                                // if (subCategoryParent is not null)
                                //     subCategory2.ParentId = subCategoryParent.Id;

                                if (category is not null)
                                {
                                    if (category.Id == 0)
                                    {
                                        category = _categoryRepository.GetBy(g => g.InternalId == category.InternalId);
                                        //_categoryRepository.Insert(category);
                                    }

                                    subCategory2.ParentId = category.Id;
                                }

                                var categoryCheck =
                                    _categoryRepository.GetBy(p => p.InternalId == subCategory2.InternalId);
                                if (categoryCheck is null)
                                    _categoryRepository.Insert(subCategory2);
                                else
                                {
                                    var subCategory2Temp = _categoryRepository.GetBy(t => t.InternalId == subCategory2.InternalId);
                                    subCategory2Temp.ParentId = category.Id;
                                    _categoryRepository.Update(subCategory2Temp);
                                }
                                 
                                
 
                                GetSubCategoryList(subCategory.Id, subCategory2);
                            }
                        }
                        catch (Exception e)
                        {
                            
                            Console.WriteLine(e.Message);
                        }
                    }
                    else
                    {
                        subCategory2.IsDeepest = true;
                        subCategory2.HasError = false;
                        _categoryRepository.Update(subCategory2);
                        Console.WriteLine("Alt kategori yok");
                    }
                }
            }
            else
            {
                var categoryError = _categoryRepository.GetBy(p => p.InternalId == parentCategoryId);
                if (categoryError != null)
                {
                    categoryError.HasError = true;
                    _categoryRepository.Update(categoryError);
                }
                Console.WriteLine(subCategories.Result.ErrorMessage);
                Console.WriteLine("siktiğim servisi");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }


    // Gets called by the below dispose method
    // resource cleaning happens here
    protected virtual void Dispose(bool disposing)
    {
        // check if already disposed
        if (!disposedValue) {
            if (disposing) {
                // free managed objects here
                
            }
            // free unmanaged objects here
            Console.WriteLine("The {0} has been disposed", this.GetType().Name);

            // set the bool value to true
            disposedValue = true;
        }
    }
    public void Dispose()
    {
        // Invoke the above virtual
        // dispose(bool disposing) method
        Dispose(disposing : true);
  
        // Notify the garbage collector
        // about the cleaning event
        GC.SuppressFinalize(this);
    }
    ~UpdateSubCategoryJob() { Dispose(disposing : false); }
}