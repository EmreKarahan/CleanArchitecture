using System.Linq.Expressions;
using Application.Common.Extensions;
using Domain.Common;

namespace Application.Common.Interfaces
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        #region Sync Methods

        TEntity Insert(TEntity entity);
        TEntity Update(TEntity entity);
        TEntity Delete(TEntity entity);
        void DeleteRange(IEnumerable<TEntity> entity);

        IEnumerable<TEntity> GetAll();
        IQueryable<TEntity> QueryAble();
        IQueryable<TEntity> QueryAble(Func<TEntity, bool> predicate);
        TEntity GetBy(Func<TEntity, bool> predicate);
        IList<TEntity> GetAllBy(Func<TEntity, bool> predicate);
        IQueryable<TEntity> GetAllByQ(Func<TEntity, bool> predicate);
        IEnumerable<TEntity> GetAllBy(
            Expression<Func<TEntity, bool>> filter = null,
            string[] includePaths = null,
            int? page = 0, int? pageSize = null,
            params SortExpression<TEntity>[] sortExpressions);

        bool Exists(Expression<Func<TEntity, bool>> predicate);

        //IEnumerable<TModel> ExecuteStoredProcedure<TModel>(string rawSql, (string, object)[] paramList)
        //    where TModel : class, new();

        //IEnumerable<TModel> ExecuteStoredProcedure<TModel>(string rawSql) where TModel : class, new();


        #endregion

        #region Async Methods

        Task<TEntity> InsertAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);

        Task DeleteAsync(Expression<Func<TEntity, bool>> predicate);

        Task<IReadOnlyList<TEntity>> GetAllAsync();
        Task<TEntity> GetByAsync(Expression<Func<TEntity, bool>> predicate);
        Task<IReadOnlyList<TEntity>> GetAllByAsync(Expression<Func<TEntity, bool>> predicate);
        Task<IReadOnlyList<TEntity>> GetAllByAsync(
            Expression<Func<TEntity, bool>> predicate,
            string[] includePaths = null,
            int? page = 0, int? pageSize = null,
            params SortExpression<TEntity>[] sortExpressions);


        //Task<IEnumerable<TModel>> ExecuteStoredProcedureAsync<TModel>(string rawSql, (string, object)[] paramList)
        //    where TModel : class, new();

        //Task<IEnumerable<TModel>> ExecuteStoredProcedureAsync<TModel>(string rawSql) where TModel : class, new();
        #endregion
    }
}