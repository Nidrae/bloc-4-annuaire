using AnnuaireEntreprise.API.Data;
using Microsoft.AspNetCore.Mvc;

namespace AnnuaireEntreprise.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    private readonly AppDbContext _context;

    public TestController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("test-connection")]
    public IActionResult TestConnection()
    {
        try
        {
            var sites = _context.Sites.ToList();
            return Ok(new { Message = "Connexion réussie", Sites = sites });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "Erreur de connexion", Error = ex.Message });
        }
    }
}
