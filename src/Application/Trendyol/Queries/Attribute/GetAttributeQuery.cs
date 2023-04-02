using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Trendyol.Queries.Attribute;

public record GetAttributeQuery() : IRequest<AttributeValueVm>
{
    public long AttributeId { get; set; }
}

public class GetTodosQueryHandler : IRequestHandler<GetAttributeQuery, AttributeValueVm>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetTodosQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<AttributeValueVm> Handle(GetAttributeQuery request, CancellationToken cancellationToken)
    {
        return new AttributeValueVm
        {
            Lists = await _context.TrendyolAttributeValue
                .AsNoTracking()
                .Where(w => w.CategoryAttributeId == request.AttributeId)
                .ProjectTo<AttributeValueItemDto>(_mapper.ConfigurationProvider)
                .OrderBy(t => t.Name)
                .ToListAsync(cancellationToken)
        };
    }
}