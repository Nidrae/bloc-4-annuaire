using AnnuaireEntreprise.MAUI.Models;
using System.Net.Http.Json;

namespace AnnuaireEntreprise.MAUI.Views.Admin;

public partial class ManageEmployeesPage : ContentPage
{
    private readonly HttpClient _httpClient;
    private bool isRequestInProgress = false;

    public ManageEmployeesPage()
    {
        InitializeComponent();
        _httpClient = new HttpClient { BaseAddress = new Uri("http://172.20.64.1:5195/api/") };
        LoadData();
    }

    public void RefreshEmployeeList()
    {
        LoadData(); // Recharger les données
    }

    private async void LoadData()
    {
        if (isRequestInProgress) return; // Évite d'exécuter la méthode si une requête est en cours

        isRequestInProgress = true;
        try
        {
            // Charger les salariés
            var employees = await _httpClient.GetFromJsonAsync<List<Employee>>("Salaries");
            EmployeeList.ItemsSource = employees;

            // Charger les sites et services
            var sites = await _httpClient.GetFromJsonAsync<List<Site>>("Sites");
            var services = await _httpClient.GetFromJsonAsync<List<Service>>("Services");

            // Assigner les données aux Pickers
            SitePicker.ItemsSource = sites?.Select(s => s.Ville).ToList() ?? new List<string>();
            ServicePicker.ItemsSource = services?.Select(s => s.Nom).ToList() ?? new List<string>();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erreur", $"Impossible de charger les données : {ex.Message}", "OK");
        }
        finally
        {
            isRequestInProgress = false;
        }
    }

    private async void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        if (isRequestInProgress) return; // Évite les appels multiples rapides
        isRequestInProgress = true;

        try
        {
            var employees = await _httpClient.GetFromJsonAsync<List<Employee>>($"Salaries/search?name={e.NewTextValue}");
            EmployeeList.ItemsSource = employees;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erreur", $"Recherche impossible : {ex.Message}", "OK");
        }
        finally
        {
            isRequestInProgress = false;
        }
    }

    private async void OnSiteSelected(object sender, EventArgs e)
    {
        if (isRequestInProgress || SitePicker.SelectedIndex == -1) return;
        isRequestInProgress = true;

        try
        {
            var selectedSite = SitePicker.SelectedItem as string;
            var employees = await _httpClient.GetFromJsonAsync<List<Employee>>("Salaries");

            EmployeeList.ItemsSource = employees
                .Where(emp => emp.Site != null && emp.Site.Ville == selectedSite)
                .ToList();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erreur", $"Erreur de chargement : {ex.Message}", "OK");
        }
        finally
        {
            isRequestInProgress = false;
        }
    }

    private async void OnServiceSelected(object sender, EventArgs e)
    {
        if (isRequestInProgress || ServicePicker.SelectedIndex == -1) return;
        isRequestInProgress = true;

        try
        {
            var selectedService = ServicePicker.SelectedItem as string;
            var employees = await _httpClient.GetFromJsonAsync<List<Employee>>("Salaries");

            EmployeeList.ItemsSource = employees
                .Where(emp => emp.Service != null && emp.Service.Nom == selectedService)
                .ToList();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erreur", $"Erreur de chargement : {ex.Message}", "OK");
        }
        finally
        {
            isRequestInProgress = false;
        }
    }

       private async void OnEditEmployeeClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is Employee selectedEmployee)
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
        var employee = (sender as Button)?.BindingContext as Employee;
        if (employee != null && await DisplayAlert("Confirmation", $"Supprimer le salarié {employee.Nom} {employee.Prenom} ?", "Oui", "Non"))
        {
            await _httpClient.DeleteAsync($"Salaries/{employee.Id}");
            RefreshEmployeeList(); // Recharger la liste des salariés après suppression
        }
    }

     private async void OnAddEmployeeClicked(object sender, EventArgs e)
{
    await Navigation.PushAsync(new AddEditEmployeePage());
}
}
