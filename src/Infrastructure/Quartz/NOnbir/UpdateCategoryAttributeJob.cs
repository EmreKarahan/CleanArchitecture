using Domain.Entities.NOnbir;
using Minima.MarketPlace.NOnbir.Models.ReturnType.Category.Requests;
using Minima.MarketPlace.NOnbir.Models.Service.Category.Types;
using Shared.Attributes;
using Attribute = Domain.Entities.NOnbir.Attribute;
using ICategoryApiService = Minima.MarketPlace.NOnbir.Services.ICategoryApiService;

namespace Infrastructure.Quartz.NOnbir;

//[ScheduledJob("UpdateCategoryAttributeJob", "N11", "UpdateCategoryAttributeJobTrigger", "N11", "0 /1 * ? * *")]
public class UpdateCategoryAttributeJob : IJob
{
    readonly ICategoryApiService _categoryApiService;
    readonly IRepository<Attribute> _attributeRepository;
    readonly IRepository<Category> _categoryRepository;

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
        //throw new NotImplementedException();
        GetCategoryAttributes(1000000).GetAwaiter().GetResult();
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


}