namespace GestionOrdinateurs.Client.Utils;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;

public class JwtAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ISessionStorageService _sessionStorage;

    public JwtAuthenticationStateProvider(ISessionStorageService sessionStorage)
    {
        _sessionStorage = sessionStorage;
    }

    // Récupération de l'utilisateur à partir du token en session
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await _sessionStorage.GetItemAsync<string>("jwtToken");
        if (!string.IsNullOrWhiteSpace(token))
        {
            var handler = new JwtSecurityTokenHandler();
            // La signature est vérifiée par l'API ; ici on vérifie l'expiration pour l'affichage.
            if (handler.CanReadToken(token) && handler.ReadJwtToken(token).ValidTo > DateTime.UtcNow)
            {
                var identity = new ClaimsIdentity(ParseClaims(token), "jwt");
                return new AuthenticationState(new ClaimsPrincipal(identity));
            }

            await _sessionStorage.RemoveItemAsync("jwtToken");
        }

        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }

    public void NotifyUserAuthentication(string token)
    {
        var identity = new ClaimsIdentity(ParseClaims(token), "jwt");
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity))));
    }

    public void NotifyUserLogout()
    {
        var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymous)));
    }

    private static IEnumerable<Claim> ParseClaims(string jwt)
    {
        var token = new JwtSecurityTokenHandler().ReadJwtToken(jwt);
        var claims = new List<Claim>();
        foreach (var claim in token.Claims)
        {
            if (claim.Type == "role")
            {
                claims.Add(new Claim(ClaimTypes.Role, claim.Value));
            }
            else if (claim.Type == "unique_name")
            {
                claims.Add(new Claim(ClaimTypes.Name, claim.Value));
            }
            else
            {
                claims.Add(claim);
            }
        }
        return claims;
    }
}
