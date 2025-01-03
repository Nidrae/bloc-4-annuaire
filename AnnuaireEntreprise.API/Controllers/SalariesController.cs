using AnnuaireEntreprise.API.Data;
using AnnuaireEntreprise.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnnuaireEntreprise.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalariesController : ControllerBase
{
    private readonly AppDbContext _context;

    public SalariesController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/Salaries
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Salarie>>> GetSalaries()
    {
        return await _context.Salaries
                             .Include(s => s.Site)
                             .Include(s => s.Service)
                             .ToListAsync();
    }

    // GET: api/Salaries/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Salarie>> GetSalarie(int id)
    {
        var salarie = await _context.Salaries
                                    .Include(s => s.Site)
                                    .Include(s => s.Service)
                                    .FirstOrDefaultAsync(s => s.Id == id);

        if (salarie == null)
        {
            return NotFound(new { Message = $"Le salarié avec l'ID {id} n'existe pas." });
        }

        return salarie;
    }

    // POST: api/Salaries
    [HttpPost]
    public async Task<ActionResult<Salarie>> CreateSalarie(Salarie salarie)
    {
        _context.Salaries.Add(salarie);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetSalarie), new { id = salarie.Id }, salarie);
    }

    // PUT: api/Salaries/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSalarie(int id, Salarie salarie)
    {
        if (id != salarie.Id)
        {
            return BadRequest(new { Message = "L'ID de l'URL ne correspond pas à l'ID de l'objet." });
        }

        _context.Entry(salarie).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!SalarieExists(id))
            {
                return NotFound(new { Message = $"Le salarié avec l'ID {id} n'existe pas." });
            }
            throw;
        }

        return NoContent();
    }

    // DELETE: api/Salaries/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSalarie(int id)
    {
        var salarie = await _context.Salaries.FindAsync(id);
        if (salarie == null)
        {
            return NotFound(new { Message = $"Le salarié avec l'ID {id} n'existe pas." });
        }

        _context.Salaries.Remove(salarie);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool SalarieExists(int id)
    {
        return _context.Salaries.Any(e => e.Id == id);
    }

    // GET: api/Salaries/search?name=Durand
[HttpGet("search")]
public async Task<ActionResult<IEnumerable<Salarie>>> SearchByName([FromQuery] string name)
{
    var results = await _context.Salaries
                                .Include(s => s.Site)
                                .Include(s => s.Service)
                                .Where(s => s.Nom.Contains(name))
                                .ToListAsync();

    if (!results.Any())
    {
        return NotFound(new { Message = $"Aucun salarié trouvé pour le nom contenant '{name}'." });
    }

    return Ok(results);
}
// GET: api/Salaries/by-site/{siteId}
[HttpGet("by-site/{siteId}")]
public async Task<ActionResult<IEnumerable<Salarie>>> GetBySite(int siteId)
{
    var results = await _context.Salaries
                                .Include(s => s.Site)
                                .Include(s => s.Service)
                                .Where(s => s.SiteId == siteId)
                                .ToListAsync();

    if (!results.Any())
    {
        return NotFound(new { Message = $"Aucun salarié trouvé pour le site ID {siteId}." });
    }

    return Ok(results);
}

// GET: api/Salaries/by-service/{serviceId}
[HttpGet("by-service/{serviceId}")]
public async Task<ActionResult<IEnumerable<Salarie>>> GetByService(int serviceId)
{
    var results = await _context.Salaries
                                .Include(s => s.Site)
                                .Include(s => s.Service)
                                .Where(s => s.ServiceId == serviceId)
                                .ToListAsync();

    if (!results.Any())
    {
        return NotFound(new { Message = $"Aucun salarié trouvé pour le service ID {serviceId}." });
    }

    return Ok(results);
}


}
