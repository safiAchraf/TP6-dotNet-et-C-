namespace GestionOrdinateurs.API.Services;

using Microsoft.EntityFrameworkCore;
using GestionOrdinateurs.API.Data;
using GestionOrdinateurs.Shared;

public class ReparationService
{
    private readonly ParcInformatiqueContext _context;
    private readonly JournalService _journal;
    private readonly object _lockObject = new();

    public ReparationService(ParcInformatiqueContext context, JournalService journal)
    {
        _context = context;
        _journal = journal;
    }

    public record HistoriqueReparationRecord(
        int OrdinateurId,
        string NomOrdinateur,
        string Technicien,
        string TypeReparation,
        DateTime DateIntervention
    );

    public async Task SimulerReparationAsync(Ordinateur ordinateur)
    {
        await Task.Delay(300);

        lock (_lockObject)
        {
            var reparation = new Reparation
            {
                OrdinateurId = ordinateur.Id,
                TypeReparation = "Réparation générale",
                Technicien = "Technicien de garde",
                DateDebut = DateTime.Now,
                Statut = "Terminée"
            };

            _context.Reparations.Add(reparation);
            _context.SaveChanges();
        }

        Console.WriteLine($"{ordinateur.Nom} réparé");
        _journal.Ecrire($"{ordinateur.Nom} réparé");
    }

    public async Task<List<Ordinateur>> GetOrdinateursPourSimulationAsync(int count = 5)
    {
        return await _context.Ordinateurs
            .Take(count)
            .ToListAsync();
    }

    public async Task<List<Reparation>> GetAllAsync()
    {
        return await _context.Reparations
            .Include(r => r.UnOrdinateur)
            .OrderByDescending(r => r.DateDebut)
            .ToListAsync();
    }

    public async Task<List<HistoriqueReparationRecord>> GetHistoriqueAsync()
    {
        return await _context.Reparations
            .OrderByDescending(r => r.DateDebut)
            .Select(r => new HistoriqueReparationRecord(
                r.OrdinateurId,
                r.UnOrdinateur.Nom,
                r.Technicien,
                r.TypeReparation,
                r.DateDebut))
            .ToListAsync();
    }
}
