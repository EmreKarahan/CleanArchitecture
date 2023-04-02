using Application.MarketPlaces.Trendyol.Commands;
using Application.MarketPlaces.Trendyol.Commands.Attribute;
using Application.MarketPlaces.Trendyol.Commands.Category;
using Domain.Entities.Trendyol;
using Microsoft.Extensions.Logging;
using Shared.Attributes;
using CategoryAttribute = Minima.Trendyol.Client.Models.Service.Category.Response.CategoryAttribute;

namespace Infrastructure.Quartz.Trendyol;

[ScheduledJob("UpdateCategoryAttribute", "Trendyol", "UpdateCategoryAttributeTrigger", "Trendyol", "0 9 4 1/1 * ? *",
    true)]
public class UpdateCategoryAttribute //: IJob
{
    readonly ICategoryApiService _categoryApiService;
    readonly IRepository<Category> _categporyRepository;
    readonly IMediator _mediator;
    ILogger<UpdateCategoryAttribute> _logger;

    public UpdateCategoryAttribute(
        ICategoryApiService categoryApiService,
        IRepository<Category> categporyRepository,
        IMediator mediator,
        ILogger<UpdateCategoryAttribute> logger)
    {
        _categoryApiService = categoryApiService;
        _categporyRepository = categporyRepository;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        await GetCategoryAttributes();
    }

    private async Task GetCategoryAttributes()
    {
        var result = await _categporyRepository.GetAllAsync(); //.GetAllByAsync(f=> f.IsDeepest);

        var categoryCount = 0;
        foreach (var category in result)
        {
            var attributes = await _categoryApiService.GetCategoryAttributeByCategoryIdAsync(category.InternalId);
            if (!attributes.IsSuccess)
            {
                continue;
            }

            if (attributes.Data.CategoryAttributes != null && attributes.Data.CategoryAttributes.Length > 0)
            {
                var updateCategoryCommand = new UpdateCategoryCommand
                {
                    Id = category.Id,
                    InternalId = category.InternalId,
                    HasAttribute = true,
                    IsDeepest = category.IsDeepest
                };
                await _mediator.Send(updateCategoryCommand);
            }
            else
            {
                _logger.LogWarning("Category has not attribute");
            }

            await InsertDatabase(attributes.Data, category);
            categoryCount++;
            _logger.LogWarning(categoryCount + " category attribute inserted");
        }
    }

    private async Task InsertDatabase(TrendyolCategoryAttributeResponse attributesData, Category category)
    {
        foreach (CategoryAttribute categoryAttribute in attributesData.CategoryAttributes)
        {
            var createAttributeCommand = new CreateAttributeCommand
            {
                InternalId = categoryAttribute.Attribute.Id,
                Name = categoryAttribute.Attribute.Name,
                Required = categoryAttribute.CategoryAttributeRequired,
                AllowCustom = categoryAttribute.AllowCustom,
                Slicer = categoryAttribute.Slicer,
                Varianter = categoryAttribute.Varianter,
                CategoryId = category.Id,
                //AttributeValues = new List<AttributeValue>()
            };
            var attributeId = await _mediator.Send(createAttributeCommand);
            
            
            foreach (var attributeValue in categoryAttribute.AttributeValues)
            {
                var createAttributeValueCommand = new CreateAttributeValueCommand
                {
                    InternalId = attributeValue.Id, 
                    Name = attributeValue.Name, 
                    AttributeId = attributeId.Id,
                    CategoryId = category.Id
                };
                await _mediator.Send(createAttributeValueCommand);
                //attribute.AttributeValues.Add(attributeValue1);
            }

            // Attribute existAttribute = await _attributeRepository.GetByAsync(f =>
            //     f.InternalId == categoryAttribute.Attribute.Id && f.CategoryId == category.Id);

            // if (existAttribute != null)
            // {
            //     await _attributeRepository.InsertAsync(attribute);
            // }
        }
    }
}