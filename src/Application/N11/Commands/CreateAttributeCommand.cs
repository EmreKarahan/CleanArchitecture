using Application.Common.Interfaces;
using Domain.Events.NOnbir;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Attribute = Domain.Entities.NOnbir.Attribute;

namespace Application.N11.Commands;


public class CreateAttributeCommand : IRequest<int>
{
    public long InternalId { get; set; }
    public string Name { get; set; }
    public bool Mandatory { get; set; }
    public bool MultipleSelect { get; set; }
    public double Priority { get; set; }
    public int CategoryId { get; set; }
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

        var attributeCheck = await _context.N11Attribute.AsQueryable().FirstOrDefaultAsync(f =>
            f.InternalId == request.InternalId && f.CategoryId == request.CategoryId, cancellationToken: cancellationToken);

        if (attributeCheck is not null)
            return attributeCheck.Id;
        
        var entity = new Attribute
        {
            InternalId = request.InternalId,
            CategoryId = request.CategoryId,
            Mandatory = request.Mandatory,
            MultipleSelect = request.MultipleSelect,
            Name = request.Name,
            Priority = request.Priority,
        };

        entity.AddDomainEvent(new AttributeCreatedEvent(entity));

        _context.N11Attribute.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);
        _logger.LogInformation($"{entity.Name} attribute created. - {entity.Id}");

        return entity.Id;
    }
}