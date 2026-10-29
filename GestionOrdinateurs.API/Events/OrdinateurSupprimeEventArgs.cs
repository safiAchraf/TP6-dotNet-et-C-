using GestionOrdinateurs.Shared;

namespace GestionOrdinateurs.API.Events;

public class OrdinateurSupprimeEventArgs : EventArgs
{
    public Ordinateur Ordinateur { get; set; } = null!;
}
