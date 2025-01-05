using AnnuaireEntreprise.MAUI.Models;
using System.Net.Http.Json;

namespace AnnuaireEntreprise.MAUI.Views.Admin;

public partial class ManageSitesPage : ContentPage
{
    private readonly HttpClient _httpClient;
    private bool isRequestInProgress = false;

    public ManageSitesPage()
    {
        InitializeComponent();
        _httpClient = new HttpClient { BaseAddress = new Uri("http://172.20.64.1:5195/api/") };
        LoadData();
    }

    private async void LoadData()
    {
        if (isRequestInProgress) return;
        isRequestInProgress = true;

        try
        {
            var sites = await _httpClient.GetFromJsonAsync<List<Site>>("Sites");
            SitesList.ItemsSource = sites;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erreur", $"Impossible de charger les sites : {ex.Message}", "OK");
        }
        finally
        {
            isRequestInProgress = false;
        }
    }

    private async void OnAddSiteClicked(object sender, EventArgs e)
    {
        string siteName = await DisplayPromptAsync("Ajouter un Site", "Nom du site :");
        if (!string.IsNullOrWhiteSpace(siteName))
        {
            var newSite = new Site { Ville = siteName };
            await _httpClient.PostAsJsonAsync("Sites", newSite);
            LoadData();
        }
    }

    private async void OnEditSiteClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is Site selectedSite)
        {
            string updatedName = await DisplayPromptAsync("Modifier un Site", "Nom du site :", initialValue: selectedSite.Ville);
            if (!string.IsNullOrWhiteSpace(updatedName))
            {
                selectedSite.Ville = updatedName;
                await _httpClient.PutAsJsonAsync($"Sites/{selectedSite.Id}", selectedSite);
                LoadData();
            }
        }
    }

    private async void OnDeleteSiteClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is Site selectedSite)
        {
            if (await DisplayAlert("Confirmation", $"Supprimer le site {selectedSite.Ville} ?", "Oui", "Non"))
            {
                await _httpClient.DeleteAsync($"Sites/{selectedSite.Id}");
                LoadData();
            }
        }
    }
}
