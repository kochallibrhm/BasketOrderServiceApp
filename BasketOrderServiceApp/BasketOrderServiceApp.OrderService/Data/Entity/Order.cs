namespace BasketOrderServiceApp.OrderService.Data.Entity;

public class Order {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    public long UserId { get; set; }

    [Required]
    public string ProductName { get; set; }

    [Required]
    public int Quantity { get; set; }

    [Required]
    public decimal Price { get; set; }
}
