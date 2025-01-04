using AnnuaireEntreprise.MAUI.Views;
using AnnuaireEntreprise.MAUI.Models;

namespace AnnuaireEntreprise.MAUI.Views.Admin;

public partial class AdminDashboardPage : ContentPage
{
    public AdminDashboardPage()
    {
        InitializeComponent();
    }

    private async void OnManageSitesClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SitesPage());
    }

    private async void OnManageServicesClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ServicesPage());
    }

    private async void OnManageEmployeesClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new EmployeesPage());
    }
}
