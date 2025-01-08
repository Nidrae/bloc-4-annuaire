using AnnuaireEntreprise.MAUI.Views;
using AnnuaireEntreprise.MAUI.Views.Admin;
using AnnuaireEntreprise.MAUI.Models;
using System.Net.Http.Json;

namespace AnnuaireEntreprise.MAUI.Views;

public partial class SearchPage : ContentPage
{
    private readonly HttpClient _httpClient;
    private List<Employee> _allEmployees = new();
    private int _currentPage = 1;
    private const int PageSize = 10;

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
                _allEmployees = await _httpClient.GetFromJsonAsync<List<Employee>>("Salaries");
            }
            else
            {
                // Si le champ n'est pas vide, effectuer une recherche
                _allEmployees = await _httpClient.GetFromJsonAsync<List<Employee>>($"Salaries/search?name={e.NewTextValue}");
            }

            _currentPage = 1;
            UpdatePagination();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erreur", $"Impossible de charger les données : {ex.Message}", "OK");
        }
    }

    private void UpdatePagination()
    {
        // Calculer les employés à afficher pour la page actuelle
        var employeesToShow = _allEmployees
            .Skip((_currentPage - 1) * PageSize)
            .Take(PageSize)
            .ToList();

        EmployeeList.ItemsSource = employeesToShow;

        // Mettre à jour l'état des boutons de pagination
        PreviousButton.IsEnabled = _currentPage > 1;
        NextButton.IsEnabled = _currentPage < (_allEmployees.Count + PageSize - 1) / PageSize;
    }


    private void OnNextPageClicked(object sender, EventArgs e)
    {
        if (_currentPage < (_allEmployees.Count + PageSize - 1) / PageSize)
        {
            _currentPage++;
            UpdatePagination();
        }
    }

    private void OnPreviousPageClicked(object sender, EventArgs e)
    {
        if (_currentPage > 1)
        {
            _currentPage--;
            UpdatePagination();
        }
    }

    private async void OnSiteSelected(object sender, EventArgs e)
    {
        if (SitePicker.SelectedIndex == -1) return;

        try
        {
            var siteId = SitePicker.SelectedIndex + 1; // Assumes Picker index matches database ID
            _allEmployees = await _httpClient.GetFromJsonAsync<List<Employee>>($"Salaries/by-site/{siteId}");
            _currentPage = 1;
            UpdatePagination();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erreur", $"Impossible de charger les données : {ex.Message}", "OK");
        }
    }

    private async void OnServiceSelected(object sender, EventArgs e)
    {
        if (ServicePicker.SelectedIndex == -1) return;

        try
        {
            var serviceId = ServicePicker.SelectedIndex + 1; // Assumes Picker index matches database ID
            _allEmployees = await _httpClient.GetFromJsonAsync<List<Employee>>($"Salaries/by-service/{serviceId}");
            _currentPage = 1;
            UpdatePagination();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erreur", $"Impossible de charger les données : {ex.Message}", "OK");
        }
    }

    private async void OnDetailsButtonClicked(object sender, EventArgs e)
    {
        try
        {
            var button = sender as Button;
            var employee = button?.CommandParameter as Employee;

            if (employee != null)
            {
                // Navigation vers la page des détails de l'employé
                await Navigation.PushAsync(new EmployeeDetailsPage(employee));
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erreur", $"Impossible d'afficher les détails : {ex.Message}", "OK");
        }
    }


}
