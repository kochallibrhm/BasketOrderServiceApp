namespace BasketOrderServiceApp.Common.ConsumerMessages;
public class CleanRedisMessage {
    public bool IsSucces { get; set; }

    public List<string> OrderIdListToFlush { get; set; }
}
