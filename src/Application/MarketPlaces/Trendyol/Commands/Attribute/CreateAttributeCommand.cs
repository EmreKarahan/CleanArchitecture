using Application.Common.Interfaces;
using Domain.Entities.Trendyol;
using Domain.Events.Trendyol;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.MarketPlaces.Trendyol.Commands.Attribute;

public class CreateAttributeCommand : IRequest<Domain.Entities.Trendyol.Attribute>
{
    public long InternalId { get; set; }
    public int CategoryId { get; set; }
    public string Name { get; set; }
    public bool Required { get; set; }
    public bool AllowCustom { get; set; }
    public bool Slicer { get; set; }
    public bool Varianter { get; set; }
}

public class CreateAttributeCommandHandler : IRequestHandler<CreateAttributeCommand, Domain.Entities.Trendyol.Attribute>
{
    private readonly IApplicationDbContext _context;
    readonly ILogger<CreateAttributeCommandHandler> _logger;

    public CreateAttributeCommandHandler(IApplicationDbContext context, ILogger<CreateAttributeCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Domain.Entities.Trendyol.Attribute> Handle(CreateAttributeCommand request, CancellationToken cancellationToken)
    {
        var attributeCheck = await _context.TrendyolAttribute.AsQueryable()
            .FirstOrDefaultAsync(f =>
                f.InternalId == request.InternalId, cancellationToken: cancellationToken);

        if (attributeCheck is null)
        {
            var entity = new Domain.Entities.Trendyol.Attribute
            {
                InternalId = request.InternalId,
                //CategoryId = request.CategoryId,
                Name = request.Name,
                Required = request.Required,
                AllowCustom = request.AllowCustom,
                Slicer = request.Slicer,
                Varianter = request.Varianter,
            };


            entity.AddDomainEvent(new AttributeCreatedEvent(entity));

            _context.TrendyolAttribute.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            var categoryToAttributeEntity = new CategoryToAttribute
            {
                CategoryId = request.CategoryId, AttributeId = entity.Id
            };
            
            _context.TrendyolCategoryToAttribute.Add(categoryToAttributeEntity);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation($"{entity.Name} attribute created. - {entity.Id}");

            return entity;
        }

        var categoryAttributeCheck = await _context.TrendyolAttribute.AsQueryable()
            .Include(f => f.CategoryToAttributes)
            .FirstOrDefaultAsync(f =>
                    f.InternalId == request.InternalId &&
                    f.CategoryToAttributes.Any(a => a.CategoryId == request.CategoryId),
                cancellationToken: cancellationToken);

        if (categoryAttributeCheck is null)
        {
            var categoryToAttributeEntity = new CategoryToAttribute
            {
                CategoryId = request.CategoryId, AttributeId = attributeCheck.Id
            };

            _context.TrendyolCategoryToAttribute.Add(categoryToAttributeEntity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        return attributeCheck;
    }
}