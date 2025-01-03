using System.Net.Http;
using System.Net.Http.Json;

namespace AnnuaireEntreprise.MAUI.Views;

public partial class TestApiPage : ContentPage
{
    private readonly HttpClient _httpClient;
    public TestApiPage()
    {
        InitializeComponent();
        _httpClient = new HttpClient { BaseAddress = new Uri("http://172.20.64.1:5195/api/") };

    }

    private async void OnTestGetSitesClicked(object sender, EventArgs e)
    {
        try
        {
            var sites = await _httpClient.GetFromJsonAsync<List<Site>>("Sites");
            string message = sites != null && sites.Any()
                ? $"Sites fetched successfully: {string.Join(", ", sites.Select(s => s.Ville))}"
                : "No sites found.";

            await DisplayAlert("API Response", message, "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to fetch sites: {ex.Message}", "OK");
        }
    }
}

// DTO pour le modèle Site
public class Site
{
    public int Id { get; set; }
    public string Ville { get; set; }
}
