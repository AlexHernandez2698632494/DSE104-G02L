using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var buider = WebApplication.CreateBuilder(args);

buider.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange:true);

buider.Services.AddOcelot();

var app  = buider.Build();

app.UseOcelot().Wait();

app.Run();