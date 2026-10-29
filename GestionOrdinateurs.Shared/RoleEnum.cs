namespace GestionOrdinateurs.Shared;

using System.Text.Json.Serialization;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum RoleEnum
{
    Gestionnaire,
    Admin
}
