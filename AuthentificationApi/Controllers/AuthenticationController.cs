namespace AuthentificationApi.Controllers;

using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Json;
using AuthentificationApi.Models;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using GestionOrdinateurs.Shared;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;

    public AuthenticationController(IConfiguration configuration, IHttpClientFactory httpClientFactory)
    {
        _configuration = configuration;
        _httpClientFactory = httpClientFactory;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Login))
        {
            return BadRequest("Le login est requis.");
        }

        var login = request.Login.Trim();
        User? user;
        try
        {
            var client = _httpClientFactory.CreateClient("GestionOrdinateursAPI");
            using var response = await client.GetAsync($"api/users/{Uri.EscapeDataString(login)}");
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound($"L'utilisateur '{login}' n'existe pas.");
            }

            response.EnsureSuccessStatusCode();
            user = await response.Content.ReadFromJsonAsync<User>();
        }
        catch (HttpRequestException)
        {
            return StatusCode(503, "L'API de gestion des utilisateurs est indisponible.");
        }
        catch (TaskCanceledException)
        {
            return StatusCode(503, "L'API de gestion des utilisateurs ne répond pas.");
        }

        if (user == null)
        {
            return NotFound($"L'utilisateur '{login}' n'existe pas.");
        }

        // Le rôle enregistré en base détermine les droits du compte.
        if (request.Role.HasValue && request.Role.Value != user.Role)
        {
            return BadRequest("Le rôle ne correspond pas à cet utilisateur.");
        }

        var jwtKey = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("La clé Jwt:Key est manquante.");
        var key = Encoding.ASCII.GetBytes(jwtKey);
        var token = GenTokenJWT(key, user.Role, user.Login);

        return Ok(new { token });
    }

    private static string GenTokenJWT(byte[] keyBytes, RoleEnum role, string userId)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, userId),
            new Claim(ClaimTypes.Role, role.ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(15),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(keyBytes),
                SecurityAlgorithms.HmacSha256Signature)
        };

        return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
    }
}
