using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities.NOnbir;
using MediatR;

namespace Application.N11.Commands;

public record UpdateCategoryCommand : IRequest
{
    public int Id { get; init; }
    public bool IsDeepest { get; set; }
    public bool HasError { get; set; }
    public int? ParentId { get; set; }
}

public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateCategoryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.N11Category
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Category), request.Id);
        }

        entity.IsDeepest = request.IsDeepest;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
