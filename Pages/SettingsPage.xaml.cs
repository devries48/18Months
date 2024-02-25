namespace Months18.Pages;

public partial class SettingsPage : ContentPage
{
    public SettingsPage()
    {
        InitializeComponent();

        BindingContext = MauiProgram.GetService<SettingsPageViewModel>();
    }

    public SettingsPageViewModel ViewModel => (SettingsPageViewModel)BindingContext;

}
