using Application.Common.Interfaces;
using Domain.Events.Trendyol;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Attribute = Domain.Entities.Trendyol.Attribute;

namespace Application.Trendyol.Commands;


public class CreateAttributeCommand : IRequest<int>
{
    public long InternalId { get; set; }
    public int CategoryId { get; set; }
    public string Name { get; set; }
    public bool Required { get; set; }
    public bool AllowCustom { get; set; }
    public bool Slicer { get; set; }
    public bool Varianter { get; set; }
}

public class CreateAttributeCommandHandler : IRequestHandler<CreateAttributeCommand, int>
{
    private readonly IApplicationDbContext _context;
    readonly ILogger<CreateAttributeCommandHandler> _logger;

    public CreateAttributeCommandHandler(IApplicationDbContext context, ILogger<CreateAttributeCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<int> Handle(CreateAttributeCommand request, CancellationToken cancellationToken)
    {

        var attributeCheck = await _context.TrendyolAttribute.AsQueryable().FirstOrDefaultAsync(f =>
            f.InternalId == request.InternalId && f.CategoryId == request.CategoryId, cancellationToken: cancellationToken);

        if (attributeCheck is not null)
            return attributeCheck.Id;
        
        var entity = new Attribute
        {
            InternalId = request.InternalId,
            CategoryId = request.CategoryId,
            Name = request.Name,
            Required = request.Required,
            AllowCustom = request.AllowCustom,
            Slicer = request.Slicer,
            Varianter = request.Varianter,
        };

        entity.AddDomainEvent(new AttributeCreatedEvent(entity));

        _context.TrendyolAttribute.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);
        _logger.LogInformation($"{entity.Name} attribute created. - {entity.Id}");

        return entity.Id;
    }
}