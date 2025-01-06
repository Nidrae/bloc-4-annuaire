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

            SitePicker.ItemsSource = sites.Select(s => s.Ville).ToList();
            ServicePicker.ItemsSource = services.Select(s => s.Nom).ToList();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load data: {ex.Message}", "OK");
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
        if (SitePicker.SelectedIndex == -1) return;

        var siteId = SitePicker.SelectedIndex + 1; // Assumes Picker index matches database ID
        var employees = await _httpClient.GetFromJsonAsync<List<Employee>>($"Salaries/by-site/{siteId}");
        EmployeeList.ItemsSource = employees;
    }

    private async void OnServiceSelected(object sender, EventArgs e)
    {
        if (ServicePicker.SelectedIndex == -1) return;

        var serviceId = ServicePicker.SelectedIndex + 1; // Assumes Picker index matches database ID
        var employees = await _httpClient.GetFromJsonAsync<List<Employee>>($"Salaries/by-service/{serviceId}");
        EmployeeList.ItemsSource = employees;
    }

private async void OnEmployeeSelected(object sender, SelectedItemChangedEventArgs e)
{
    if (e.SelectedItem == null) return;

    var selectedEmployee = e.SelectedItem as Employee;

    if (selectedEmployee != null)
    {
        await Navigation.PushAsync(new EmployeeDetailsPage(selectedEmployee));
    }

    // Déselectionne l'élément après navigation
    ((ListView)sender).SelectedItem = null;
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
