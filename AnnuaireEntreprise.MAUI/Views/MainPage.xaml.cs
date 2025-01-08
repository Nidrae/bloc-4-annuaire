using AnnuaireEntreprise.MAUI.Views.Admin;

namespace AnnuaireEntreprise.MAUI;

public partial class MainPage : ContentPage
{
    int count = 0;

    public MainPage()
    {
        InitializeComponent();
        var clickGestureRecognizer = new TapGestureRecognizer();
        clickGestureRecognizer.Tapped += OnImageTapped;
        AdminTriggerImage.GestureRecognizers.Add(clickGestureRecognizer);
    }


    private async void OnSearchButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Views.SearchPage());
    }



    // Gestion des clics sur l'image
    private int _adminClickCount = 0; // Compteur de clics

    private void OnImageTapped(object sender, EventArgs e)
    {
        _adminClickCount++;

        if (_adminClickCount == 10)
        {
            PasswordEntry.IsVisible = true;
            ValidateButton.IsVisible = true;
            _adminClickCount = 0; // Réinitialiser le compteur après 10 clics
        }
    }

    private async void OnValidateButtonClicked(object sender, EventArgs e)
    {
        if (PasswordEntry.Text == "admbloc4")
        {
            await Navigation.PushAsync(new AdminDashboardPage());
        }
        else
        {
            await DisplayAlert("Erreur", "Mot de passe incorrect", "OK");
        }
    }


}

