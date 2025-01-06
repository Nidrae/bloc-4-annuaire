namespace AnnuaireEntreprise.MAUI.Models;

public class Site
{
    public int Id { get; set; }
    public string Ville { get; set; }
    public bool IsLinkedToEmployees { get; set; } // Pas besoin de [NotMapped] ici
}
