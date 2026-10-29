namespace GestionOrdinateurs.API.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionOrdinateurs.API.Data;
using GestionOrdinateurs.Shared;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly ParcInformatiqueContext _context;

    public UsersController(ParcInformatiqueContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<User>>> GetAll()
    {
        var users = await _context.Users.ToListAsync();
        return Ok(users);
    }

    // Recherche de l'utilisateur par login (insensible à la casse)
    [HttpGet("{login}")]
    public async Task<ActionResult<User>> GetByLogin(string login)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Login.ToLower() == login.ToLower());
        if (user == null)
        {
            return NotFound($"Utilisateur '{login}' introuvable.");
        }
        return Ok(user);
    }
}
