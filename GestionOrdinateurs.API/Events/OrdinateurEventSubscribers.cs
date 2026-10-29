using Microsoft.Extensions.Logging;

namespace GestionOrdinateurs.API.Events;

public class OrdinateurEventSubscribers
{
    private readonly ILogger<OrdinateurEventSubscribers> _logger;

    public OrdinateurEventSubscribers(ILogger<OrdinateurEventSubscribers> logger)
    {
        _logger = logger;
    }

    public void AfficherMessageAjout(object? sender, OrdinateurAjouteEventArgs e)
    {
        _logger.LogInformation("Événement : ordinateur ajouté : {Nom}", e.Ordinateur.Nom);
    }

    public void AfficherMessageSuppression(object? sender, OrdinateurSupprimeEventArgs e)
    {
        _logger.LogInformation("Événement : ordinateur supprimé : {Nom}", e.Ordinateur.Nom);
    }

    public void AfficherMessageMiseAJour(object? sender, OrdinateurMisAJourEventArgs e)
    {
        _logger.LogInformation("Événement : ordinateur mis à jour : {Nom}", e.Ordinateur.Nom);
    }
}
