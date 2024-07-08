namespace BasketOrderServiceApp.BasketService.Consumers;

public class SendOrderConsumer : IConsumer<SendOrderMessage> {
    private readonly IRedisCacheManager<OrderCacheModel> redisCacheManager;
    private readonly IPublishEndpoint publishEndpoint;

    public SendOrderConsumer(IRedisCacheManager<OrderCacheModel> redisCacheManager, IPublishEndpoint publishEndpoint) {
        this.redisCacheManager = redisCacheManager;
        this.publishEndpoint = publishEndpoint;
    }

    public async Task Consume(ConsumeContext<SendOrderMessage> context) {
        try {
            var message = context.Message;
            var orders = await redisCacheManager.GetByUserIdAsync(message.UserId);

            var orderMessages = orders.Select(order => new OrderMessage {
                Id = order.Id,
                UserId = order.UserId,
                ProductName = order.ProductName,
                Quantity = order.Quantity,
                Price = order.Price
            }).ToList();

            var consumerMessage = new InsertOrdersMessage {
                OrderMessages = orderMessages
            };

            await publishEndpoint.Publish(consumerMessage);

            Log.Information($"SendOrderConsumer - InsertOrdersMessage published with UserId: {message.UserId}");
        }
        catch (Exception ex) {
            Log.Error(ex, "SendOrderConsumer - Error on InsertOrdersMessage publish");
        }

    }
}
