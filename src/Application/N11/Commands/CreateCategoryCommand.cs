using Application.Common.Interfaces;
using Domain.Entities.NOnbir;
using Domain.Events.NOnbir;
using MediatR;

namespace Application.N11.Commands;

public class CreateCategoryCommand : IRequest<int>
{
    public string Name { get; set; }
    public long InternalId { get; set; }
    public long InternalParentId { get; set; }
    public int ParentId { get; set; }
    public bool IsDeepest { get; set; }
}

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateCategoryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = new Category
        {
          InternalId = request.InternalId,
          ParentId = request.ParentId,
          InternalParentId = request.InternalParentId,
          Name = request.Name,
          IsDeepest = request.IsDeepest
        };

        entity.AddDomainEvent(new CategoryCreatedEvent(entity));

        _context.N11Category.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}