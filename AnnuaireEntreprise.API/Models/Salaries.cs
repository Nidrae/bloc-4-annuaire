namespace AnnuaireEntreprise.API.Models;

public class Salarie
{
    public int Id { get; set; }
    public string Nom { get; set; } = null!;
    public string Prenom { get; set; } = null!;
    public string? TelFixe { get; set; }
    public string? TelPortable { get; set; }
    public string Email { get; set; } = null!;
    public int ServiceId { get; set; }
    public int SiteId { get; set; }
    public Service? Service { get; set; }
    public Site? Site { get; set; }
}
