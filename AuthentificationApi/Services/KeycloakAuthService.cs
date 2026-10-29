using System.Net;
using AuthentificationApi.Models;

namespace AuthentificationApi.Services;

public class KeycloakAuthService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public KeycloakAuthService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<TokenResponse?> LoginAsync(string username, string password)
    {
        var tokenEndpoint = $"{_configuration["Keycloak:BaseUrl"]}/realms/{_configuration["Keycloak:Realm"]}/protocol/openid-connect/token";
        var requestBody = new Dictionary<string, string>
        {
            { "grant_type", "password" },
            { "client_id", _configuration["Keycloak:ClientId"]! },
            { "client_secret", _configuration["Keycloak:ClientSecret"]! },
            { "username", username },
            { "password", password }
        };

        using var content = new FormUrlEncodedContent(requestBody);
        using var response = await _httpClient.PostAsync(tokenEndpoint, content);
        if (response.StatusCode == HttpStatusCode.BadRequest || response.StatusCode == HttpStatusCode.Unauthorized)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TokenResponse>();
    }
}
