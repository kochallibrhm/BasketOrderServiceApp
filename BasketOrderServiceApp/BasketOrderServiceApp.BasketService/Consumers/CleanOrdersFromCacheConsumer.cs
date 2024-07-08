namespace BasketOrderServiceApp.BasketService.Consumers; 
public class CleanOrdersFromCacheConsumer : IConsumer<CleanRedisMessage> {

    private readonly IRedisCacheManager<OrderCacheModel> redisCacheManager;

    public CleanOrdersFromCacheConsumer(IRedisCacheManager<OrderCacheModel> redisCacheManager) {
        this.redisCacheManager = redisCacheManager;
    }

    public async Task Consume(ConsumeContext<CleanRedisMessage> context) {
        var message = context.Message;

        try {
            if (message.IsSucces) {
                foreach (var id in message.OrderIdListToFlush) {
                    await redisCacheManager.DeleteAsync(id);
                }
            }
            else {
                Log.Warning("CleanOrdersFromCacheConsumer - Could not clean cache");
            }
        }
        catch (Exception ex) {
            Log.Error(ex, "CleanOrdersFromCacheConsumer - Error on cleaning cache");
        }
    }
}
