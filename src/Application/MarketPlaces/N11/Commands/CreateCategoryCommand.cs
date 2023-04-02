using Application.Common.Interfaces;
using Domain.Entities.NOnbir;
using Domain.Events.NOnbir;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.MarketPlaces.N11.Commands;

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
    readonly ILogger<CreateCategoryCommandHandler> _logger;

    public CreateCategoryCommandHandler(
        IApplicationDbContext context, 
        ILogger<CreateCategoryCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<int> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var categoryCheck = await _context.N11Category.FirstOrDefaultAsync(f => f.InternalId == request.InternalId,
            cancellationToken: cancellationToken);

        if (categoryCheck is not null)
            return categoryCheck.Id;
        
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
        _logger.LogInformation($"{entity.Name} Category created. - {entity.Id}");

        return entity.Id;
    }
}