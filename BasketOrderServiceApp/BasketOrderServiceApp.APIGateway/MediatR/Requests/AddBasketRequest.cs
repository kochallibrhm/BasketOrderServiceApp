namespace BasketOrderServiceApp.APIGateway.MediatR.Requests;

public class AddBasketRequest : BaseRequest, IRequest<AddBasketResponse> {
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}

public class AddBasketRequestValidator : AbstractValidator<AddBasketRequest> {
    public AddBasketRequestValidator() {
        RuleFor(request => request.UserId)
            .GreaterThan(0).WithMessage("UserId must be greater than zero");

        RuleFor(request => request.ProductName)
            .NotEmpty().WithMessage("Product name must not be empty");

        RuleFor(request => request.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than zero");

        RuleFor(request => request.Price)
            .GreaterThan(0).WithMessage("Price must be greater than zero");
    }
}
