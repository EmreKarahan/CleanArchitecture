using Application.Common.Interfaces;
using Domain.Entities.NOnbir;
using Domain.Events.NOnbir;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.N11.Commands;

public class CreateAttributeValueCommand : IRequest<int>
{
    public int AttributeId { get; set; }
    public string Name { get; set; }
    public string DependedName { get; set; }
}

public class CreateAttributeValueCommandHandler : IRequestHandler<CreateAttributeValueCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateAttributeValueCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateAttributeValueCommand request, CancellationToken cancellationToken)
    {
        var attributeValueCheck = await _context.N11AttributeValue.AsQueryable().FirstOrDefaultAsync(f =>
            f.AttributeId == request.AttributeId && f.Name == request.Name, cancellationToken: cancellationToken);

        if (attributeValueCheck is not null)
            return attributeValueCheck.Id;
        
        var entity = new AttributeValue
        {
            DependedName = request.DependedName,
            AttributeId = request.AttributeId,
            Name = request.Name
        };

        entity.AddDomainEvent(new AttributeValueCreatedEvent(entity));

        _context.N11AttributeValue.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}