using AnnuaireEntreprise.API.Data;
using AnnuaireEntreprise.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnnuaireEntreprise.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServicesController : ControllerBase
{
    private readonly AppDbContext _context;

    public ServicesController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/Services
[HttpGet]
public async Task<ActionResult<IEnumerable<Service>>> GetServices()
{
    var services = await _context.Services
        .Select(service => new Service
        {
            Id = service.Id,
            Nom = service.Nom,
            IsLinkedToEmployees = _context.Salaries.Any(emp => emp.ServiceId == service.Id) // Calcul dynamique
        })
        .ToListAsync();

    return Ok(services);
}


[HttpGet("{id}")]
public async Task<ActionResult<Service>> GetService(int id)
{
    var service = await _context.Services
        .Where(s => s.Id == id)
        .Select(s => new Service
        {
            Id = s.Id,
            Nom = s.Nom,
            IsLinkedToEmployees = _context.Salaries.Any(emp => emp.ServiceId == s.Id) // Calcul dynamique
        })
        .FirstOrDefaultAsync();

    if (service == null)
    {
        return NotFound(new { Message = $"Le service avec l'ID {id} n'existe pas." });
    }

    return Ok(service);
}


    // POST: api/Services
    [HttpPost]
    public async Task<ActionResult<Service>> CreateService(Service service)
    {
        _context.Services.Add(service);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetService), new { id = service.Id }, service);
    }

    // PUT: api/Services/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateService(int id, Service service)
    {
        if (id != service.Id)
        {
            return BadRequest(new { Message = "L'ID de l'URL ne correspond pas à l'ID de l'objet." });
        }

        _context.Entry(service).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ServiceExists(id))
            {
                return NotFound(new { Message = $"Le service avec l'ID {id} n'existe pas." });
            }
            throw;
        }

        return NoContent();
    }

    // DELETE: api/Services/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteService(int id)
    {
        var service = await _context.Services.FindAsync(id);
        if (service == null)
        {
            return NotFound(new { Message = $"Le service avec l'ID {id} n'existe pas." });
        }

        _context.Services.Remove(service);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ServiceExists(int id)
    {
        return _context.Services.Any(e => e.Id == id);
    }



}
