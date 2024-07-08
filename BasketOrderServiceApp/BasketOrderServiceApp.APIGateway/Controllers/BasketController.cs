namespace BasketOrderServiceApp.APIGateway.Controllers;

public class BasketController : BaseController
{
    [HttpPost("addBasket")]
    public async Task<IActionResult> AddBasket([FromBody] AddBasketRequest request)
    {
        var response = await Mediator.Send(request);
        return Ok(response);
    }

    // Sipariş göndermek için endpoint
    [HttpPost("sendOrder")]
    public async Task<IActionResult> SendOrder([FromBody] SendOrderRequest request)
    {
        var response = await Mediator.Send(request);
        return Ok(response);
    }
}
