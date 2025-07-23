using System.Linq.Expressions;

namespace StreamingMovie.Domain.Interfaces;

public interface IGenericRepository<T> where T : class
{
    // All
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes);
    // By Id
    Task<T?> GetByIdAsync(int id);
    Task<T?> GetByIdAsync(Guid id);
    // Predicate 
    Task<IEnumerable<T>> FindAsync(params Expression<Func<T, bool>>[] predicates);
    Task<T> FindOneAsync(Expression<Func<T, bool>> predicate);
    // CUD
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task<bool> DeleteAsync(int id);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> DeleleAsync(T entity);
    //Qeury
    IQueryable<T> Query(params Expression<Func<T, object>>[] includes);
    IQueryable<T> Find(params Expression<Func<T, bool>>[] predicates);
}