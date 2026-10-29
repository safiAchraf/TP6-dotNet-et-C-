using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using GestionOrdinateurs.API.Data;
using GestionOrdinateurs.API.Events;
using GestionOrdinateurs.API.Middleware;
using GestionOrdinateurs.API.Services;
using GestionOrdinateurs.Shared;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient", policy =>
    {
        policy.WithOrigins("http://localhost:5165")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "GestionOrdinateurs",
        Version = "v1"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Bearer {token}"
    });

    c.AddSecurityRequirement(_ => new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecuritySchemeReference("Bearer"),
            new List<string>()
        }
    });
});

var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("La clé Jwt:Key est manquante.");
var key = Encoding.ASCII.GetBytes(jwtKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
    options.Events = new JwtBearerEvents
    {
        OnTokenValidated = context =>
        {
            Console.WriteLine($"Token valide: {context.Principal?.Identity?.Name ?? "Anonymous"}");
            return Task.CompletedTask;
        },
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine("Token invalide");
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();

builder.Services.AddDbContext<ParcInformatiqueContext>(options =>
    options.UseSqlite("Data Source=parcinfo.db"));

builder.Services.AddScoped<OrdinateurEventSubscribers>();
builder.Services.AddScoped<IOrdinateurService>(sp =>
{
    var service = new OrdinateurService(
        sp.GetRequiredService<ParcInformatiqueContext>(),
        sp.GetRequiredService<JournalService>());
    var abonnes = sp.GetRequiredService<OrdinateurEventSubscribers>();
    service.OrdinateurAjoute += abonnes.AfficherMessageAjout;
    service.OrdinateurSupprime += abonnes.AfficherMessageSuppression;
    service.OrdinateurMisAJour += abonnes.AfficherMessageMiseAJour;
    return service;
});
builder.Services.AddScoped<ReparationService>();
builder.Services.AddSingleton<JournalService>();
builder.Services.AddSingleton<StockService>();

var app = builder.Build();

// Initialisation des utilisateurs par défaut au démarrage
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ParcInformatiqueContext>();
    db.Database.Migrate();

    if (!db.Users.Any())
    {
        db.Users.AddRange(
            new User { Login = "user1", Role = RoleEnum.Admin },
            new User { Login = "user2", Role = RoleEnum.Gestionnaire },
            new User { Login = "user3", Role = RoleEnum.Gestionnaire }
        );
        db.SaveChanges();
    }
}

app.UseCors("AllowBlazorClient");

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
// Le middleware peut maintenant lire le nom contenu dans le JWT.
app.UseMiddleware<RequestLoggerMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();
