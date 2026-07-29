using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using X_Libra_Catering.Cliente;
using X_Libra_Catering.Cliente.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5137") });

builder.Services.AddScoped<ServicioCliente>();
builder.Services.AddScoped<ServicioEvento>();
builder.Services.AddScoped<ServicioMenu>();
builder.Services.AddScoped<ServicioVehiculo>();
builder.Services.AddScoped<ServicioPedido>();
builder.Services.AddScoped<ServicioDashboard>();
builder.Services.AddSingleton<ServicioNotificacion>();

await builder.Build().RunAsync();
