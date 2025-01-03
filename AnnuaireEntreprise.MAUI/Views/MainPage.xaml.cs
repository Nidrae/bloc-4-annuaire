namespace AnnuaireEntreprise.MAUI;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();
	}

private async void OnSearchButtonClicked(object sender, EventArgs e)
{
    await Navigation.PushAsync(new Views.SearchPage());
}

private async void OnAdminButtonClicked(object sender, EventArgs e)
{
    await Navigation.PushAsync(new Views.Admin.SitesPage());
}

}

