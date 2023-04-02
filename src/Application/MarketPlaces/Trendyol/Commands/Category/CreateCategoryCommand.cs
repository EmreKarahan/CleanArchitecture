using Application.Common.Interfaces;
using Domain.Events.Trendyol;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.MarketPlaces.Trendyol.Commands.Category;

public class CreateCategoryCommand : IRequest<int>
{
    public string Name { get; set; }
    public int InternalId { get; set; }

    public int? ParentId { get; set; }

    public int? InternalParentId { get; set; }
    //public List<Category> SubCategories { get; set; }
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
        var categoryCheck = await _context.TrendyolCategory.FirstOrDefaultAsync(f => f.InternalId == request.InternalId,
            cancellationToken: cancellationToken);

        if (categoryCheck is not null)
            return categoryCheck.Id;
        
        var entity = new Domain.Entities.Trendyol.Category
        {
            InternalId = request.InternalId,
            Name = request.Name,
            ParentId = request.ParentId,
            InternalParentId = request.InternalParentId
        };

        entity.AddDomainEvent(new CategoryCreatedEvent(entity));

        try
        {
            _context.TrendyolCategory.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation($"{entity.Name} Category created. - {entity.Id}");
        }
        catch (Exception e)
        {
            _logger.LogInformation(e, e.Message);
        }
        
        return entity.Id;
    }
}