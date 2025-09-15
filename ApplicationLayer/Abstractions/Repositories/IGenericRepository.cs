using System.Linq.Expressions;

namespace ApplicationLayer.Abstractions.Repositories;

public interface IGenericRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetAllAsync();

    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);

    // valfri extra:
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
}
