using GestionOrdinateurs.Shared;
using Microsoft.EntityFrameworkCore;

namespace GestionOrdinateurs.API.Data;

public class ParcInformatiqueContext : DbContext
{
    public ParcInformatiqueContext(DbContextOptions<ParcInformatiqueContext> options)
        : base(options)
    {
    }

    public DbSet<Ordinateur> Ordinateurs { get; set; } = null!;
    public DbSet<Reparation> Reparations { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Ordinateur>()
            .OwnsOne(o => o.Caracteristiques);

        modelBuilder.Entity<Ordinateur>()
            .HasMany(o => o.Reparations)
            .WithOne(r => r.UnOrdinateur);

        // Login unique en base de données
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Login)
            .IsUnique();

        modelBuilder.Entity<Ordinateur>().HasData(
            new { Id = 1, Nom = "PC-SAFIA-01", Etat = EtatOrdinateur.EnService, Stock = 3, Prix = 899.99m, Vendu = false },
            new { Id = 2, Nom = "PC-COMPTA-01", Etat = EtatOrdinateur.EnService, Stock = 2, Prix = 599.99m, Vendu = false },
            new { Id = 3, Nom = "PC-DIRECTION-01", Etat = EtatOrdinateur.EnService, Stock = 1, Prix = 1199.99m, Vendu = false },
            new { Id = 4, Nom = "PC-DESIGN-01", Etat = EtatOrdinateur.EnMaintenance, Stock = 0, Prix = 1499.99m, Vendu = false },
            new { Id = 5, Nom = "PC-MOBILE-01", Etat = EtatOrdinateur.HorsService, Stock = 0, Prix = 749.99m, Vendu = false });

        modelBuilder.Entity<Ordinateur>().OwnsOne(o => o.Caracteristiques).HasData(
            new { OrdinateurId = 1, RamGo = 16, TypeOrdi = TypeOrdinateur.Fixe, StockageGo = 512, Processeur = "Intel Core i7", Marque = "Dell" },
            new { OrdinateurId = 2, RamGo = 8, TypeOrdi = TypeOrdinateur.Fixe, StockageGo = 256, Processeur = "Intel Core i5", Marque = "HP" },
            new { OrdinateurId = 3, RamGo = 32, TypeOrdi = TypeOrdinateur.Fixe, StockageGo = 1000, Processeur = "AMD Ryzen 7", Marque = "Lenovo" },
            new { OrdinateurId = 4, RamGo = 32, TypeOrdi = TypeOrdinateur.Fixe, StockageGo = 1000, Processeur = "Apple M2", Marque = "Apple" },
            new { OrdinateurId = 5, RamGo = 16, TypeOrdi = TypeOrdinateur.Portable, StockageGo = 512, Processeur = "AMD Ryzen 5", Marque = "Asus" });
    }
}
