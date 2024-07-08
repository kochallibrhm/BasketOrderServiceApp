using StackExchange.Redis;
using System.Linq.Expressions;

namespace BasketOrderServiceApp.Common.Cache;
public class RedisCacheManager<T> : IRedisCacheManager<T> where T : notnull {
    private readonly RedisConnectionProvider _provider;
    private readonly IRedisCollection<T> _collection;
    private readonly IDatabase _database;
    private readonly ConnectionMultiplexer _connectionMultiplexer;

    public RedisCacheManager(ApplicationSettings applicationSettings, ConnectionMultiplexer connectionMultiplexer) {
        _provider = new RedisConnectionProvider(applicationSettings.RedisSetting.ConnectionString);
        _collection = _provider.RedisCollection<T>();
        _connectionMultiplexer = connectionMultiplexer;
        _database = _connectionMultiplexer.GetDatabase();

        CreateIndexIfNotExists();

    }

    private void CreateIndexIfNotExists() {
        var result = _database.Execute("FT._LIST");

        RedisValue[] indexes = (RedisValue[])result;

        var indexList = indexes.Select(index => index.ToString().ToLower()).ToList();

        // Küçük harfe çevrilmiş indeks adı ile kontrol yap
        if (!indexes.Contains("ordercachemodel-idx")) {
            _database.Execute("FT.CREATE", "ordercachemodel-idx",
                              "ON", "HASH",
                              "PREFIX", "1", "BasketOrderServiceApp.Common.Models.OrderCacheModel:",
                              "SCHEMA", "UserId", "NUMERIC", "SORTABLE");
        }
    }

    public async Task InsertAsync(T item) {
        await _collection.InsertAsync(item);
    }

    public async Task<T> GetByIdAsync(string id) {
        return await _collection.FindByIdAsync(id);
    }

    public async Task UpdateAsync(T item) {
        await _collection.UpdateAsync(item);
    }

    public async Task DeleteAsync(string id) {
        var item = await _collection.FindByIdAsync(id);
        if (item != null) {
            await _collection.DeleteAsync(item);
        }
    }

    public async Task<IEnumerable<T>> GetByUserIdAsync(long userId) {
        var parameter = Expression.Parameter(typeof(T), "x");
        var property = Expression.Property(parameter, "UserId");
        var constant = Expression.Constant(userId);
        var equality = Expression.Equal(property, constant);
        var lambda = Expression.Lambda<Func<T, bool>>(equality, parameter);

        return await _collection.Where(lambda).ToListAsync();
    }
}
