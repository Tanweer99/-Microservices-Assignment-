using Common;
using MassTransit;
using ProductService.Service;
using Steeltoe.Discovery.Client;
using Steeltoe.Discovery.Eureka;

var builder = WebApplication.CreateBuilder(args);

// Discovery
builder.Services.AddDiscoveryClient();
builder.Services.AddHealthChecks();
builder.Services.AddSingleton<IHealthCheckHandler, ScopedEurekaHealthCheckHandler>();

// Add services to the container.
builder.Services.AddMassTransit(x =>
{
    x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(config =>
    {
        config.Host(new Uri("rabbitmq://localhost"), hostConfig =>
        {
            hostConfig.Username("guest");
            hostConfig.Password("guest");
        });
    }));
    x.AddRequestClient<ProductListRequest>();
    x.AddRequestClient<ProductRequest>();
    x.AddRequestClient<PriceRequest>();
    x.AddRequestClient<ProductDetailRequest>();
});

// Add a custom scoped service
builder.Services.AddTransient<IServiceProduct, ServiceProduct>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//discovery
app.UseHealthChecks("/info");

app.Run();
