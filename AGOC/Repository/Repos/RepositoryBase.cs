using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using AGOC.Models;
using AGOC.Repository.Interfaces;

namespace AGOC.Repository.Repos
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        public VehicleMsContext _repositoryContext { get; set; }
        internal DbSet<T> DbSet { get; set; }

        public RepositoryBase(VehicleMsContext PortalDBContext)
        {
            _repositoryContext = PortalDBContext;
            DbSet = _repositoryContext.Set<T>();
        }

        /// <summary>
        /// Get All Async
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await DbSet.ToListAsync();
        }

        /// <summary>
        /// Get by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<T> GetByIdAsync(int id)
        {
            return await DbSet.FindAsync(id);
        }

        /// <summary>
        /// Add Async
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task AddAsync(T entity)
        {
            await DbSet.AddAsync(entity);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="entity"></param>
        public void Update(T entity)
        {
            DbSet.Attach(entity);
            _repositoryContext.Entry(entity).State = EntityState.Modified;
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(T entity)
        {
            DbSet.Remove(entity);
        }

        public  IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression)
        {
            return _repositoryContext.Set<T>().Where(expression).AsNoTracking();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, int pageIndex, int pageSize)
        {
            return _repositoryContext.Set<T>().Where(expression)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize).AsNoTracking();
        }

        public async Task<int> Count(Expression<Func<T, bool>> expression)
        {
            return await _repositoryContext.Set<T>().CountAsync(expression);
        }

        public bool Any(Expression<Func<T, bool>> expression)
        {
            return _repositoryContext.Set<T>().Any(expression);
        }

        public void Add(List<T> entities)
        {
            _repositoryContext.Set<T>().AddRangeAsync(entities);
        }

        public void Update(List<T> entities)
        {
            _repositoryContext.Set<T>().UpdateRange(entities);
        }

        public void Delete(List<T> entity)
        {
            _repositoryContext.Set<T>().RemoveRange(entity);
        }

        public virtual IQueryable<T1> Get<TResult, T1>(
          Expression<Func<T, bool>> filter = null,
          Expression<Func<T, TResult>> orderBy = null,
          Func<IQueryable<T>, IQueryable<T1>> selector = null,
          string includeProperties = "")
        {
            IQueryable<T> query = DbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            if (orderBy != null)
            {
                query = query.OrderBy(orderBy);
            }

            if (selector != null)
            {
                return selector(query);
            }
            else
            {
                return (IQueryable<T1>)query.ToList();
            }
        }

        public virtual IEnumerable<T> Get(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<T> query = DbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }
    }
}