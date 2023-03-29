using Application.N11.Commands;
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
    IMediator _mediator;
    
    // boolean variable to ensure dispose
    // method executes only once
    private bool disposedValue;

    public UpdateCategoryAttributeJob(
        ICategoryApiService categoryApiService, 
        IRepository<Attribute> attributeRepository, 
        IRepository<Category> categoryRepository, 
        IMediator mediator)
    {
        _categoryApiService = categoryApiService;
        _attributeRepository = attributeRepository;
        _categoryRepository = categoryRepository;
        _mediator = mediator;
    }
    
    public async Task Execute(IJobExecutionContext context)
    {
        
        string instName = context.JobDetail.Key.Name;
        JobDataMap dataMap = context.JobDetail.JobDataMap;

        long categoryId = dataMap.GetLongValue("categoryId");
        Console.WriteLine("Instance {0} of DumbJob says: {1}", instName, categoryId);

        GetCategoryAttributes(categoryId).GetAwaiter().GetResult();
    }

    private async Task GetCategoryAttributes(long categoryId)
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
            // var attributeCheck = _attributeRepository.GetBy(b => b.InternalId == data.Id);
            // if (attributeCheck != null)
            // {
            //     continue;
            // }

            // var attribute = new Attribute
            // {
            //     InternalId = data.Id,
            //     Name = data.Name,
            //     Mandatory = data.Mandatory,
            //     MultipleSelect = data.MultipleSelect,
            //     Priority = data.Priority,
            //     CategoryId = category.Id
            // };
            var createAttributeCommand = new CreateAttributeCommand
            {
                InternalId = data.Id,
                Name = data.Name,
                Mandatory = data.Mandatory,
                MultipleSelect = data.MultipleSelect,
                Priority = data.Priority,
                CategoryId = category.Id
            };
            var attributeId = await _mediator.Send(createAttributeCommand);
            
            var createAttributeValueCommands = data.ValueList.Select(s => new CreateAttributeValueCommand()
            {
                AttributeId = attributeId,
                Name = s.Name,
                DependedName = s.DependedName
            }).ToList();
            foreach (var createAttributeValueCommand in createAttributeValueCommands)
            {
                await _mediator.Send(createAttributeValueCommand);
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