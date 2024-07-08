using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketOrderServiceApp.Common.ConsumerMessages; 
public class InsertOrdersMessage {
    public List<OrderMessage> OrderMessages { get; set; }
}
public class OrderMessage {
    public string Id { get; set; }
    public long UserId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}