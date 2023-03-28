using Application.Common.Interfaces;
using Domain.Entities.NOnbir;
using Domain.Events.NOnbir;
using MediatR;

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
        var entity = new AttributeValue()
        {
        };

        entity.AddDomainEvent(new AttributeValueCreatedEvent(entity));

        _context.N11AttributeValue.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}