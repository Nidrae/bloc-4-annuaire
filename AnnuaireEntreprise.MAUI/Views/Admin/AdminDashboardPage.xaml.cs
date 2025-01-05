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
    await Navigation.PushAsync(new ManageSitesPage());
}

private async void OnManageServicesClicked(object sender, EventArgs e)
{
    await Navigation.PushAsync(new ManageServicesPage());
}

private async void OnManageEmployeesClicked(object sender, EventArgs e)
{
    await Navigation.PushAsync(new ManageEmployeesPage());
}

private async void OnBackToHomeClicked(object sender, EventArgs e)
{
    await Navigation.PopToRootAsync();
}

 }