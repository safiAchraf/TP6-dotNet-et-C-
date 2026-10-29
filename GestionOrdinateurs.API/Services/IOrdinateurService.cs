using GestionOrdinateurs.API.Events;
using GestionOrdinateurs.Shared;

namespace GestionOrdinateurs.API.Services;

public interface IOrdinateurService
{
    event EventHandler<OrdinateurAjouteEventArgs>? OrdinateurAjoute;
    event EventHandler<OrdinateurSupprimeEventArgs>? OrdinateurSupprime;
    event EventHandler<OrdinateurMisAJourEventArgs>? OrdinateurMisAJour;

    Task<List<Ordinateur>> GetAllAsync();
    Task<Ordinateur?> GetByIdAsync(int id);
    Task<List<Ordinateur>> GetByMarqueAsync(string marque);
    Task<List<Ordinateur>> GetDisponiblesAsync();
    Task<List<Ordinateur>> GetDuPlusCherAuMoinsCherAsync();
    Task<Ordinateur> AddAsync(Ordinateur ordinateur);
    Task<Ordinateur?> UpdateAsync(int id, Ordinateur ordinateur);
    Task<bool> DeleteAsync(int id);

    string Diagnostiquer(Ordinateur ordinateur);
    (int TotalOrdinateur, int OrdiDisponibles, decimal ValeurStock, int NbReparations) CalculerStatistiques();
}
