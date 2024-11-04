using System.Linq.Expressions;

namespace AGOC.Repository.Interfaces
{
    public interface IRepositoryBase<T>
    {
        IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression);

        Task<T> GetByIdAsync(int id);

        Task<IEnumerable<T>> GetAllAsync();

        Task AddAsync(T entity);

        void Update(T entity);

        void Delete(T entity);

        Task<int> Count(Expression<Func<T, bool>> expression);

        public bool Any(Expression<Func<T, bool>> expression);

        void Add(List<T> entities);

        void Update(List<T> entities);

        void Delete(List<T> entity);

        IQueryable<T1> Get<TResult, T1>(
        Expression<Func<T, bool>> filter = null,
        Expression<Func<T, TResult>> orderBy = null,
        Func<IQueryable<T>, IQueryable<T1>> selector = null,
        string includeProperties = "");

        IEnumerable<T> Get(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "");
    }
}