namespace GestionOrdinateurs.Client.Services;

using System.Net.Http.Json;

public class DeviseService
{
    private readonly HttpClient _http;
    public decimal? TauxDollar { get; private set; }
    public string MessageErreur { get; private set; } = string.Empty;

    public DeviseService(IHttpClientFactory httpClientFactory)
    {
        _http = httpClientFactory.CreateClient("ExternalApiClient");
    }

    // Consommation de l'API publique ; les prix du tableau restent en euros.
    public async Task ChargerTauxAsync()
    {
        TauxDollar = null;
        MessageErreur = string.Empty;
        try
        {
            var resultat = await _http.GetFromJsonAsync<ExchangeRatesResponse>("https://open.er-api.com/v6/latest/EUR");
            if (resultat != null && resultat.Rates.TryGetValue("USD", out var taux) && taux > 0)
            {
                TauxDollar = taux;
            }
            else
            {
                MessageErreur = "Taux de change indisponible.";
            }
        }
        catch (HttpRequestException)
        {
            MessageErreur = "L'API de change est indisponible.";
        }
        catch (TaskCanceledException)
        {
            MessageErreur = "L'API de change ne répond pas.";
        }
        catch (System.Text.Json.JsonException)
        {
            MessageErreur = "Réponse de l'API de change incorrecte.";
        }
    }
}
