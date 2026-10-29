namespace GestionOrdinateurs.Shared;

using System.Text.Json.Serialization;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TypeOrdinateur
{
    Portable,
    Fixe,
    Server
}
