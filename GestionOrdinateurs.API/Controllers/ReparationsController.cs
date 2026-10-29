namespace GestionOrdinateurs.API.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GestionOrdinateurs.API.Services;
using GestionOrdinateurs.Shared;

[Authorize]
[ApiController]
[Route("api/[controller]")]
[Route("api/reparation")]
public class ReparationsController : ControllerBase
{
    private readonly ReparationService _reparationService;
    private readonly StockService _stockService;

    public ReparationsController(ReparationService reparationService, StockService stockService)
    {
        _reparationService = reparationService;
        _stockService = stockService;
    }

    // Simulation de réparations en parallèle (réservé aux Admins)
    [HttpPost("SimulerReparation")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> SimulerReparation()
    {
        var ordinateurs = await _reparationService.GetOrdinateursPourSimulationAsync(5);
        if (ordinateurs.Count < 5)
        {
            return BadRequest("Au moins 5 ordinateurs sont nécessaires pour la simulation.");
        }

        var tasks = ordinateurs.Select(o => _reparationService.SimulerReparationAsync(o));
        await Task.WhenAll(tasks);

        return Ok($"Simulation terminée : {ordinateurs.Count} réparations effectuées.");
    }

    [HttpGet]
    public async Task<ActionResult<List<Reparation>>> GetAll()
    {
        var list = await _reparationService.GetAllAsync();
        return Ok(list);
    }

    [HttpGet("GetStockSSD")]
    public async Task<ActionResult<int>> GetStockSSD()
    {
        var tasks = Enumerable.Range(0, 10).Select(_ => _stockService.ConsommerSSD());
        await Task.WhenAll(tasks);
        return Ok(_stockService.StockSSD);
    }

    [HttpGet("historique")]
    public async Task<IActionResult> Historique()
    {
        var historique = await _reparationService.GetHistoriqueAsync();
        return Ok(historique);
    }
}
