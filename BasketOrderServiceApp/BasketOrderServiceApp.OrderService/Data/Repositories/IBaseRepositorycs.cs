namespace BasketOrderServiceApp.OrderService.Data.Repositories;

public interface IBaseRepository<T> where T : class {
    Task AddAsync(T entity);
    Task AddRangeAsync(IEnumerable<T> entities);
}