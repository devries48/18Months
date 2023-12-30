using Months18.ViewModels;

namespace Months18.Pages;

public partial class SettingsPage : ContentPage
{
    public SettingsPage()
    {
        InitializeComponent();

        BindingContext = MauiProgram.GetService<SettingsPageViewModel>();
    }

    public SettingsPageViewModel ViewModel => (SettingsPageViewModel)BindingContext;

    //EventToCommand cannot be used on ContentPage
    private async void ContentPage_Appearing(object sender, EventArgs e) => await ViewModel.LoadSettings();
}
