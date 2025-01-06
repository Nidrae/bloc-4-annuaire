using System.ComponentModel.DataAnnotations.Schema;

public class Service
{
    public int Id { get; set; }
    public string Nom { get; set; }

    [NotMapped] // Indique que cette propriété ne sera pas stockée dans la base de données
    public bool IsLinkedToEmployees { get; set; }
}
