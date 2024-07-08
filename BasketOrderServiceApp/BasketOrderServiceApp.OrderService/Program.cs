var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.RegisterApplicationSettings();
var appSettings = builder.Services.BuildServiceProvider().GetService<ApplicationSettings>();
builder.Services.AddDbContext<BasketOrderServiceAppContext>(options =>
    options.UseSqlServer(
builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton<IHashService>(new HashService(builder.Configuration.GetSection("HashSalt").Get<string>()));

builder.Services.AddScoped<IOrderRepository, OrderRepository>();

builder.Host.UseSerilog((ctx, lc) => lc
    .ReadFrom.Configuration(ctx.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console());

builder.Services.AddMassTransit(x => {
    x.AddConsumer<InsertOrdersConsumer>();

    x.UsingRabbitMq((context, cfg) => {
        cfg.Host(appSettings.RabbitMq.Host, h => {
            h.Username(appSettings.RabbitMq.UserName);
            h.Password(appSettings.RabbitMq.Password);
        });

        cfg.ReceiveEndpoint("insertorders-queue", e => {
            e.ConfigureConsumer<InsertOrdersConsumer>(context);
        });
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

CreateDbIfNotExists(app);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

async void CreateDbIfNotExists(WebApplication app) {
    using (var scope = app.Services.CreateScope()) {
        var services = scope.ServiceProvider;
        try {
            var context = services.GetRequiredService<BasketOrderServiceAppContext>();
            var hashService = services.GetRequiredService<IHashService>();
            await context.Initialize(hashService);
        }
        catch (Exception ex) {
            Log.Error(ex, "OrderService - Program.cs An error occurred creating the DB.");
        }
    }
}