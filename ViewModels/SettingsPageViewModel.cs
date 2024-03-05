namespace Months18.ViewModels;

public partial class SettingsPageViewModel : ObservableObject
{
    public SettingsPageViewModel()
    {
        MediaPath = Prefernces.MediaPath;
        TriviaPath = Prefernces.TriviaPath;

        var theme = Application.Current?.UserAppTheme ?? AppTheme.Dark;
        IsDarkTheme = theme == AppTheme.Dark;
    }

    private const string myMailAddress = "r.vries@live.com";
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

    [RelayCommand]
    private async Task OpenIssuesPage()
    {
        await Launcher.OpenAsync("https://github.com/devries48/18Months/issues");
    }

    partial void OnIsDarkThemeChanging(bool value)
    {
        if (Application.Current == null) return;

        var curTheme = Application.Current.UserAppTheme;

        if (curTheme == AppTheme.Dark && !value || curTheme == AppTheme.Light && value)
        {
            Application.Current.UserAppTheme = value ? AppTheme.Dark : AppTheme.Light;
            Prefernces.DarkTheme = value;
        }
    }
}
