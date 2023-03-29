using System.Data;
using Application.Common.Exceptions;
using Dapper;

namespace Infrastructure.Persistence;

public class DapperRepository : IDapperRepository
{
    private readonly ApplicationDbContext _dbContext;

    public DapperRepository(ApplicationDbContext dbContext) => _dbContext = dbContext;

    public async Task<IReadOnlyList<T>> QueryAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
    where T : BaseEntity
    {
        return (await _dbContext.Connection.QueryAsync<T>(sql, param, transaction))
            .AsList();
    }

    public async Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
    where T : BaseEntity
    {
        
        var entity = (await _dbContext.Connection.QueryFirstOrDefaultAsync<T>(sql, param, transaction));
        return entity ?? throw new NotFoundException(string.Empty);
        
    }

    public Task<T> QuerySingleAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
    where T : BaseEntity
    {
        return _dbContext.Connection.QuerySingleAsync<T>(sql, param, transaction);
    }
}