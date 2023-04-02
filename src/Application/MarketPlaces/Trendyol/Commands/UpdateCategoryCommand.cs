using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities.Trendyol;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.MarketPlaces.Trendyol.Commands;

public record UpdateCategoryCommand : IRequest
{
    public int Id { get; init; }
    public bool IsDeepest { get; set; }
    public int InternalId { get; set; }
    public bool HasAttribute { get; set; }
}

public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand>
{
    private readonly IApplicationDbContext _context;
    readonly ILogger<UpdateCategoryCommandHandler> _logger;

    public UpdateCategoryCommandHandler(
        IApplicationDbContext context, 
        ILogger<UpdateCategoryCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity =
            await _context.TrendyolCategory.FirstOrDefaultAsync(f => f.InternalId == request.InternalId, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Category), request.Id);
        }

        entity.IsDeepest = request.IsDeepest;
        entity.HasAttribute = request.HasAttribute;

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation($"{entity.Name} Category Updated. - {entity.Id}");
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
        }
    }
}
