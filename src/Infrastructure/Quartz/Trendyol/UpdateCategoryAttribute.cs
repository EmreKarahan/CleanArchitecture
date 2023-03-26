using Domain.Entities.Trendyol;
using Shared.Attributes;
using Attribute = Domain.Entities.Trendyol.Attribute;
using CategoryAttribute = Minima.Trendyol.Client.Models.Service.Category.Response.CategoryAttribute;

namespace Infrastructure.Quartz.Trendyol;

[ScheduledJob("UpdateCategoryAttribute", "Trendyol", "UpdateCategoryAttributeTrigger", "Trendyol", "0 /1 * ? * *")]
public class UpdateCategoryAttribute : IJob
{
    readonly ICategoryApiService _categoryApiService;
    readonly IRepository<Category> _categporyRepository;
    readonly IRepository<Attribute> _attributeRepository;

    public UpdateCategoryAttribute(ICategoryApiService categoryApiService, IRepository<Category> categporyRepository,
        IRepository<Attribute> attributeRepository)
    {
        _categoryApiService = categoryApiService;
        _categporyRepository = categporyRepository;
        _attributeRepository = attributeRepository;
    }
    public async Task Execute(IJobExecutionContext context)
    {
        await GetCategoryAttributes();
    }

    private async Task GetCategoryAttributes()
    {
        var result = await _categporyRepository.GetAllAsync();

        foreach (var category in result)
        {
            var attributes = await _categoryApiService.GetCategoryAttributeByCategoryIdAsync(category.InternalId);
            if (!attributes.IsSuccess)
            {
                continue;
            }

            if (attributes.Data.CategoryAttributes != null && attributes.Data.CategoryAttributes.Length > 0)
            {
                category.HasAttribute = true;
                await _categporyRepository.UpdateAsync(category);
            }

            await InsertDatabase(attributes.Data, category);
        }
    }

    private async Task InsertDatabase(TrendyolCategoryAttributeResponse attributesData, Category category)
    {
        foreach (CategoryAttribute categoryAttribute in attributesData.CategoryAttributes)
        {
            var attribute = new Attribute
            {
                InternalId = categoryAttribute.Attribute.Id,
                Name = categoryAttribute.Attribute.Name,
                Required = categoryAttribute.CategoryAttributeRequired,
                AllowCustom = categoryAttribute.AllowCustom,
                Slicer = categoryAttribute.Slicer,
                Varianter = categoryAttribute.Varianter,
                CategoryId = category.Id,
                AttributeValues = new List<AttributeValue>()
            };

            foreach (var attributeValue in categoryAttribute.AttributeValues)
            {
                var attributeValue1 = new AttributeValue
                {
                    InternalId = attributeValue.Id, Name = attributeValue.Name, AttributeId = attribute.Id
                };
                attribute.AttributeValues.Add(attributeValue1);
            }
            
            Attribute existAttribute = await _attributeRepository.GetByAsync(f =>
                f.InternalId == categoryAttribute.Attribute.Id && f.CategoryId == category.Id);
            
            if (existAttribute != null)
            {
                await _attributeRepository.InsertAsync(attribute);
            }
        }
    }


}