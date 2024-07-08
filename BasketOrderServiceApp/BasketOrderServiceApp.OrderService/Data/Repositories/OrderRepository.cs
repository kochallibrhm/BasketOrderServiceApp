namespace BasketOrderServiceApp.OrderService.Data.Repositories; 
public class OrderRepository : BaseRepository<Order> , IOrderRepository {
    public OrderRepository(BasketOrderServiceAppContext context) : base(context) {
    }
}
