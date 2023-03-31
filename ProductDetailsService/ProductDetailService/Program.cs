using Common;
using MassTransit;
using ProductDetailService.Consumer;
using ProductDetailService.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMassTransit(x =>
{
    x.AddRequestClient<PriceRequest>();

    x.AddConsumer<ProductDetailConsumer>();

    x.UsingRabbitMq((context, config) =>
{
        config.Host(new Uri("rabbitmq://localhost"), hostConfig =>
        {
            hostConfig.Username("guest");
            hostConfig.Password("guest");
        });
        config.ReceiveEndpoint("product-detail-request", ep =>
        {
            ep.PrefetchCount = 16;
            ep.UseMessageRetry(mr => mr.Interval(2, 100));
            ep.ConfigureConsumer<ProductDetailConsumer>(context);
        });
    });
});

// Add a custom scoped service
builder.Services.AddTransient<IProductDetailsService, ProductDetailsService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
