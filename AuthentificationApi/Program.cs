using AuthentificationApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient", policy =>
    {
        policy.WithOrigins("http://localhost:5165", "http://localhost:5000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
builder.Services.AddHttpClient<KeycloakAuthService>();
builder.Services.AddHttpClient("GestionOrdinateursAPI", client =>
{
    client.BaseAddress = new Uri("http://gestionordinateurs-api:5247/");
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("AllowBlazorClient");
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.Run();
