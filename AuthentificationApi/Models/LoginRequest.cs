using GestionOrdinateurs.Shared;

namespace AuthentificationApi.Models;

public class LoginRequest
{
    public string Login { get; set; } = string.Empty;
    public RoleEnum? Role { get; set; }
}
