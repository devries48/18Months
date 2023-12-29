using Months18.ViewModels;

namespace Months18.Pages;

public partial class SettingsPage : ContentPage
{
    public SettingsPage()
    {
        InitializeComponent();

        BindingContext = new AnimalViewModel
        {
            GroupName = "Connecion",
            Selection = "Local"
        };
    }
}
