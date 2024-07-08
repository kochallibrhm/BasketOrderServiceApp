namespace BasketOrderServiceApp.OrderService.Data.Repositories;

public abstract class BaseRepository<T> : IBaseRepository<T> where T : class {
    protected readonly BasketOrderServiceAppContext _context;

    protected BaseRepository(BasketOrderServiceAppContext context) {
        _context = context;
    }

    public async Task AddAsync(T entity) {
        await _context.Set<T>().AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task AddRangeAsync(IEnumerable<T> entities) {
        await _context.Set<T>().AddRangeAsync(entities);
        await _context.SaveChangesAsync();
    }
}