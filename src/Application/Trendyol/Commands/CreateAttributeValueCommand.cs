using Application.Common.Interfaces;
using Domain.Entities.Trendyol;
using Domain.Events.Trendyol;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Trendyol.Commands;

public class CreateAttributeValueCommand : IRequest<int>
{
    public int AttributeId { get; set; }
    public string Name { get; set; }
    public int InternalId { get; set; }
    public int CategoryId { get; set; }
}

public class CreateAttributeValueCommandHandler : IRequestHandler<CreateAttributeValueCommand, int>
{
    private readonly IApplicationDbContext _context;
    readonly ILogger<CreateAttributeValueCommandHandler> _logger;

    public CreateAttributeValueCommandHandler(
        IApplicationDbContext context, 
        ILogger<CreateAttributeValueCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<int> Handle(CreateAttributeValueCommand request, CancellationToken cancellationToken)
    {
        var attributeValueCheck = await _context.TrendyolAttributeValue
            .AsQueryable()
            .FirstOrDefaultAsync(f =>
                f.CategoryToAttribute.AttributeId == request.AttributeId &&
                f.CategoryToAttribute.CategoryId == request.CategoryId &&
                f.Name == request.Name, cancellationToken: cancellationToken);

        if (attributeValueCheck is not null)
        {
            _logger.LogInformation($"{attributeValueCheck.Name} AttributeValue already exists. - {attributeValueCheck.Id}");
            return attributeValueCheck.Id;
        }
         
        var categoryToAttributeCheck = await _context.TrendyolCategoryToAttribute
            .AsQueryable()
            .FirstOrDefaultAsync(f =>
                f.AttributeId == request.AttributeId &&
                f.CategoryId == request.CategoryId, cancellationToken: cancellationToken);
        
        var entity = new AttributeValue
        {
            //AttributeId = request.AttributeId,
            Name = request.Name,
            InternalId = request.InternalId,
            CategoryAttributeId = categoryToAttributeCheck.Id
        };

        entity.AddDomainEvent(new AttributeValueCreatedEvent(entity));

        _context.TrendyolAttributeValue.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);
        _logger.LogInformation($"{entity.Name} AttributeValue created. - {entity.Id}");
        return entity.Id;
    }
}