using AnnuaireEntreprise.MAUI.Models;
using System.Net.Http.Json;

namespace AnnuaireEntreprise.MAUI.Views.Admin;

public partial class AddEditEmployeePage : ContentPage
{
    private readonly HttpClient _httpClient;
    private Employee _employee;

    public AddEditEmployeePage(Employee employee = null)
    {
        InitializeComponent();
        _httpClient = new HttpClient { BaseAddress = new Uri("http://172.20.64.1:5195/api/") };
        _employee = employee;

        // Charger les données pour les pickers
        LoadData();
    }

    private async void LoadData()
    {
        try
        {
            var services = await _httpClient.GetFromJsonAsync<List<Service>>("Services");
            var sites = await _httpClient.GetFromJsonAsync<List<Site>>("Sites");

            ServicePicker.ItemsSource = services ?? new List<Service>();
            SitePicker.ItemsSource = sites ?? new List<Site>();

            // Une fois les pickers chargés, peupler les champs si nécessaire
            PopulateFields();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erreur", $"Impossible de charger les données : {ex.Message}", "OK");
        }
    }

    private void PopulateFields()
    {
        if (_employee == null)
        {
            return; // Pas d'employé à charger
        }

        NameEntry.Text = _employee.Nom;
        FirstNameEntry.Text = _employee.Prenom;
        PhoneEntry.Text = _employee.TelFixe;
        MobilePhoneEntry.Text = _employee.TelPortable;
        EmailEntry.Text = _employee.Email;

        ServicePicker.SelectedItem = ((List<Service>)ServicePicker.ItemsSource)?.FirstOrDefault(s => s.Id == _employee.Service?.Id);
        SitePicker.SelectedItem = ((List<Site>)SitePicker.ItemsSource)?.FirstOrDefault(s => s.Id == _employee.Site?.Id);
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        try
        {
            if (ServicePicker.SelectedItem == null || !(ServicePicker.SelectedItem is Service selectedService))
            {
                await DisplayAlert("Erreur", "Veuillez sélectionner un service valide.", "OK");
                return;
            }

            if (SitePicker.SelectedItem == null || !(SitePicker.SelectedItem is Site selectedSite))
            {
                await DisplayAlert("Erreur", "Veuillez sélectionner un site valide.", "OK");
                return;
            }

            var employee = new
            {
                Id = _employee?.Id ?? 0,
                Nom = NameEntry.Text,
                Prenom = FirstNameEntry.Text,
                TelFixe = PhoneEntry.Text,
                TelPortable = MobilePhoneEntry.Text,
                Email = EmailEntry.Text,
                ServiceId = selectedService.Id,
                SiteId = selectedSite.Id
            };

            HttpResponseMessage response;

            if (_employee == null)
            {
                response = await _httpClient.PostAsJsonAsync("Salaries", employee);
            }
            else
            {
                response = await _httpClient.PutAsJsonAsync($"Salaries/{_employee.Id}", employee);
            }

            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Succès", "Salarié enregistré avec succès.", "OK");

                if (Navigation.NavigationStack.LastOrDefault() is ManageEmployeesPage employeePage)
                {
                    employeePage.RefreshEmployeeList();
                }

                await Navigation.PopAsync();
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                await DisplayAlert("Erreur", $"Erreur lors de l'enregistrement : {error}", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erreur", $"Une erreur est survenue : {ex.Message}", "OK");
        }
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        Console.WriteLine("Bouton Annuler cliqué"); // Diagnostic
        await Navigation.PopAsync();
    }
}
