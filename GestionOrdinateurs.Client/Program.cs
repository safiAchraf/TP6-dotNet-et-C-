using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.SessionStorage;
using GestionOrdinateurs.Client;
using GestionOrdinateurs.Client.Utils;
using GestionOrdinateurs.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazoredSessionStorage();
builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<JwtAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<JwtAuthenticationStateProvider>());

// Handler pour ajouter automatiquement le token JWT dans les requêtes
builder.Services.AddScoped<JwtHandler>();

// Client HTTP principal vers la Gateway YARP (port 5010)
builder.Services.AddHttpClient("GatewayClient", client =>
{
    client.BaseAddress = new Uri("http://localhost:5010/");
})
.AddHttpMessageHandler<JwtHandler>();

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("GatewayClient"));

// Client HTTP pour l'API externe de taux de change
builder.Services.AddHttpClient("ExternalApiClient", client =>
{
    client.Timeout = TimeSpan.FromSeconds(10);
});
builder.Services.AddScoped<DeviseService>();

await builder.Build().RunAsync();
