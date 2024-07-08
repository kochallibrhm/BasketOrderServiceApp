using BasketOrderServiceApp.Common.ConsumerMessages;
using BasketOrderServiceApp.OrderService.Data.Repositories;
using MassTransit;
using XAct.Messages;

namespace BasketOrderServiceApp.OrderService.Consumers; 
public class InsertOrdersConsumer : IConsumer<InsertOrdersMessage> {
    private readonly IOrderRepository orderRepository;
    private readonly IPublishEndpoint publishEndpoint;

    public InsertOrdersConsumer(IOrderRepository orderRepository, IPublishEndpoint publishEndpoint) {
        this.orderRepository = orderRepository;
        this.publishEndpoint = publishEndpoint;
    }

    public async Task Consume(ConsumeContext<InsertOrdersMessage> context) {
        try {
            var message = context.Message;

            Log.Information("OrderService InsertOrdersConsumer - InsertOrdersMessage consumed");

            var orders = message.OrderMessages.Select(orderMessage => new Order {
                UserId = orderMessage.UserId,
                ProductName = orderMessage.ProductName,
                Quantity = orderMessage.Quantity,
                Price = orderMessage.Price
            }).ToList();

            await orderRepository.AddRangeAsync(orders);

            Log.Information("InsertOrdersConsumer - Orders inserted");

            CleanRedisMessage cleanRedisMessage = new() {
                IsSucces = true,
                OrderIdListToFlush = message.OrderMessages.Select(x => x.Id).ToList()
            };

            await publishEndpoint.Publish(cleanRedisMessage);
        }
        catch (Exception ex) {
            Log.Error(ex, "OrderService InsertOrdersConsumer - Error on inserting orders to db");
            CleanRedisMessage cleanRedisMessage = new() {
                IsSucces = false,
                OrderIdListToFlush = new List<string>()
            };

            await publishEndpoint.Publish(cleanRedisMessage);
        }
    }
}