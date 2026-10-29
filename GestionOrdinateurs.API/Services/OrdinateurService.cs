using GestionOrdinateurs.API.Data;
using GestionOrdinateurs.API.Events;
using GestionOrdinateurs.Shared;
using Microsoft.EntityFrameworkCore;

namespace GestionOrdinateurs.API.Services;

public class OrdinateurService : IOrdinateurService
{
    private readonly ParcInformatiqueContext _context;
    private readonly JournalService? _journal;

    public OrdinateurService(ParcInformatiqueContext context, JournalService? journal = null)
    {
        _context = context;
        _journal = journal;
    }

    public event EventHandler<OrdinateurAjouteEventArgs>? OrdinateurAjoute;
    public event EventHandler<OrdinateurSupprimeEventArgs>? OrdinateurSupprime;
    public event EventHandler<OrdinateurMisAJourEventArgs>? OrdinateurMisAJour;

    public async Task<List<Ordinateur>> GetAllAsync()
    {
        return await _context.Ordinateurs.ToListAsync();
    }

    public async Task<Ordinateur?> GetByIdAsync(int id)
    {
        return await _context.Ordinateurs.FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<List<Ordinateur>> GetByMarqueAsync(string marque)
    {
        return await _context.Ordinateurs
            .Where(o => o.Caracteristiques.Marque.ToLower() == marque.ToLower())
            .ToListAsync();
    }

    public async Task<List<Ordinateur>> GetDisponiblesAsync()
    {
        return await _context.Ordinateurs
            .Where(o => o.Stock > 0)
            .ToListAsync();
    }

    public async Task<List<Ordinateur>> GetDuPlusCherAuMoinsCherAsync()
    {
        return await _context.Ordinateurs
            .OrderByDescending(o => o.Prix)
            .ToListAsync();
    }

    public async Task<Ordinateur> AddAsync(Ordinateur ordinateur)
    {
        _context.Ordinateurs.Add(ordinateur);
        await _context.SaveChangesAsync();

        _journal?.EcrireOrdinateur($"Ordinateur {ordinateur.Nom} ajouté");

        OrdinateurAjoute?.Invoke(this, new OrdinateurAjouteEventArgs
        {
            Ordinateur = ordinateur
        });

        return ordinateur;
    }

    public async Task<Ordinateur?> UpdateAsync(int id, Ordinateur ordinateur)
    {
        var existant = await _context.Ordinateurs.FindAsync(id);
        if (existant == null)
        {
            return null;
        }

        existant.Nom = ordinateur.Nom;
        existant.Caracteristiques = ordinateur.Caracteristiques;
        existant.Etat = ordinateur.Etat;
        existant.Stock = ordinateur.Stock;
        existant.Prix = ordinateur.Prix;
        existant.Vendu = ordinateur.Vendu;

        await _context.SaveChangesAsync();
        _journal?.EcrireOrdinateur($"Ordinateur {existant.Nom} modifié");

        OrdinateurMisAJour?.Invoke(this, new OrdinateurMisAJourEventArgs
        {
            Ordinateur = existant
        });

        return existant;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var ordinateur = await _context.Ordinateurs.FindAsync(id);
        if (ordinateur == null)
        {
            return false;
        }

        _context.Ordinateurs.Remove(ordinateur);
        await _context.SaveChangesAsync();
        _journal?.EcrireOrdinateur($"Ordinateur {ordinateur.Nom} supprimé");

        OrdinateurSupprime?.Invoke(this, new OrdinateurSupprimeEventArgs
        {
            Ordinateur = ordinateur
        });

        return true;
    }

    // Diagnostic avec pattern matching
    public string Diagnostiquer(Ordinateur ordinateur)
    {
        return ordinateur switch
        {
            { Etat: EtatOrdinateur.HorsService }
                => "Remplacement",

            {
                Etat: EtatOrdinateur.Lent,
                Caracteristiques: { RamGo: < 8 }
            }
                => "Ajouter RAM",

            _
                => "Aucune action"
        };
    }

    public (int TotalOrdinateur, int OrdiDisponibles, decimal ValeurStock, int NbReparations) CalculerStatistiques()
    {
        var ordinateurs = _context.Ordinateurs.ToList();
        return (
            TotalOrdinateur: ordinateurs.Count,
            OrdiDisponibles: ordinateurs.Count(o => o.Stock > 0),
            ValeurStock: ordinateurs.Sum(o => o.Stock * o.Prix),
            NbReparations: _context.Reparations.Count()
        );
    }
}
