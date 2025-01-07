using AnnuaireEntreprise.MAUI.Views;
using AnnuaireEntreprise.MAUI.Views.Admin;
using AnnuaireEntreprise.MAUI.Models;
using System.Net.Http.Json;

namespace AnnuaireEntreprise.MAUI.Views;

public partial class SearchPage : ContentPage
{
    private readonly HttpClient _httpClient;

    public SearchPage()
    {
        InitializeComponent();
        _httpClient = new HttpClient { BaseAddress = new Uri("http://172.20.64.1:5195/api/") };
        LoadData();
    }

private async void LoadData()
{
    try
    {
        var sites = await _httpClient.GetFromJsonAsync<List<Site>>("Sites");
        var services = await _httpClient.GetFromJsonAsync<List<Service>>("Services");

        if (sites != null)
        {
            SitePicker.ItemsSource = sites;
            SitePicker.ItemDisplayBinding = new Binding("Ville"); // Affiche uniquement la ville
        }

        if (services != null)
        {
            ServicePicker.ItemsSource = services;
            ServicePicker.ItemDisplayBinding = new Binding("Nom"); // Affiche uniquement le nom
        }
    }
    catch (Exception ex)
    {
        await DisplayAlert("Erreur", $"Échec du chargement des données : {ex.Message}", "OK");
    }
}


private async void OnSearchTextChanged(object sender, TextChangedEventArgs e)
{
    try
    {
        if (string.IsNullOrWhiteSpace(e.NewTextValue))
        {
            // Si le champ est vide, recharger tous les employés
            var employees = await _httpClient.GetFromJsonAsync<List<Employee>>("Salaries");
            EmployeeList.ItemsSource = employees;
        }
        else
        {
            // Si le champ n'est pas vide, effectuer une recherche
            var employees = await _httpClient.GetFromJsonAsync<List<Employee>>($"Salaries/search?name={e.NewTextValue}");
            EmployeeList.ItemsSource = employees;
        }
    }
    catch (Exception ex)
    {
        await DisplayAlert("Erreur", $"Impossible de charger les données : {ex.Message}", "OK");
    }
}




private async void OnSiteSelected(object sender, EventArgs e)
{
    if (SitePicker.SelectedItem is Site selectedSite)
    {
        var url = $"Salaries/by-site/{selectedSite.Id}";
        Console.WriteLine($"URL appelée : {url}"); // Log de l'URL

        try
        {
            var employees = await _httpClient.GetFromJsonAsync<List<Employee>>(url);
            Console.WriteLine($"Résultats reçus : {employees?.Count ?? 0}");
            EmployeeList.ItemsSource = employees;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de l'appel à l'API : {ex.Message}");
            await DisplayAlert("Erreur", $"Impossible de charger les données : {ex.Message}", "OK");
        }
    }
}



private async void OnServiceSelected(object sender, EventArgs e)
{
    if (ServicePicker.SelectedItem is Service selectedService)
    {
        var url = $"Salaries/by-service/{selectedService.Id}";
        Console.WriteLine($"URL appelée : {url}"); // Log de l'URL

        try
        {
            var employees = await _httpClient.GetFromJsonAsync<List<Employee>>(url);
            Console.WriteLine($"Résultats reçus : {employees?.Count ?? 0}");
            EmployeeList.ItemsSource = employees;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de l'appel à l'API : {ex.Message}");
            await DisplayAlert("Erreur", $"Impossible de charger les données : {ex.Message}", "OK");
        }
    }
}


private async void OnDetailsButtonClicked(object sender, EventArgs e)
{
    var button = sender as Button;
    var employee = button?.CommandParameter as Employee;

    if (employee != null)
    {
        await Navigation.PushAsync(new EmployeeDetailsPage(employee));
    }
}





}
