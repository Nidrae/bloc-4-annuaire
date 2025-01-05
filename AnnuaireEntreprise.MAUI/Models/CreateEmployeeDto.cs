namespace AnnuaireEntreprise.MAUI.Models;

public class CreateEmployeeDto
{
    public string Nom { get; set; }
    public string Prenom { get; set; }
    public string TelFixe { get; set; }
    public string TelPortable { get; set; }
    public string Email { get; set; }
    public int ServiceId { get; set; }
    public int SiteId { get; set; }
}
