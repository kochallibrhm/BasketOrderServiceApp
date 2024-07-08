namespace BasketOrderServiceApp.APIGateway.MediatR.Responses; 
public class BaseResponse {
    public bool IsSuccess { get; set; }
    public string ErrorMessage { get; set; }
}
