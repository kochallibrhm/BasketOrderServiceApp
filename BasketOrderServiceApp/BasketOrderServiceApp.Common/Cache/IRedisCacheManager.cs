namespace BasketOrderServiceApp.Common.Cache;
public interface IRedisCacheManager<T> where T : notnull {
    Task InsertAsync(T item);
    Task<T> GetByIdAsync(string id);
    Task UpdateAsync(T item);
    Task DeleteAsync(string id);

    Task<IEnumerable<T>> GetByUserIdAsync(long userId);
}
