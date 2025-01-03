namespace AnnuaireEntreprise.MAUI.Views;

using AnnuaireEntreprise.MAUI.Models;

public partial class EmployeeDetailsPage : ContentPage
{
    public Employee Employee { get; set; }

    public EmployeeDetailsPage(Employee employee)
    {
        InitializeComponent();
        Employee = employee;
        BindingContext = this;
    }
}
