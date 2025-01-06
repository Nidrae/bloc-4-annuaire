using AnnuaireEntreprise.API.Data;
using AnnuaireEntreprise.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnnuaireEntreprise.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SitesController : ControllerBase
{
    private readonly AppDbContext _context;

    public SitesController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/Sites
[HttpGet]
public async Task<ActionResult<IEnumerable<Site>>> GetSites()
{
    var sites = await _context.Sites
        .Select(site => new Site
        {
            Id = site.Id,
            Ville = site.Ville,
            IsLinkedToEmployees = _context.Salaries.Any(emp => emp.SiteId == site.Id) // Calcul dynamique
        })
        .ToListAsync();

    return Ok(sites);
}



    // GET: api/Sites/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Site>> GetSite(int id)
    {
        var site = await _context.Sites.FindAsync(id);

        if (site == null)
        {
            return NotFound(new { Message = $"Le site avec l'ID {id} n'existe pas." });
        }

        return site;
    }

    // POST: api/Sites
    [HttpPost]
    public async Task<ActionResult<Site>> CreateSite(Site site)
    {
        _context.Sites.Add(site);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetSite), new { id = site.Id }, site);
    }

    // PUT: api/Sites/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSite(int id, Site site)
    {
        if (id != site.Id)
        {
            return BadRequest(new { Message = "L'ID de l'URL ne correspond pas à l'ID de l'objet." });
        }

        _context.Entry(site).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!SiteExists(id))
            {
                return NotFound(new { Message = $"Le site avec l'ID {id} n'existe pas." });
            }
            throw;
        }

        return NoContent();
    }

    // DELETE: api/Sites/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSite(int id)
    {
        var site = await _context.Sites.FindAsync(id);
        if (site == null)
        {
            return NotFound(new { Message = $"Le site avec l'ID {id} n'existe pas." });
        }

        _context.Sites.Remove(site);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool SiteExists(int id)
    {
        return _context.Sites.Any(e => e.Id == id);
    }


}
