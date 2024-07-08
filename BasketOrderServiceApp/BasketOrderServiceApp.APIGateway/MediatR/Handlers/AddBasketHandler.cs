using BasketOrderServiceApp.Common.ConsumerMessages;

namespace BasketOrderServiceApp.APIGateway.MediatR.Handlers;
public class AddBasketHandler : IRequestHandler<AddBasketRequest, AddBasketResponse> {

    private readonly IPublishEndpoint publishEndpoint;

    public AddBasketHandler(IPublishEndpoint publishEndpoint) {
        this.publishEndpoint = publishEndpoint;
    }
    public async Task<AddBasketResponse> Handle(AddBasketRequest request, CancellationToken cancellationToken) {
        try {
            var consumerMessage = new AddBasketMessage {
                UserId = request.UserId,
                Price = request.Price,
                ProductName = request.ProductName,
                Quantity = request.Quantity,
            };

            await publishEndpoint.Publish(consumerMessage);

            Log.Information("AddBasketHandler - AddBasketMessage published");

            return new AddBasketResponse() { IsSuccess = true, ErrorMessage = string.Empty };
        }
        catch (Exception ex) {
            Log.Error(ex, "AddBasketHandler - Error on publishing AddBasketMessage");
            return new AddBasketResponse() { IsSuccess = false, ErrorMessage = ex.Message };
        }
    }
}
