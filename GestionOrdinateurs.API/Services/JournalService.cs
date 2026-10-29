namespace GestionOrdinateurs.API.Services;

public class JournalService
{
    private readonly string _reparationsLog = Path.Combine("Logs", "Reparations", "reparations.log");
    private readonly string _ordinateursLog = Path.Combine("Logs", "Ordinateurs", "ordinateurs.log");
    private readonly object _lock = new();

    public JournalService()
    {
        Directory.CreateDirectory(Path.Combine("Logs", "Reparations"));

        Directory.CreateDirectory(Path.Combine("Logs", "Ordinateurs"));
    }

    public void Ecrire(string message)
    {
        lock (_lock)
        {
            using var writer = new StreamWriter(_reparationsLog, append: true);
            writer.WriteLine($"{DateTime.Now:dd/MM/yyyy} - {message}");
        }
    }

    public void EcrireOrdinateur(string message)
    {
        lock (_lock)
        {
            using var writer = new StreamWriter(_ordinateursLog, append: true);
            writer.WriteLine($"{DateTime.Now:dd/MM/yyyy} - {message}");
        }
    }
}
