using FluentAssertions;
using GestionOrdinateurs.API.Data;
using GestionOrdinateurs.API.Services;
using GestionOrdinateurs.Shared;
using Microsoft.EntityFrameworkCore;

namespace GestionOrdinateurs.Tests;

public class OrdinateurServiceTests
{
    [Fact]
    public async Task AjouterOrdinateur()
    {
        using var contexte = CreerContexte();
        var service = new OrdinateurService(contexte);
        var ordinateur = new Ordinateur(0, "PC-Test", "Dell", 8, "Intel i5", EtatOrdinateur.EnService);

        var resultat = await service.AddAsync(ordinateur);

        resultat.Id.Should().BePositive();
        (await contexte.Ordinateurs.FindAsync(resultat.Id)).Should().NotBeNull();
    }

    [Fact]
    public async Task SupprimerOrdinateur()
    {
        using var contexte = CreerContexte();
        var service = new OrdinateurService(contexte);

        // L'ordinateur 1 fait partie des données initiales du contexte.
        var resultat = await service.DeleteAsync(1);

        resultat.Should().BeTrue();
        (await contexte.Ordinateurs.FindAsync(1)).Should().BeNull();
    }

    [Fact]
    public async Task SupprimerOrdinateurInexistant()
    {
        using var contexte = CreerContexte();
        var service = new OrdinateurService(contexte);

        var resultat = await service.DeleteAsync(999);

        resultat.Should().BeFalse();
    }

    private static ParcInformatiqueContext CreerContexte()
    {
        var options = new DbContextOptionsBuilder<ParcInformatiqueContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var contexte = new ParcInformatiqueContext(options);
        contexte.Database.EnsureCreated();
        return contexte;
    }
}
