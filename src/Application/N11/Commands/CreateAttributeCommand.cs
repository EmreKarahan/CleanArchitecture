using Application.Common.Interfaces;
using Domain.Events.NOnbir;
using MediatR;
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

    public CreateAttributeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateAttributeCommand request, CancellationToken cancellationToken)
    {
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

        return entity.Id;
    }
}