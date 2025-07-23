using StreamingMovie.Domain.Interfaces;
using System.Linq.Expressions;

namespace StreamingMovie.Application.Services;

public class GenericService<T> : IGenericService<T> where T : class
{
    private readonly IGenericRepository<T> _repo;

    public GenericService(IGenericRepository<T> repo)
    {
        _repo = repo;
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
        => await _repo.GetAllAsync();

    public virtual async Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes)
        => await _repo.GetAllAsync(includes);

    public virtual async Task<T?> GetByIdAsync(int id)
        => await _repo.GetByIdAsync(id);

    public virtual async Task<T?> GetByIdAsync(Guid id)
        => await _repo.GetByIdAsync(id);

    public virtual async Task<IEnumerable<T>> FindAsync(params Expression<Func<T, bool>>[] predicates)
        => await _repo.FindAsync(predicates);

    public virtual async Task<T?> FindOneAsync(Expression<Func<T, bool>> predicate)
        => await _repo.FindOneAsync(predicate);

    #region CUD Operations
    public virtual async Task<T> AddAsync(T entity)
        => await _repo.AddAsync(entity);

    public virtual async Task<T> UpdateAsync(T entity)
        => await _repo.UpdateAsync(entity);

    public virtual async Task<bool> DeleteAsync(int id)
        => await _repo.DeleteAsync(id);

    public virtual async Task<bool> DeleteAsync(Guid id)
        => await _repo.DeleteAsync(id);

    public virtual async Task<bool> DeleteAsync(T entity)
        => await _repo.DeleleAsync(entity);
    #endregion
}
