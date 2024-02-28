namespace Months18.ViewModels;

public partial class SettingsPageViewModel : ObservableObject
{
    public SettingsPageViewModel()
    {
        MediaPath = Prefernces.MediaPath;
        TriviaPath = Prefernces.TriviaPath;
        
        var theme = Application.Current?.PlatformAppTheme ?? AppTheme.Dark;
        IsDarkTheme = theme == AppTheme.Dark;
    }

    [ObservableProperty]
    private string? mediaPath;

    [ObservableProperty]
    private string? triviaPath;

    [ObservableProperty]
    private bool isDarkTheme;

    [RelayCommand]
    private async Task SaveSettings()
    {
        if (MediaPath != null) Prefernces.MediaPath = MediaPath;
        if (TriviaPath != null) Prefernces.TriviaPath = TriviaPath;

        await Shell.Current.DisplayAlert(Shell.Current.Title, "Settings saved!", "OK");
    }
}
