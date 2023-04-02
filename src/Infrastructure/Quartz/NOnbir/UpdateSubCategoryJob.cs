using Application.MarketPlaces.N11.Commands;
using Domain.Entities.NOnbir;
using Microsoft.Extensions.Logging;
using Minima.MarketPlace.NOnbir.Models.ReturnType.Category.Requests;
using Minima.MarketPlace.NOnbir.Models.Service.Category.Types;
using Shared.Attributes;
using ICategoryApiService = Minima.MarketPlace.NOnbir.Services.ICategoryApiService;


namespace Infrastructure.Quartz.NOnbir;

[ScheduledJob("UpdateSubCategoryJob", "N11", "UpdateSubCategoryJobTigger", "N11", "0 0/10 * 1/1 * ? *")]
public class UpdateSubCategoryJob //: IJob, IDisposable
{
    readonly ICategoryApiService _categoryApiService;
    readonly IRepository<Category> _categoryRepository;
    readonly IMediator _mediator;
    ILogger<UpdateSubCategoryJob> _logger;

    // boolean variable to ensure dispose
    // method executes only once
    private bool disposedValue;

    public UpdateSubCategoryJob(
        ICategoryApiService categoryApiService,
        IRepository<Category> categoryRepository, 
        IMediator mediator, 
        ILogger<UpdateSubCategoryJob> logger)
    {
        _categoryApiService = categoryApiService;
        _categoryRepository = categoryRepository;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        string instName = context.JobDetail.Key.Name;
        JobDataMap dataMap = context.JobDetail.JobDataMap;

        long parentCategoryId = dataMap.GetLongValue("parentCategoryId");
        _logger.LogInformation("Instance {0} of DumbJob says: {1}", instName, parentCategoryId);
        await GetSubCategories(parentCategoryId);
    }

    private async Task GetSubCategories(long parentCategoryId)
    {
        var topLevelCategory = _categoryRepository.GetBy(p => p.InternalId == parentCategoryId);
        await GetSubCategoryList(topLevelCategory.InternalId, topLevelCategory);
    }


    private async Task GetSubCategoryList(long parentCategoryId, Category category)
    {
        try
        {
            Category subCategory2 = null;
            _logger.LogInformation(category.Name);
            var subCategories = _categoryApiService.GetSubCategories(new GetSubCategoriesRequestReturn
            {
                CategoryId = parentCategoryId
            });
            Thread.Sleep(500);

            if (subCategories != null && subCategories.Result.Status == "success")
            {
                if (subCategories?.Category?.FirstOrDefault()?.SubCategoryList is null)
                {
                    var categoryTemp = _categoryRepository.GetBy(i => i.InternalId == category.InternalId);

                    var updateCategoryCommand = new UpdateCategoryCommand
                    {
                        Id = categoryTemp.Id, 
                        IsDeepest = true, 
                        HasError = false,
                    };
                    await _mediator.Send(updateCategoryCommand);
                }
                else
                {
                    var subCategoryList = subCategories?.Category?.FirstOrDefault()?.SubCategoryList.ToList();

                    if (subCategoryList is {Count: > 0})
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


                                if (category is not null)
                                {
                                    if (category.Id == 0)
                                    {
                                        category = _categoryRepository.GetBy(g => g.InternalId == category.InternalId);
                                    }

                                    subCategory2.ParentId = category.Id;
                                }

                                var categoryCheck =
                                    _categoryRepository.GetBy(p => p.InternalId == subCategory2.InternalId);
                                if (categoryCheck is null)
                                {
                                    var categoryCreateCommand = new CreateCategoryCommand
                                    {
                                        Name = subCategory2.Name,
                                        InternalId = subCategory2.Id,
                                        InternalParentId = subCategory2.InternalId,
                                        ParentId = subCategory2.Id
                                    };
                                    
                                    await _mediator.Send(categoryCreateCommand);
                                }

                                else
                                {
                                    var subCategory2Temp =
                                        _categoryRepository.GetBy(t => t.InternalId == subCategory2.InternalId);
                                    var updateCategoryCommand = new UpdateCategoryCommand
                                    {
                                        Id = subCategory2Temp.Id,
                                        ParentId = category.Id
                                    };
                                    await _mediator.Send(updateCategoryCommand);
                                    
                                }

                                await GetSubCategoryList(subCategory.Id, subCategory2);
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                    else
                    {
                        var updateCategoryCommand = new UpdateCategoryCommand
                        {
                            Id = subCategory2.Id,
                            IsDeepest =true,
                            HasError = true
                        };
                        await _mediator.Send(updateCategoryCommand);
                        Console.WriteLine("Alt kategori yok");
                    }
                }
            }
            else
            {
                var categoryError = _categoryRepository.GetBy(p => p.InternalId == parentCategoryId);
                if (categoryError != null)
                {
                    var updateCategoryCommand = new UpdateCategoryCommand
                    {
                        Id = categoryError.Id,
                        HasError = true
                    };
                    await _mediator.Send(updateCategoryCommand);
                }

                _logger.LogError(subCategories.Result.ErrorMessage);
                _logger.LogError("siktiğim servisi");
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
        }
    }


    // Gets called by the below dispose method
    // resource cleaning happens here
    protected virtual void Dispose(bool disposing)
    {
        // check if already disposed
        if (!disposedValue)
        {
            if (disposing)
            {
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
        Dispose(disposing: true);

        // Notify the garbage collector
        // about the cleaning event
        GC.SuppressFinalize(this);
    }

    ~UpdateSubCategoryJob() { Dispose(disposing: false); }
}