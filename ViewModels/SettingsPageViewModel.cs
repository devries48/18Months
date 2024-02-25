namespace Months18.ViewModels;

public partial class SettingsPageViewModel : ObservableObject
{
    public SettingsPageViewModel() => DataPath =Prefernces.DataPath;

    [ObservableProperty]
    private string? dataPath;

    [RelayCommand]
    private async Task SaveSettings()
    {
        if (DataPath == null)
            return;

        Prefernces.DataPath = DataPath;
        await Shell.Current.DisplayAlert(Shell.Current.Title, "Settings saved!", "OK");
    }
}
