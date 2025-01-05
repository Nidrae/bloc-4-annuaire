using AnnuaireEntreprise.MAUI.Models;
using System.Net.Http.Json;

namespace AnnuaireEntreprise.MAUI.Views.Admin;

public partial class ManageServicesPage : ContentPage
{
    private readonly HttpClient _httpClient;
    private bool isRequestInProgress = false;

    public ManageServicesPage()
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
            var services = await _httpClient.GetFromJsonAsync<List<Service>>("Services");
            ServicesList.ItemsSource = services;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erreur", $"Impossible de charger les services : {ex.Message}", "OK");
        }
        finally
        {
            isRequestInProgress = false;
        }
    }

    private async void OnAddServiceClicked(object sender, EventArgs e)
    {
        string serviceName = await DisplayPromptAsync("Ajouter un Service", "Nom du service :");
        if (!string.IsNullOrWhiteSpace(serviceName))
        {
            var newService = new Service { Nom = serviceName };
            await _httpClient.PostAsJsonAsync("Services", newService);
            LoadData();
        }
    }

    private async void OnEditServiceClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is Service selectedService)
        {
            string updatedName = await DisplayPromptAsync("Modifier un Service", "Nom du service :", initialValue: selectedService.Nom);
            if (!string.IsNullOrWhiteSpace(updatedName))
            {
                selectedService.Nom = updatedName;
                await _httpClient.PutAsJsonAsync($"Services/{selectedService.Id}", selectedService);
                LoadData();
            }
        }
    }

    private async void OnDeleteServiceClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is Service selectedService)
        {
            if (await DisplayAlert("Confirmation", $"Supprimer le service {selectedService.Nom} ?", "Oui", "Non"))
            {
                await _httpClient.DeleteAsync($"Services/{selectedService.Id}");
                LoadData();
            }
        }
    }
}
