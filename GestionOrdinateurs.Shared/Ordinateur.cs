using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GestionOrdinateurs.Shared;

[Table("Ordinateur")]
public class Ordinateur
{
    public int Id { get; set; }
    public string Nom { get; set; } = string.Empty;
    public EtatOrdinateur Etat { get; set; }

    [DefaultValue(0)]
    public int Stock { get; set; }

    [DefaultValue(0)]
    public decimal Prix { get; set; }

    [DefaultValue(false)]
    public bool Vendu { get; set; } = false;

    public Caracteristiques Caracteristiques { get; set; } = new();

    [JsonIgnore]
    public List<Reparation> Reparations { get; set; } = new();

    public Ordinateur()
    {
    }

    public Ordinateur(
        int id,
        string nom,
        string marque,
        int ram,
        string processeur,
        EtatOrdinateur etat,
        int stock = 1,
        decimal prix = 0,
        bool vendu = false)
    {
        Id = id;
        Nom = nom;
        Etat = etat;
        Stock = stock;
        Prix = prix;
        Vendu = vendu;
        Caracteristiques = new Caracteristiques(
            ram, 0, TypeOrdinateur.Fixe, processeur, marque);
    }

    public void MettreEnService() => Etat = EtatOrdinateur.EnService;
    public void MettreEnMaintenance() => Etat = EtatOrdinateur.EnMaintenance;

    public override string ToString() =>
        $"{Nom} ({Caracteristiques.Marque}) - {Caracteristiques.RamGo} Go RAM - {Caracteristiques.Processeur} - {Etat}";
}
