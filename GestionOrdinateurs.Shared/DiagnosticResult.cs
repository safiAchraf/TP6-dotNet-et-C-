namespace GestionOrdinateurs.Shared;

public struct DiagnosticResult
{
    public string Action { get; set; }
    public bool NecessiteIntervention { get; set; }

    public DiagnosticResult(string action, bool necessiteIntervention)
    {
        Action = action;
        NecessiteIntervention = necessiteIntervention;
    }

    public override string ToString()
    {
        return Action;
    }
}
