using BasketOrderServiceApp.Common.ConsumerMessages;
using XAct.Messages;

namespace BasketOrderServiceApp.BasketService.Consumers;

public class AddBasketConsumer : IConsumer<AddBasketMessage> {
    private readonly IRedisCacheManager<OrderCacheModel> redisCacheManager;

    public AddBasketConsumer(IRedisCacheManager<OrderCacheModel> redisCacheManager) {
        this.redisCacheManager = redisCacheManager;
    }
      
    public async Task Consume(ConsumeContext<AddBasketMessage> context) {
        Log.Information("AddBasketConsumer - AddBasketMessage consumed");
        try {
             var message = context.Message;
            OrderCacheModel order = new() {
                Id = Guid.NewGuid().ToString(),
                UserId = message.UserId,
                ProductName = message.ProductName,
                Price = message.Price,
                Quantity = message.Quantity,
            };

            await redisCacheManager.InsertAsync(order);

            Log.Information($"AddBasketConsumer - Basket added to redis with KEY: {order.Id}");

        }
        catch (Exception ex) {
            Log.Error(ex, "AddBasketConsumer- Error on consuming AddBasketMessage");
        }
        
    }
}