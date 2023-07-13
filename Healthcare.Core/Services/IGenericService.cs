using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Healthcare.Core.Services;

public interface IGenericService<T> where T : class
{
    Task<T> GetByIdAsync(int id);
    Task<T> GetByGuidIdAsync(Guid id);
    Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> expression, string[] includes = null);
    Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> expression, string[] includes = null);
    Task<IEnumerable<T>> GetAllAsync(int? pageNumber = null, int? pageSize = null);
    IQueryable<T> GetList(Expression<Func<T, bool>> expression, string[] includes = null, bool asNoTracking = true, params Expression<Func<T, object>>[] includeExpressions);
    Task<bool> AnyAsync(Expression<Func<T, bool>> expression);
    Task<T> AddAsync(T entity);
    Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);
    Task UpdateAsync(T entity);
    Task RemoveAsync(T entity);
    Task RemoveRangeAsync(IEnumerable<T> entities);
}
