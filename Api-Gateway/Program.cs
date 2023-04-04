using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Eureka;

var builder = WebApplication.CreateBuilder(args);

//Add ocelot
builder.Configuration.AddJsonFile("Ocelot.json");

builder.Services.AddOcelot()
    .AddEureka();

var app = builder.Build();

app.UseOcelot().Wait();

app.MapGet("/", () => "Hello World!");


app.Run();
