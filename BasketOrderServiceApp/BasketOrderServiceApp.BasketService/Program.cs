using BasketOrderServiceApp.Common.Models;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.RegisterApplicationSettings();
var appSettings = builder.Services.BuildServiceProvider().GetService<ApplicationSettings>();

builder.Host.UseSerilog((ctx, lc) => lc
    .ReadFrom.Configuration(ctx.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console());

builder.Services.AddMassTransit(x => {
    x.AddConsumer<AddBasketConsumer>();
    x.AddConsumer<SendOrderConsumer>();
    x.AddConsumer<CleanOrdersFromCacheConsumer>();

    x.UsingRabbitMq((context, cfg) => {
        cfg.Host(appSettings.RabbitMq.Host, h => {
            h.Username(appSettings.RabbitMq.UserName);
            h.Password(appSettings.RabbitMq.Password);
        });

        cfg.ReceiveEndpoint("addbasket-queue", e => {
            e.ConfigureConsumer<AddBasketConsumer>(context);
        });

        cfg.ReceiveEndpoint("sendorderequest-queue", e => {
            e.ConfigureConsumer<SendOrderConsumer>(context);
        });

        cfg.ReceiveEndpoint("cleanordersfromcache-queue", e => {
            e.ConfigureConsumer<CleanOrdersFromCacheConsumer>(context);
        });
    });
});

builder.Services.AddScoped(typeof(IRedisCacheManager<>), typeof(RedisCacheManager<>));

builder.Services.AddSingleton(sp => {
    var configuration = appSettings.RedisSetting.Configuration;
    return ConnectionMultiplexer.Connect(configuration);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
