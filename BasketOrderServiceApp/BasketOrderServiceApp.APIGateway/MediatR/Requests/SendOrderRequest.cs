namespace BasketOrderServiceApp.APIGateway.MediatR.Requests; 
public class SendOrderRequest : BaseRequest, IRequest<SendOrderResponse> {
}

public class SendOrderRequestValidator : AbstractValidator<SendOrderRequest> {
    public SendOrderRequestValidator() {
        RuleFor(request => request.UserId)
            .GreaterThan(0).WithMessage("UserId must be greater than zero");
    }
}
