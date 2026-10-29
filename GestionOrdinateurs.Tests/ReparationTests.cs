using FluentAssertions;
using GestionOrdinateurs.Shared;

namespace GestionOrdinateurs.Tests;

public class ReparationTests
{
    [Fact]
    public void CreerReparation()
    {
        var reparation = new Reparation
        {
            OrdinateurId = 1,
            TypeReparation = "Remplacement du SSD",
            Technicien = "Alice",
            DateDebut = new DateTime(2026, 10, 15),
            Statut = "En cours"
        };

        reparation.OrdinateurId.Should().Be(1);
        reparation.TypeReparation.Should().Be("Remplacement du SSD");
        reparation.Technicien.Should().Be("Alice");
        reparation.Statut.Should().Be("En cours");
    }
}
