using BasketOrderServiceApp.Common.ConsumerMessages;
using MassTransit.Transports;

namespace BasketOrderServiceApp.APIGateway.MediatR.Handlers;
public class SendOrderHandler : IRequestHandler<SendOrderRequest, SendOrderResponse> {
    private readonly IPublishEndpoint publishEndpoint;

    public SendOrderHandler(IPublishEndpoint publishEndpoint)
    {
        this.publishEndpoint = publishEndpoint;
    }
    public async Task<SendOrderResponse> Handle(SendOrderRequest request, CancellationToken cancellationToken) {
        try {
            var consumerMessage = new SendOrderMessage {
                UserId = request.UserId
            };

            await publishEndpoint.Publish(consumerMessage);

            Log.Information("SendOrderHandler - SendOrderMessage published");
            return new SendOrderResponse() { IsSuccess = true, ErrorMessage = string.Empty };
        }
        catch (Exception ex) {
            Log.Error(ex, "SendOrderHandler - Error on publishing SendOrderMessage");
            return new SendOrderResponse() { IsSuccess = false, ErrorMessage = ex.Message };
        }
    }
}
