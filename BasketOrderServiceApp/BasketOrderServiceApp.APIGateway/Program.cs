var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options => {
    options.AddPolicy(name: "MyAllowSpecificOrigins",
                      builder => {
                          builder.WithOrigins("http://localhost:4200")
                                 .AllowAnyHeader()
                                 .AllowAnyMethod();
                      });
});

//builder.Services.AddCors(options => {
//    options.AddPolicy(name: "MyAllowSpecificOrigins",
//                      builder => {
//                          builder.AllowAnyOrigin()
//                                 .AllowAnyHeader()
//                                 .AllowAnyMethod();
//                      });
//});

builder.Host.UseSerilog((ctx, lc) => lc
    .ReadFrom.Configuration(ctx.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console());

builder.Services.AddControllers()
    .AddJsonOptions(options => {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services.AddMediatR(cfg =>
               cfg.RegisterServicesFromAssembly(typeof(SendOrderRequest).Assembly));

builder.Services.AddFluentValidation(fv =>
    fv.RegisterValidatorsFromAssemblyContaining<AddBasketRequestValidator>());

builder.Services.RegisterApplicationSettings();

var appSettings = builder.Services.BuildServiceProvider().GetService<ApplicationSettings>();

builder.Services.AddMassTransit(x => {

    x.UsingRabbitMq((context, cfg) => {
        cfg.Host(appSettings.RabbitMq.Host, h => {
            h.Username(appSettings.RabbitMq.UserName);
            h.Password(appSettings.RabbitMq.Password);
        });
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("LocalhostSpecificOrigins");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

