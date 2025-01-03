namespace AnnuaireEntreprise.MAUI.Models;

public class Employee
{
    public int Id { get; set; }
    public string Nom { get; set; }
    public string Prenom { get; set; }
    public string TelFixe { get; set; }
    public string TelPortable { get; set; }
    public string Email { get; set; }
    public Service Service { get; set; } // Modifié pour refléter un objet complexe
    public Site Site { get; set; }       // Modifié pour refléter un objet complexe
}
