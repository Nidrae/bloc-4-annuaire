namespace AnnuaireEntreprise.MAUI.Models;

public class Service
{
    public int Id { get; set; }
    public string Nom { get; set; }
    public bool IsLinkedToEmployees { get; set; } // Pas besoin de [NotMapped] ici
}
