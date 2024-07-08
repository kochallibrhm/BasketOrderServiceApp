namespace BasketOrderServiceApp.Common.ConsumerMessages;
public class AddBasketMessage {
    public long UserId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}
