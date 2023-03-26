namespace Infrastructure.Persistence;

public class EntityRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity

{
    private readonly ApplicationDbContext _minimaContext;
    private readonly DbSet<TEntity> _entities;

    #region ctor
    public EntityRepository(ApplicationDbContext context)
    {
        _minimaContext = context;
        _entities = context.Set<TEntity>();
    }
    #endregion

    #region Sync Methods    

    private async Task<int> SaveChangesAsync()
    {
        return await _minimaContext.SaveChangesAsync();
    }

    private int SaveChanges()
    {
        return _minimaContext.SaveChanges();
    }
    #endregion

    #region Sync Methods              
    public TEntity Insert(TEntity entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        _entities.Add(entity);
        SaveChanges();
        return entity;
    }

    public TEntity Update(TEntity entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        _entities.Attach(entity);
        _minimaContext.Entry(entity).State = EntityState.Modified;
        SaveChanges();

        return entity;
    }

    public TEntity Delete(TEntity entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        _entities.Attach(entity);
        _minimaContext.Entry(entity).State = EntityState.Deleted;
        SaveChanges();
        return entity;
    }

    public void DeleteRange(IEnumerable<TEntity> entity)
    {
        _minimaContext.RemoveRange(entity);
        SaveChanges();
    }

    public IEnumerable<TEntity> GetAll() =>
        _entities.AsNoTracking().AsEnumerable();

    public IQueryable<TEntity> QueryAble()
    {
        return _entities.AsQueryable();
    }

    public IQueryable<TEntity> QueryAble(Func<TEntity, bool> predicate)
    {
        return _entities.Where(predicate).AsQueryable();
    }

    public TEntity GetBy(Func<TEntity, bool> predicate)
    {
        if (predicate == null)
            throw new ArgumentNullException(nameof(predicate));

        var result = _entities.FirstOrDefault(predicate);
        return result;
    }

    public IList<TEntity> GetAllBy(Func<TEntity, bool> predicate)
    {
        if (predicate == null)
            throw new ArgumentNullException(nameof(predicate));

        return _entities.Where(predicate).ToList();
    }

    public IQueryable<TEntity> GetAllByQ(Func<TEntity, bool> predicate)
    {
        return _entities.Where(predicate).AsQueryable();
    }

    public IEnumerable<TEntity> GetAllBy(
        Expression<Func<TEntity, bool>> filter = null,
        string[] includePaths = null,
        int? page = 0, int? pageSize = null,
        params SortExpression<TEntity>[] sortExpressions)
    {
        IQueryable<TEntity> query = _entities;

        if (filter != null)
        {
            query = query.AsNoTracking().Where(filter);
        }

        if (includePaths != null)
        {
            for (var i = 0; i < includePaths.Count(); i++)
            {
                query = query.Include(includePaths[i]);
            }
        }

        if (sortExpressions != null)
        {
            IOrderedQueryable<TEntity> orderedQuery = null;
            for (var i = 0; i < sortExpressions.Count(); i++)
            {
                if (i == 0)
                {
                    orderedQuery = sortExpressions[i].SortDirection == ListSortDirection.Ascending ? query.OrderBy(sortExpressions[i].SortBy) : query.OrderByDescending(sortExpressions[i].SortBy);
                }
                else
                {
                    orderedQuery = sortExpressions[i].SortDirection == ListSortDirection.Ascending ? orderedQuery.ThenBy(sortExpressions[i].SortBy) : orderedQuery.ThenByDescending(sortExpressions[i].SortBy);
                }
            }

            if (page != null)
            {
                query = orderedQuery.Skip(((int)page - 1) * (int)pageSize);
            }
        }

        if (pageSize != null)
        {
            query = query.Take((int)pageSize);
        }

        return query.ToList();
    }

    public bool Exists(Expression<Func<TEntity, bool>> predicate)
    {
        return _entities.AsQueryable().Any(predicate);
    }

    //public IEnumerable<TModel> ExecuteStoredProcedure<TModel>(string rawSql, (string, object)[] paramList) where TModel : class, new()
    //{
    //    List<TModel> model = null;

    //    var spBuilder = _minimaContext.LoadStoredProc(rawSql);
    //    foreach ((string, object) tuple in paramList)
    //    {
    //        spBuilder.AddParam(tuple.Item1, tuple.Item2);
    //    }

    //    spBuilder.Exec(r => model = r.ToList<TModel>());

    //    return model;
    //    //return _entities.FromSqlRaw(rawSql, paramList).ToList();
    //}

    //public IEnumerable<TModel> ExecuteStoredProcedure<TModel>(string rawSql) where TModel : class, new()
    //{
    //    List<TModel> model = null;
    //    _minimaContext.LoadStoredProc(rawSql).Exec(r => model = r.ToList<TModel>());
    //    return model;
    //}

    #endregion

    #region Async Methods      

    public async Task<TEntity> InsertAsync(TEntity entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        await _entities.AddAsync(entity);
        await SaveChangesAsync();
        return entity;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        _entities.Attach(entity);
        _minimaContext.Entry(entity).State = EntityState.Modified;
        await SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(TEntity entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        _entities.Attach(entity);
        _minimaContext.Entry(entity).State = EntityState.Deleted;
        await SaveChangesAsync();
    }

    public async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate)
    {
        if (predicate == null)
            throw new ArgumentNullException(nameof(predicate));

        var entity = await GetByAsync(predicate);

        if (entity == null)
            throw new ArgumentNullException(nameof(entity));


        _entities.Attach(entity);
        _minimaContext.Entry(entity).State = EntityState.Deleted;
        await SaveChangesAsync();
    }

    public async Task<IReadOnlyList<TEntity>> GetAllAsync() =>
        await _entities.ToListAsync();

    public async Task<TEntity> GetByAsync(Expression<Func<TEntity, bool>> predicate)
    {
        if (predicate == null)
            throw new ArgumentNullException(nameof(predicate));

        var result = await _entities.FirstOrDefaultAsync(predicate);
        return result;
    }

    public async Task<IReadOnlyList<TEntity>> GetAllByAsync(Expression<Func<TEntity, bool>> predicate)
    {
        if (predicate == null)
            throw new ArgumentNullException("predicate");

        return await _entities.Where(predicate).ToListAsync();
    }

    public async Task<IReadOnlyList<TEntity>> GetAllByAsync(Expression<Func<TEntity, bool>> filter, string[] includePaths, int? page = 0, int? pageSize = null, params SortExpression<TEntity>[] sortExpressions)
    {

        IQueryable<TEntity> query = _entities;

        if (filter != null)
        {
            query = query.AsNoTracking().Where(filter);
        }

        if (includePaths != null)
        {
            for (var i = 0; i < includePaths.Count(); i++)
            {
                query = query.Include(includePaths[i]);
            }
        }

        if (sortExpressions != null)
        {
            IOrderedQueryable<TEntity> orderedQuery = null;
            for (var i = 0; i < sortExpressions.Count(); i++)
            {
                if (i == 0)
                {
                    orderedQuery = sortExpressions[i].SortDirection == ListSortDirection.Ascending ? query.OrderBy(sortExpressions[i].SortBy) : query.OrderByDescending(sortExpressions[i].SortBy);
                }
                else
                {
                    orderedQuery = sortExpressions[i].SortDirection == ListSortDirection.Ascending ? orderedQuery.ThenBy(sortExpressions[i].SortBy) : orderedQuery.ThenByDescending(sortExpressions[i].SortBy);
                }
            }

            if (page != null)
            {
                if (pageSize != null) query = orderedQuery.Skip(((int)page - 1) * (int)pageSize);
            }
        }

        if (pageSize != null)
        {
            query = query.Take((int)pageSize);
        }
        return await query.ToListAsync();
    }

    //public async Task<IEnumerable<TModel>> ExecuteStoredProcedureAsync<TModel>(string rawSql, (string, object)[] paramList) where TModel : class, new()
    //{
    //    Task<List<TModel>> model = null;

    //    var spBuilder = _minimaContext.LoadStoredProc(rawSql);
    //    foreach ((string, object) tuple in paramList)
    //    {
    //        spBuilder.AddParam(tuple.Item1, tuple.Item2);
    //    }

    //    await spBuilder.ExecAsync(r => model = r.ToListAsync<TModel>());

    //    return model.GetAwaiter().GetResult();
    //    //return _entities.FromSqlRaw(rawSql, paramList).ToList();
    //}

    //public async Task<IEnumerable<TModel>> ExecuteStoredProcedureAsync<TModel>(string rawSql) where TModel : class, new()
    //{
    //    Task<List<TModel>> model = null;
    //    await _minimaContext.LoadStoredProc(rawSql).ExecAsync(r => model = r.ToListAsync<TModel>());
    //    return model.GetAwaiter().GetResult();
    //}

    #endregion
}