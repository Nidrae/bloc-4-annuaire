using AnnuaireEntreprise.MAUI.Models;
using System.Net.Http.Json;

namespace AnnuaireEntreprise.MAUI.Views.Admin;

public partial class ManageEmployeesPage : ContentPage
{
    private readonly HttpClient _httpClient;
    private List<Employee> _allEmployees = new();
    private int _currentPage = 1;
    private const int PageSize = 10;

    public ManageEmployeesPage()
    {
        InitializeComponent();
        _httpClient = new HttpClient { BaseAddress = new Uri("http://172.20.64.1:5195/api/") };
        LoadData();
    }

    private async void LoadData()
    {
        try
        {
            // Charger les sites et services
            var sites = await _httpClient.GetFromJsonAsync<List<Site>>("Sites");
            var services = await _httpClient.GetFromJsonAsync<List<Service>>("Services");

            SitePicker.ItemsSource = sites?.Select(s => s.Ville).ToList() ?? new List<string>();
            ServicePicker.ItemsSource = services?.Select(s => s.Nom).ToList() ?? new List<string>();

            // Charger les employés par défaut
            _allEmployees = await _httpClient.GetFromJsonAsync<List<Employee>>("Salaries");
            _currentPage = 1;
            UpdatePagination();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erreur", $"Échec du chargement des données : {ex.Message}", "OK");
        }
    }

    private void UpdatePagination()
    {
        var employeesToShow = _allEmployees
            .Skip((_currentPage - 1) * PageSize)
            .Take(PageSize)
            .ToList();

        EmployeeList.ItemsSource = employeesToShow;

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

    private async void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(e.NewTextValue))
            {
                _allEmployees = await _httpClient.GetFromJsonAsync<List<Employee>>("Salaries");
            }
            else
            {
                _allEmployees = await _httpClient.GetFromJsonAsync<List<Employee>>($"Salaries/search?name={e.NewTextValue}");
            }

            _currentPage = 1;
            UpdatePagination();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erreur", $"Impossible de rechercher : {ex.Message}", "OK");
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

    private async void OnEditEmployeeClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is Employee selectedEmployee)
        {
            await Navigation.PushAsync(new AddEditEmployeePage(selectedEmployee));
        }
        else
        {
            await DisplayAlert("Erreur", "Impossible de récupérer les données de l'employé.", "OK");
        }
    }

    private async void OnDeleteEmployeeClicked(object sender, EventArgs e)
    {
        var employee = (sender as Button)?.CommandParameter as Employee;
        if (employee != null && await DisplayAlert("Confirmation", $"Supprimer le salarié {employee.Nom} {employee.Prenom} ?", "Oui", "Non"))
        {
            await _httpClient.DeleteAsync($"Salaries/{employee.Id}");
            _allEmployees.Remove(employee);
            UpdatePagination();
        }
    }

    private async void OnAddEmployeeClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddEditEmployeePage());
    }

    public void RefreshEmployeeList()
    {
        _currentPage = 1; // Réinitialiser à la première page
        UpdatePagination(); // Actualiser l'affichage avec la pagination
    }

}
