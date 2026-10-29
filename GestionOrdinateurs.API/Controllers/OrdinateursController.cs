namespace GestionOrdinateurs.API.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GestionOrdinateurs.API.Data;
using GestionOrdinateurs.API.Services;
using GestionOrdinateurs.Shared;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class OrdinateursController : ControllerBase
{
    private readonly IOrdinateurService _service;
    private readonly ParcInformatiqueContext _context;
    private readonly ILogger<OrdinateursController> _logger;
    private readonly IConfiguration _configuration;

    public OrdinateursController(
        IOrdinateurService service,
        ParcInformatiqueContext context,
        ILogger<OrdinateursController> logger,
        IConfiguration configuration)
    {
        _service = service;
        _context = context;
        _logger = logger;
        _configuration = configuration;
    }

    [HttpGet]
    public async Task<ActionResult<List<Ordinateur>>> GetAll()
    {
        _logger.LogInformation("Récupération de tous les ordinateurs.");
        var liste = await _service.GetAllAsync();
        return Ok(liste);
    }

    [HttpGet("info")]
    public IActionResult Info()
    {
        _logger.LogInformation("Consultation des informations de l'application.");
        return Ok(new
        {
            Name = _configuration["Application:Name"],
            Version = _configuration["Application:Version"]
        });
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Ordinateur>> GetById(int id)
    {
        _logger.LogInformation("Recherche de l’ordinateur {Id}.", id);
        var ordinateur = await _service.GetByIdAsync(id);
        if (ordinateur == null)
        {
            return NotFound();
        }
        return Ok(ordinateur);
    }

    [HttpGet("{id:int}/diagnostic")]
    public async Task<IActionResult> Diagnostiquer(int id)
    {
        _logger.LogInformation("Diagnostic de l’ordinateur {Id}.", id);
        var ordinateur = await _service.GetByIdAsync(id);
        if (ordinateur == null)
        {
            return NotFound();
        }

        var resultat = _service.Diagnostiquer(ordinateur);
        return Ok(resultat);
    }

    [HttpGet("statistiques")]
    public IActionResult Statistiques()
    {
        _logger.LogInformation("Consultation des statistiques.");
        var (total, enStock, valeurStock, reparations) = _service.CalculerStatistiques();
        return Ok(new
        {
            TotalOrdinateur = total,
            OrdiDisponibles = enStock,
            ValeurStock = valeurStock,
            NbReparations = reparations
        });
    }

    [HttpGet("inventaire/{nom}")]
    public IActionResult Chercher(string nom)
    {
        _logger.LogInformation("Recherche dans l’inventaire : {Nom}.", nom);
        var inventaire = new InventaireOrdinateurs();
        inventaire.Ordinateurs = _context.Ordinateurs.ToList();

        var ordinateur = inventaire[nom];

        if (ordinateur == null)
        {
            return NotFound();
        }

        return Ok(ordinateur);
    }

    [HttpGet("inventaire/id/{id:int}")]
    public IActionResult ChercherParId(int id)
    {
        _logger.LogInformation("Recherche dans l’inventaire : {Id}.", id);
        var inventaire = new InventaireOrdinateurs();
        inventaire.Ordinateurs = _context.Ordinateurs.ToList();

        var ordinateur = inventaire[id];

        if (ordinateur == null)
        {
            return NotFound();
        }

        return Ok(ordinateur);
    }

    [HttpGet("marque/{marque}")]
    public async Task<ActionResult<List<Ordinateur>>> GetByMarque(string marque)
    {
        _logger.LogInformation("Recherche par marque : {Marque}.", marque);
        var ordis = await _service.GetByMarqueAsync(marque);
        return Ok(ordis);
    }

    [HttpGet("disponibles")]
    public async Task<ActionResult<List<Ordinateur>>> GetDisponibles()
    {
        _logger.LogInformation("Consultation des ordinateurs disponibles.");
        var dispo = await _service.GetDisponiblesAsync();
        return Ok(dispo);
    }

    [HttpGet("prix-decroissant")]
    public async Task<ActionResult<List<Ordinateur>>> GetDuPlusCherAuMoinsCher()
    {
        _logger.LogInformation("Consultation des ordinateurs par prix décroissant.");
        var ordis = await _service.GetDuPlusCherAuMoinsCherAsync();
        return Ok(ordis);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Ordinateur>> Add(Ordinateur ordinateur)
    {
        _logger.LogInformation("Ajout de l’ordinateur {Nom}.", ordinateur.Nom);
        var created = await _service.AddAsync(ordinateur);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Ordinateur>> Update(int id, Ordinateur ordinateur)
    {
        _logger.LogInformation("Modification de l’ordinateur {Id}.", id);
        var updated = await _service.UpdateAsync(id, ordinateur);
        if (updated == null)
        {
            return NotFound();
        }
        return Ok(updated);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("Suppression de l’ordinateur {Id}.", id);
        var deleted = await _service.DeleteAsync(id);
        if (!deleted)
        {
            return NotFound();
        }
        return NoContent();
    }
}
