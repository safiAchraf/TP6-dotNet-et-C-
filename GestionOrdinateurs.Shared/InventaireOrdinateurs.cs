namespace GestionOrdinateurs.Shared;

public class InventaireOrdinateurs
{
    public List<Ordinateur> Ordinateurs { get; set; } = new();

    public Ordinateur? this[int id]
    {
        get
        {
            foreach (var ordi in Ordinateurs)
            {
                if (ordi.Id == id)
                    return ordi;
            }
            return null;
        }
    }

    public Ordinateur? this[string nom]
    {
        get
        {
            foreach (var ordi in Ordinateurs)
            {
                if (ordi.Nom.Equals(nom, StringComparison.OrdinalIgnoreCase))
                    return ordi;
            }
            return null;
        }
    }
}
