using MassTransit;
using ProductInventoryService.Consumer;
using ProductInventoryService.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<ProductConsumer>();
    x.AddConsumer<ProductListConsumer>();
    x.UsingRabbitMq((context, config) =>
    {
        config.Host(new Uri("rabbitmq://localhost"), hostConfig =>
        {
            hostConfig.Username("guest");
            hostConfig.Password("guest");
        });
        config.ReceiveEndpoint("product-list-request", ep =>
        {
            ep.PrefetchCount = 16;
            ep.UseMessageRetry(mr => mr.Interval(2, 100));
            ep.ConfigureConsumer<ProductListConsumer>(context);
        });
        config.ReceiveEndpoint("product-request", ep =>
        {
            ep.PrefetchCount = 16;
            ep.UseMessageRetry(mr => mr.Interval(2, 100));
            ep.ConfigureConsumer<ProductConsumer>(context);
        });
        
    });
});


// Add a custom scoped service
builder.Services.AddTransient<IProductInventoryService, ServiceProductInventory>();

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
