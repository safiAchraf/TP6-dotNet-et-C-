namespace GestionOrdinateurs.Client.Services;

public class ExchangeRatesResponse
{
    public Dictionary<string, decimal> Rates { get; set; } = new();
}
