using Domain.Entities.NOnbir;
using Minima.MarketPlace.NOnbir.Models.ReturnType.Category.Requests;
using Minima.MarketPlace.NOnbir.Models.Service.Category.Types;
using Shared.Attributes;
using Attribute = Domain.Entities.NOnbir.Attribute;
using ICategoryApiService = Minima.MarketPlace.NOnbir.Services.ICategoryApiService;

namespace Infrastructure.Quartz.NOnbir;

[ScheduledJob("UpdateCategoryAttributeJob", "N11", "UpdateCategoryAttributeJobTrigger", "N11", "0 /1 * ? * *")]
public class UpdateCategoryAttributeJob : IJob, IDisposable
{
    readonly ICategoryApiService _categoryApiService;
    readonly IRepository<Attribute> _attributeRepository;
    readonly IRepository<Category> _categoryRepository;
    
    // boolean variable to ensure dispose
    // method executes only once
    private bool disposedValue;

    public UpdateCategoryAttributeJob(
        ICategoryApiService categoryApiService, 
        IRepository<Attribute> attributeRepository, 
        IRepository<Category> categoryRepository)
    {
        _categoryApiService = categoryApiService;
        _attributeRepository = attributeRepository;
        _categoryRepository = categoryRepository;
    }
    
    public async Task Execute(IJobExecutionContext context)
    {
        
        string instName = context.JobDetail.Key.Name;
        JobDataMap dataMap = context.JobDetail.JobDataMap;

        long categoryId = dataMap.GetLongValue("categoryId");
        Console.WriteLine("Instance {0} of DumbJob says: {1}", instName, categoryId);

        GetCategoryAttributes(categoryId).GetAwaiter().GetResult();
    }
    
    public async Task GetCategoryAttributes(long categoryId)
    {
        var category = _categoryRepository.GetBy(i => i.InternalId == categoryId);
        var categoryAttributesResponse = _categoryApiService.GetCategoryAttributes(new GetCategoryAttributesRequestReturn
        {
            CategoryId = categoryId
        });

        if (categoryAttributesResponse.Result.Status != "success")
            return;

        foreach (CategoryAttributeData data in categoryAttributesResponse.Category.AttributeList)
        {
            var attributeCheck = _attributeRepository.GetBy(b => b.InternalId == data.Id);
            if (attributeCheck != null)
            {
                continue;
            }

            var attribute = new Attribute
            {
                InternalId = data.Id,
                Name = data.Name,
                Mandatory = data.Mandatory,
                MultipleSelect = data.MultipleSelect,
                Priority = data.Priority,
                CategoryId = category.Id
            };
            attribute.AttributeValues = data.ValueList.Select(s => new AttributeValue
            {
                //InternalId = s.Id, 
                Name = s.Name,
                DependedName = s.DependedName
            }).ToList();
            try
            {
                _attributeRepository.Insert(attribute);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            

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
    ~UpdateCategoryAttributeJob() { Dispose(disposing : false); }
}