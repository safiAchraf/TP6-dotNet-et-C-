namespace GestionOrdinateurs.API.Middleware;

public class RequestLoggerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggerMiddleware> _logger;

    public RequestLoggerMiddleware(RequestDelegate next, ILogger<RequestLoggerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        await _next(context);
        var user = context.User.Identity?.Name ?? "Anonyme";
        _logger.LogInformation("{Utilisateur} : {Methode} {Chemin} - {Statut}",
            user, context.Request.Method, context.Request.Path, context.Response.StatusCode);
    }
}
