using MassTransit;
using PriceService.Consumer;
using PriceService.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<PriceRequestConsumer>();
    x.AddConsumer<PriceAddConsumer>();
    x.AddConsumer<PriceRemoveConsumer>();
    x.UsingRabbitMq((context, config) =>
    {
        config.Host(new Uri("rabbitmq://localhost"), hostConfig =>
        {
            hostConfig.Username("guest");
            hostConfig.Password("guest");
        });
        config.ReceiveEndpoint("price-requests", ep =>
        {
            //ep.PrefetchCount = 16;
            //ep.UseMessageRetry(mr => mr.Interval(2, 100));
            ep.ConfigureConsumer<PriceRequestConsumer>(context);
        });
        config.ReceiveEndpoint("add-price", ep =>
        {
            //ep.PrefetchCount = 16;
            //ep.UseMessageRetry(mr => mr.Interval(2, 100));
            ep.ConfigureConsumer<PriceAddConsumer>(context);
        });
        config.ReceiveEndpoint("remove-price", ep =>
        {
            //ep.PrefetchCount = 16;
            //ep.UseMessageRetry(mr => mr.Interval(2, 100));
            ep.ConfigureConsumer<PriceRemoveConsumer>(context);
        });
    });
});

// Add a custom scoped service
builder.Services.AddTransient<IPriceService, ServicePrice>();

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
