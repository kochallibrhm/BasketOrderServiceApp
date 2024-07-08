namespace BasketOrderServiceApp.Common.Models;
public class ApplicationSettings {
    public RabbitMq RabbitMq { get; set; }

    public RedisSettings RedisSetting { get; set;}
}
public class RabbitMq {
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Host { get; set; }
}

public class RedisSettings {
    public string ConnectionString { get; set; }
    public string Configuration { get; set; }
}


